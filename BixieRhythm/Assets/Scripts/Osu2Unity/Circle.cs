using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    // Circle parameters
    private float PosX = 0;
    private float PosY = 0;
    private float PosZ = 0;
    [HideInInspector]
    public int PosA = 0;
    public OsuHandler OsuHandler;
    public GameManager gameManager;

    private Color MainColor, MainColor1, MainColor2, LanternColor; // Circle sprites color
    public GameObject MainApproach, MainFore, MainBack, LanternGameObj; // Circle objects

    public GameObject missObject; // Circle Object

    [HideInInspector]
    public SpriteRenderer Fore, Back, Appr; // Circle sprites
    [HideInInspector]
    public Renderer LanternRenderer;
    private BoxCollider box;

    // Checker stuff
    private bool RemoveNow = false;
    private bool GotIt = false;

    private void Awake()
    {
        Fore = MainFore.GetComponent<SpriteRenderer>();
        Back = MainBack.GetComponent<SpriteRenderer>();
        Appr = MainApproach.GetComponent<SpriteRenderer>();
        LanternRenderer = LanternGameObj.GetComponent<Renderer>();

        box = this.GetComponent<BoxCollider>();
        OsuHandler = GameObject.Find("Script Handler").GetComponent<OsuHandler>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Set circle configuration
    public void Set(float x, float y, float z, int a)
    {
        MainColor = Appr.color;
        MainColor1 = Fore.color;
        MainColor2 = Back.color;
        LanternColor = LanternRenderer.material.color;

        PosX = x;
        PosY = y;
        PosZ = z;
        PosA = a;   
    }

    // Spawning the circle
    public void Spawn()
    {
        gameObject.transform.position = new Vector3(PosX, PosY, PosZ);
        this.enabled = true;
        StartCoroutine(Checker());
    }

    // If circle wasn't clicked
    public void Remove ()
    {
        if (!GotIt)
        {
            RemoveNow = true;
            this.enabled = true;
            //this.collider.enabled = false;
        }
    }

    // If circle was clicked
    public void Got ()
    {
        if (!RemoveNow)
        {
            GotIt = true;
            MainApproach.transform.position = new Vector2(-101, -101);
            OsuHandler.pSounds.PlayOneShot(OsuHandler.pHitSound);
            RemoveNow = false;
            this.enabled = true;
        }
    }

    // Check if circle wasn't clicked
    private IEnumerator Checker()
    {
        while (true)
        {
            if (OsuHandler.timer >= PosA + (OsuHandler.ApprRate + OsuHandler.timeAfterDeath) && !GotIt)
            {
                Remove();
                OsuHandler.ClickedCount++;
                break;
            }
            yield return null;
        }
    }

    // Main Update for circles
    private void Update ()
    {
        // Approach Circle modifier
        if (MainApproach.transform.localScale.x >= 0.9f)
        {
            MainApproach.transform.localScale -= new Vector3(5.166667f, 5.166667f, 0f) * Time.deltaTime;
            MainColor.a += 4f * Time.deltaTime;
            MainColor1.a += 4f * Time.deltaTime;
            MainColor2.a += 4f * Time.deltaTime;
            if(LanternColor.a < 1f) LanternColor.a += 4f * Time.deltaTime;
            //MainLant1.a += 4f * Time.deltaTime;

            Fore.color = MainColor1;
            Back.color = MainColor2;
            Appr.color = MainColor;
            LanternRenderer.material.color = LanternColor;
            //Lant1.material.color = MainLant1;

        }
        // If circle wasn't clicked
        else if (!GotIt)
        {
            // Remove circle
            if (!RemoveNow)
            {
                MainApproach.transform.position = new Vector2(-101, -101);
                this.enabled = false;
                
            }
            // If circle wasn't clicked
            else
            {
                box.enabled = false;
                MainColor1.a -= 10f * Time.deltaTime;
                MainColor2.a -= 10f * Time.deltaTime;
                LanternColor.a -= 3f * Time.deltaTime;

                MainFore.transform.localPosition += (Vector3.down * 2) * Time.deltaTime;
                MainBack.transform.localPosition += Vector3.down * Time.deltaTime;
                LanternGameObj.transform.localPosition += (Vector3.down * 2) * Time.deltaTime;

                Fore.color = MainColor1;
                Back.color = MainColor2;
                LanternRenderer.material.color = LanternColor;

                if (MainColor1.a <= 0f)
                {
                    OsuHandler.multHelper("reset");

                    GameObject missObj = Instantiate(missObject) as GameObject;
                    missObj.transform.position = this.transform.position;
                    gameManager.GLOBAL_noteMissed();

                    gameObject.transform.position = new Vector2(-101, -101);
                    this.enabled = false;
                }
            }
        }
        // If circle was clicked
        if (GotIt)
        {
            MainColor1.a -= 10f * Time.deltaTime;
            MainColor2.a -= 10f * Time.deltaTime;
            LanternColor.a -= 3f * Time.deltaTime;

            MainFore.transform.localScale += new Vector3(2, 2, 0) * Time.deltaTime;
            MainBack.transform.localScale += new Vector3(2, 2, 0) * Time.deltaTime;
            LanternGameObj.transform.localScale += new Vector3(3/2, 3/2, 3/2) * Time.deltaTime;

            Fore.color = MainColor1;
            Back.color = MainColor2;
            LanternRenderer.material.color = LanternColor;

            if (MainColor1.a <= 0f)
            {
                gameObject.transform.position = new Vector2(-101, -101);
                this.enabled = false;
            }
        }
    }
}