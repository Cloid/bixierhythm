 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author: Herald Hamor
public class GameManager : MonoBehaviour
{
    // Public Variables
    // General GameObjects
    public GameObject chartLoader;
    public static GameManager instance;

    // Score GameObjects are split based on type of rhythm game (GH = Guitar Hero, OS = Osu)
    public Text GHScoreText;
    // Note Accuracy - public Text accText;
    public Text GHComboText;
    public Text HPText;
    public Text multiplierText;

    // Sprite GameObjects
    public Image QinyangPortrait;
    public Image MeiLienPortrait;
    public Image HPBar;
    public GameObject PerfectNote;
    public GameObject GreatNote;
    public GameObject OkayNote;
    public GameObject BadNote;
    public GameObject MissedNote;
    public GameObject GHPlayer;
    public bool GHIsOnChord;
    public Image Tutorial;
    public bool isTutorialActive;
    public bool isGamePaused;

    // Private Variables
    public int currentGHScore = 0;
    //private int currentOSScore = 0;
    private float maxPlayerHP = 100;
    private float currentPlayerHP = 100;
        // Note Accuracy - Accuracy variables
        /*private float maxNotes;
        private int noteAccuracy;
        private float currentNotes; */
    private int scorePerBadNote = 50;
    private int scorePerNote = 100;
    private int scorePerGoodNote = 150;
    private int scorePerPerfectNote = 200;

    private int currGHMultiplier;
    private int multGHTracker;
    private int[] multThreshold;

    // Tutorial time value
    private float tutorialTime;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        GHScoreText.text = "Score: " + currentGHScore;
        currGHMultiplier = 1;
        multiplierText.text = "Multiplier: x" + currGHMultiplier;
        multThreshold = new int[3] {2, 4, 8};
        HPText.text = "" + currentPlayerHP;
        GHIsOnChord = false;
        isTutorialActive = true;
        tutorialTime = Time.unscaledTime;

        //  Note Accuracy -  This is delayed with a coroutine because the notes spawn after the first frame
        // StartCoroutine(UpdateAcc());
    }

    // Update is called once per frame
    void Update()
    {
        if (isTutorialActive)
        {
            isGamePaused = true;
            PauseGame();
        }

        if((Time.unscaledTime - tutorialTime) > 5f && isTutorialActive)
        {
            isTutorialActive = false;
            isGamePaused = false;
            PauseGame();
            Tutorial.enabled = false;
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    // Note Accuracy - Determines overall accuracy of the player based on how many notes were missed by the player vs the notes spawned by ChartEditor
    /* IEnumerator UpdateAcc()
    {
        yield return new WaitForSeconds(0.01f);
        maxNotes = chartLoader.transform.childCount;
        currentNotes = maxNotes;
        noteAccuracy = Mathf.FloorToInt((currentNotes / maxNotes) * 100);
        accText.text = "Accuracy: " + noteAccuracy + "%";
    } */

    // Pauses the game 
    public void PauseGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        } else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }

    // Helper function that changes the HP bar
    public void updateHPBar()
    {
        HPBar.fillAmount = currentPlayerHP / maxPlayerHP;
        HPText.text = "" + currentPlayerHP;
    }
    
    // Note hitting function that passes the modifiers of previous functions (BadHit - PerfectHit) and changes score/multiplier numbers accordingly
    public void NoteHit()
    {
        //Debug.Log("Hit on time!");

        if (currGHMultiplier - 1 < multThreshold.Length)
        {
            multGHTracker++;
            //Debug.Log(multGHTracker);

            if (multThreshold[currGHMultiplier - 1] <= multGHTracker)
            {
                currGHMultiplier++;
                multGHTracker = 0;
                //Debug.Log("You advanced to next multiplier!");
            }
        }

        GHComboText.text = "Combo: " + multGHTracker;
        multiplierText.text = "Multiplier: x" + currGHMultiplier;
        currentGHScore += scorePerNote * currGHMultiplier;
        GHScoreText.text = "Score: " + currentGHScore;

        if(currentPlayerHP < 100)
        {
            currentPlayerHP += 1;
            updateHPBar();
        }
    }

    public void GLOBAL_noteHit(){

        if (currGHMultiplier - 1 < multThreshold.Length)
        {
            multGHTracker++;
            //Debug.Log(multGHTracker);

            if (multThreshold[currGHMultiplier - 1] <= multGHTracker)
            {
                currGHMultiplier++;
                multGHTracker = 0;
                //Debug.Log("You advanced to next multiplier!");
            }
        }

        GHComboText.text = "Combo: " + multGHTracker;
        multiplierText.text = "Multiplier: x" + currGHMultiplier;
        GHScoreText.text = "Score: " + currentGHScore;

        if(currentPlayerHP < 100)
        {
            currentPlayerHP += 1;
            updateHPBar();
        }
    }

    // Helper function that changes portrait sprite based on game state (notes missed or combos)
    private void ChangePortrait(string portraitCase, int playerType)
    {
        // Gets current portrait based on note feedback and whether it is Qinyang or Mei Lien that is reacting
        switch (portraitCase)
        {
            case "Perfect":
                if(playerType == 0)
                {
                    QinyangPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/V2/q_happy2");
                } else
                {
                    MeiLienPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/V2/m_happy2");
                }
                break;
            case "Normal":
                if (playerType == 0)
                {
                    QinyangPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/V2/q_neutral2");
                }
                else
                {
                    MeiLienPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/V2/m_neutral2");
                }
                break;
            case "Bad":
                if (playerType == 0)
                {
                    QinyangPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/V2/q_hit2");
                }
                else
                {
                    MeiLienPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/V2/m_hit2");
                }
                break;
            default:
                if (playerType == 0)
                {
                    QinyangPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/V2/q_neutral2");
                }
                else
                {
                    MeiLienPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/V2/m_neutral2");
                }
                break;
        }
    }

    // Different types of hits based on individual note accuracy
    // Instantiates note feedback and then deletes it
    public void GHBadHit(float modifier)
    {
        currentGHScore += Mathf.FloorToInt((scorePerBadNote * modifier) * currGHMultiplier);
        ChangePortrait("Bad", 0);
        GameObject clone = Instantiate(BadNote, GHPlayer.transform.GetChild(7).position, BadNote.transform.rotation);
        noteFeedbackTranslate(clone);
        Destroy(clone, 1f);
        NoteHit();
    }

    public void GHNormalHit(float modifier)
    {
        currentGHScore += Mathf.FloorToInt((scorePerNote * modifier) * currGHMultiplier);
        ChangePortrait("Normal", 0);
        GameObject clone = Instantiate(OkayNote, GHPlayer.transform.GetChild(7).position, OkayNote.transform.rotation);
        noteFeedbackTranslate(clone);
        Destroy(clone, 1f);
        NoteHit();
    }

    public void GHGoodHit(float modifier)
    {
        currentGHScore += Mathf.FloorToInt((scorePerGoodNote * modifier) * currGHMultiplier);
        ChangePortrait("Normal", 0);
        GameObject clone = Instantiate(GreatNote, GHPlayer.transform.GetChild(7).position, GreatNote.transform.rotation);
        noteFeedbackTranslate(clone);
        Destroy(clone, 1f);
        NoteHit();
    }
    public void GHPerfectHit(float modifier)
    {
        currentGHScore += Mathf.FloorToInt((scorePerPerfectNote * modifier) * currGHMultiplier);
        ChangePortrait("Perfect", 0);
        GameObject clone = Instantiate(GreatNote, GHPlayer.transform.GetChild(7).position, GreatNote.transform.rotation);
        noteFeedbackTranslate(clone);
        Destroy(clone, 1f);
        NoteHit();
    }

    // Helper function that helps the note gameobjects move along with the player
    private void noteFeedbackTranslate(GameObject clone)
    {
        clone.GetComponent<Rigidbody>().AddForce((Vector3.forward) + (Vector3.up * 0.5f), ForceMode.Impulse);
        //Debug.Log(clone.GetComponent<Rigidbody>().velocity);
    }

    // If a player misses a note, then the HP count is decreased.
    // Instantiates note feedback and then deletes it
    // Differs from other notes as it takes in the transform from the note gameObjects themselves instead of the player
    public void NoteMissed(GameObject other)
    {
        //Debug.Log("Missed note!");
        ChangePortrait("Bad", 0);
        GameObject clone = Instantiate(MissedNote, other.transform.position, MissedNote.transform.rotation);
        noteFeedbackTranslate(clone);
        Destroy(clone, 1f);
        GLOBAL_noteMissed();

        // Note Accuracy - Updates accuracy value if 
        /* currentNotes -= 1;
        noteAccuracy = Mathf.FloorToInt((currentNotes / maxNotes) * 100);
        accText.text = "Accuracy: " + noteAccuracy + "%"; */
    }

    public void GLOBAL_noteMissed(){
        currGHMultiplier = 1;
        multGHTracker = 0;
        GHComboText.text = "Combo: " + multGHTracker;
        multiplierText.text = "Multiplier: x" + currGHMultiplier;

        if(currentPlayerHP > 0)
        {
            currentPlayerHP -= 3;
            if (currentPlayerHP < 0) currentPlayerHP = 0;
            updateHPBar();
        }
    }
}
