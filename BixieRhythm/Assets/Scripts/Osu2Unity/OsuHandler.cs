using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class OsuHandler : MonoBehaviour
{
    // ----------------------------------------------------------------------------
    [Header("Beat Map")]
    public string SongFileName;
    public AudioClip MainMusic; // Music file, attach from editor
    public AudioClip HitSound; // Hit sound

    [Header("Objects")]
    public GameObject Circle; // Circle Object
    public GameObject goodObject; // Circle Object
    public GameObject perfectObject; // Circle Object
    public GameObject poorObject; // Circle Object

    [Header("Score Text")]
    public Text scoreText;
    public Text accText;
    public Text multiplierText;
    

    // ----------------------------------------------------------------------------

    const int SPAWN = -100; // Spawn coordinates for objects

    public static double timer = 0; // Main song timer
    public static int ApprRate = 600; // Approach rate (in ms)
    private int DelayPos = 0; // Delay song position
    public static int timeAfterDeath = 200;
    public static int perfectRange = 75; //The higher the more forgiving the lower the more precise

    public static int ClickedCount = 0; // Clicked objects counter
    private static int ObjCount = 0; // Spawned objects counter

    private List<GameObject> CircleList; // Circles List
    private static string[] LineParams; // Object Parameters

    // Audio stuff
    private AudioSource Sounds;
    public AudioSource Music;
    public static AudioSource pSounds;
    public static AudioClip pHitSound;

    // Other stuff
    private Camera MainCamera;
    private GameObject CursorTrail;
    private Vector3 MousePosition;
    private Ray MainRay;
    private RaycastHit MainHit;
    private int currentScore = 0;
    [Header("Scoring")]
    public int scorePerBadNote = 10;
    public int scorePerGoodNote = 30;
    public int scorePerPerfectNote = 50;
    public GameManager gameManager;

    public int currMultiplier;
    public int multTracker;
    public int[] multThreshold;

    [SerializeField] Texture2D cursor;

    private void Start()
    {
        Vector2 hotSpot = new Vector2(cursor.width / 2f, cursor.height / 2f);
        Cursor.SetCursor(cursor, hotSpot, CursorMode.ForceSoftware);

        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Music = GameObject.Find("Music Source").GetComponent<AudioSource>();
        Sounds = gameObject.GetComponent<AudioSource>();
        CursorTrail = GameObject.Find("Cursor Trail");
        Music.clip = MainMusic;
        pSounds = Sounds;
        pHitSound = HitSound;
        CircleList = new List<GameObject>();
        scoreText.text = "Score: " + currentScore;

        currMultiplier = 1;
        multiplierText.text = "Multiplier: x" + currMultiplier;
        multThreshold = new int[4] { 2, 4, 6, 8 };

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //string rip = "Assets/Resources/Dallas Truong - Bixie Track 1 (Niko Sanchez) [Normal].osu";
        string rip = Application.streamingAssetsPath + "/" + SongFileName;
        //string rip = Application.streamingAssetsPath + "/TestingPurposes.osu";
        
        ReadCircles(rip);
    }

    // MAP READER
    void ReadCircles(string path)
    {
        StreamReader reader = new StreamReader(path);
        string line;

        // Skip to [HitObjects]
        while(true)
        {
            if (reader.ReadLine() == "[HitObjects]")
                break;
        }
            
        int TotalLines = 0;

        // Count all lines
        while (!reader.EndOfStream)
        {
            reader.ReadLine();
            TotalLines++;
        }

        reader.Close();

        reader = new StreamReader(path);

        // Skip to [HitObjects] part again
        while(true)
        {
            if (reader.ReadLine() == "[HitObjects]")
                break;
        }

        // Sort objects on load
        int ForeOrder = TotalLines + 2; // Sort foreground layer
        int BackOrder = TotalLines + 1; // Sort background layer
        int ApproachOrder = TotalLines; // Sort approach circles layer

        // Some crazy Z axis modifications for sorting
        string TotalLinesStr = "0.";
        for (int i = 3; i > TotalLines.ToString().Length; i--)
            TotalLinesStr += "0";
        TotalLinesStr += TotalLines.ToString();
        float Z_Index = -(float.Parse(TotalLinesStr));

        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            if (line == null){
                Debug.Log("LINE IS NULL.");
                break;
            }
            else{
                //Debug.Log("Current Line: " + line);
            }

            // x,y,time,type,hitSound,objectParams,hitSample
            // x,y,time,type,hitSound,curveType|curvePoints,slides,length,edgeSounds,edgeSets,hitSample
            LineParams = line.Split(',','|',':'); // Line parameters (X&Y axis, time position)
            //Debug.Log(LineParams.Length);

            
            if(Regex.IsMatch(LineParams[5], @"^[a-zA-Z]+$")){ //Check if this is either a click or a slider
                //Debug.Log("String has a letter");

            }
            else{
                //Debug.Log("No Letter");
                
                // Sorting configuration
                GameObject CircleObject = Instantiate(Circle, new Vector2(SPAWN, SPAWN), Quaternion.identity);
                CircleObject.GetComponent<Circle>().Fore.sortingOrder = ForeOrder;
                CircleObject.GetComponent<Circle>().Back.sortingOrder = BackOrder;
                CircleObject.GetComponent<Circle>().Appr.sortingOrder = ApproachOrder;
                CircleObject.transform.localPosition += new Vector3((float) CircleObject.transform.localPosition.x, (float) CircleObject.transform.localPosition.y, (float) Z_Index);
                CircleObject.transform.SetAsFirstSibling();
                
                
                ForeOrder--; BackOrder--; ApproachOrder--; Z_Index += 0.01f;

                int FlipY = 384 - int.Parse(LineParams[1]); // Flip Y axis
                int AdjustedX = Mathf.RoundToInt(Screen.height * 1.333333f); // Aspect Ratio

                // Padding
                float Slices = 8f;
                float PaddingX = AdjustedX / Slices;
                float PaddingY = Screen.height / Slices;

                // Resolution set
                float NewRangeX = ((AdjustedX - PaddingX) - PaddingX);
                float NewValueX = ((int.Parse(LineParams[0]) * NewRangeX) / 512f) + PaddingX + ((Screen.width - AdjustedX) / 2f);
                float NewRangeY = Screen.height;
                float NewValueY = ((FlipY * NewRangeY) / 512f) + PaddingY;

                Vector3 MainPos = MainCamera.ScreenToWorldPoint(new Vector3 (NewValueX, NewValueY, 0)); // Convert from screen position to world position
                Circle MainCircle = CircleObject.GetComponent<Circle>();

                MainCircle.Set(MainPos.x, MainPos.y, CircleObject.transform.position.z, int.Parse(LineParams[2]) - ApprRate);

                
                CircleList.Add(CircleObject);
            }

        }
        GameStart();
    }

    //private void AddSliderToScene{

    //}

    // END MAP READER
	
    private void GameStart()
    {
        Application.targetFrameRate = 60; // Target Framerate
        Music.Play();
        StartCoroutine(UpdateRoutine()); // Using coroutine instead of Update()
    }

    public void multHelper(string mode){

        switch (mode)
        {
            case "add":
                if (currMultiplier - 1 < multThreshold.Length)
                {
                    multTracker++;

                    if (multThreshold[currMultiplier - 1] <= multTracker)
                    {
                        currMultiplier++;
                        multTracker = 0;
                    }
                }
                multiplierText.text = "Multiplier: x" + currMultiplier;
                break;

            case "reset":
                currMultiplier = 1;
                multTracker = 0;
                multiplierText.text = "Multiplier: x" + currMultiplier;
                break;

        }
        
    }

    private IEnumerator UpdateRoutine()
    {
        while (true)
        {
            timer = (Music.time * 1000); // Convert timer
            DelayPos = (CircleList[ObjCount].GetComponent<Circle>().PosA);
            MainRay = MainCamera.ScreenPointToRay(Input.mousePosition);

            if(ObjCount != CircleList.Count){
                        
                // Spawn object
                if (timer >= DelayPos)
                {
                    CircleList[ObjCount].GetComponent<Circle>().Spawn();
                    //CircleList[ObjCount].transform.parent = MainCamera.transform;
                    if(ObjCount+1 < CircleList.Count){
                        ObjCount++;        
                    }
                }
            }

            // Check if cursor is over object 
            if (Physics.Raycast(MainRay, out MainHit))
            {
                // EARLY HIT
                if (MainHit.collider.name == "Circle(Clone)" && (timer + perfectRange) < (MainHit.collider.gameObject.GetComponent<Circle>().PosA + ApprRate))
                {
                    if (Input.GetMouseButtonDown(0)){
                        //Debug.Log("TIME HIT: " + timer + 
                        //        "\nAdditive: " + (MainHit.collider.gameObject.GetComponent<Circle>().PosA + ApprRate));
                        //Debug.Log("EARLY");
                        //multHelper("add");

                        gameManager.currentGHScore += scorePerGoodNote;
                        gameManager.GLOBAL_noteHit();
                        gameManager.ChangePortrait("Normal", 1);
                        scoreText.text = "Score: " + currentScore;

                        GameObject goodObj = Instantiate(goodObject) as GameObject;
                        goodObj.transform.position = MainHit.transform.position;
                        
                        MainHit.collider.gameObject.GetComponent<Circle>().Got();
                        MainHit.collider.enabled = false;
                        ClickedCount++;
                        
                    }
                    
                }

                // LATE HIT
                else if (MainHit.collider.name == "Circle(Clone)" && (timer - perfectRange) > MainHit.collider.gameObject.GetComponent<Circle>().PosA + ApprRate)
                {
                    if (Input.GetMouseButtonDown(0)){
                        //Debug.Log("TIME HIT: " + timer + 
                        //        "\nAdditive: " + (MainHit.collider.gameObject.GetComponent<Circle>().PosA + ApprRate));
                        //Debug.Log("LATE");
                        //multHelper("add");

                        gameManager.currentGHScore += scorePerBadNote;
                        gameManager.GLOBAL_noteHit();
                        gameManager.ChangePortrait("Normal", 1);
                        scoreText.text = "Score: " + currentScore;

                        GameObject poorObj = Instantiate(poorObject) as GameObject;
                        poorObj.transform.position = MainHit.transform.position;

                        MainHit.collider.gameObject.GetComponent<Circle>().Got();
                        MainHit.collider.enabled = false;
                        ClickedCount++;
                    }
                    
                }

                // PERFECT HIT
                else if (MainHit.collider.name == "Circle(Clone)") //&& 
                    //(timer !>= MainHit.collider.gameObject.GetComponent<Circle>().PosA + ApprRate - perfectRange && timer !<= MainHit.collider.gameObject.GetComponent<Circle>().PosA + ApprRate - perfectRange))
                {
                    if (Input.GetMouseButtonDown(0)){
                        //Debug.Log("TIME HIT: " + timer + 
                                //"\nAdditive: " + (MainHit.collider.gameObject.GetComponent<Circle>().PosA + ApprRate));
                        //Debug.Log("PERFECT");
                        //multHelper("add");
                        gameManager.ChangePortrait("Perfect", 1);

                        gameManager.currentGHScore += scorePerPerfectNote;
                        gameManager.GLOBAL_noteHit();
                        scoreText.text = "Score: " + currentScore;

                        GameObject perfectObj = Instantiate(perfectObject) as GameObject;
                        perfectObj.transform.position = MainHit.transform.position;

                        MainHit.collider.gameObject.GetComponent<Circle>().Got();
                        MainHit.collider.enabled = false;
                        ClickedCount++;
                    }
                    
                }

                // // LATE HIT ??
                // else if (MainHit.collider.name == "Circle(Clone)" && timer > MainHit.collider.gameObject.GetComponent<Circle>().PosA + ApprRate)
                // {
                //     if (Input.GetMouseButtonDown(0)){
                //         MainHit.collider.gameObject.GetComponent<Circle>().Got();
                //         MainHit.collider.enabled = false;
                //         ClickedCount++;
                //         Debug.Log("TIMER: " + timer + "\nPosA: " + MainHit.collider.gameObject.GetComponent<Circle>().PosA);
                //         Debug.Log("LATE");
                //     }
                    
                // }
            }

            // Cursor trail movement
            MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            CursorTrail.transform.position = new Vector3(MousePosition.x, MousePosition.y, -9);

            yield return null;

        }


    }


}
