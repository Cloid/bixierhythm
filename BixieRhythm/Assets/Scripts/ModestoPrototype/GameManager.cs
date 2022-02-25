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

    // Private Variables
    private int currentGHScore = 0;
    private int currentOSScore = 0;
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

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        GHScoreText.text = "Score: " + currentGHScore;
        currGHMultiplier = 1;
        multiplierText.text = "Multiplier: x" + currGHMultiplier;
        multThreshold = new int[3] {2, 4, 8};
        HPText.text = "" + currentPlayerHP;

        //  Note Accuracy -  This is delayed with a coroutine because the notes spawn after the first frame
        // StartCoroutine(UpdateAcc());
    }

    // Update is called once per frame
    void Update()
    {

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

    // Helper function that changes portrait sprite based on game state (notes missed or combos)
    private void ChangePortrait(string portraitCase, int playerType)
    {
        // Gets current portrait based on note feedback and whether it is Qinyang or Mei Lien that is reacting
        switch (portraitCase)
        {
            case "Perfect":
                if(playerType == 0)
                {
                    QinyangPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/q_perfcombo");
                } else
                {
                    MeiLienPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/m_perfcombo");
                }
                break;
            case "Normal":
                if (playerType == 0)
                {
                    QinyangPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/q_normal");
                }
                else
                {
                    MeiLienPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/m_normal");
                }
                break;
            case "Bad":
                if (playerType == 0)
                {
                    QinyangPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/q_hit");
                }
                else
                {
                    MeiLienPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/m_hit");
                }
                break;
            default:
                if (playerType == 0)
                {
                    QinyangPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/q_normal");
                }
                else
                {
                    MeiLienPortrait.sprite = Resources.Load<Sprite>("Art/Sprites/Portraits/m_normal");
                }
                break;
        }
    }

    // Different types of hits based on individual note accuracy
    // Instantiates note feedback and then deletes it
    public void GHBadHit()
    {
        currentGHScore += scorePerBadNote * currGHMultiplier;
        ChangePortrait("Bad", 0);
        GameObject clone = Instantiate(BadNote, GHPlayer.transform.GetChild(7).position, BadNote.transform.rotation);
        noteFeedbackTranslate(clone);
        Destroy(clone, 1f);
        NoteHit();
    }

    public void GHNormalHit()
    {
        currentGHScore += scorePerNote * currGHMultiplier;
        ChangePortrait("Normal", 0);
        GameObject clone = Instantiate(OkayNote, GHPlayer.transform.GetChild(7).position, OkayNote.transform.rotation);
        noteFeedbackTranslate(clone);
        Destroy(clone, 1f);
        NoteHit();
    }

    public void GHGoodHit()
    {
        currentGHScore += scorePerGoodNote * currGHMultiplier;
        ChangePortrait("Normal", 0);
        GameObject clone = Instantiate(GreatNote, GHPlayer.transform.GetChild(7).position, GreatNote.transform.rotation);
        noteFeedbackTranslate(clone);
        Destroy(clone, 1f);
        NoteHit();
    }
    public void GHPerfectHit()
    {
        currentGHScore += scorePerPerfectNote * currGHMultiplier;
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
        currGHMultiplier = 1;
        multGHTracker = 0;
        GHComboText.text = "Combo: " + multGHTracker;
        multiplierText.text = "Multiplier: x" + currGHMultiplier;

        if(currentPlayerHP > 0)
        {
            currentPlayerHP -= 5;
            updateHPBar();
        }

        // Note Accuracy - Updates accuracy value if 
        /* currentNotes -= 1;
        noteAccuracy = Mathf.FloorToInt((currentNotes / maxNotes) * 100);
        accText.text = "Accuracy: " + noteAccuracy + "%"; */
    }
}
