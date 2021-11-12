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
    public Text HPText;
    public Text multiplierText;

    // Sprite GameObjects
    public Image QinyangPortrait;
    public Image MeiLienPortrait;
    public Image HPBar;

    // Private Variables
    private int currentGHScore = 0;
    private int currentOSScore = 0;
    private int playerHP = 100;
        // Note Accuracy - Accuracy variables
        /*private float maxNotes;
        private int noteAccuracy;
        private float currentNotes; */
    private int scorePerBadNote = 50;
    private int scorePerNote = 100;
    private int scorePerGoodNote = 150;
    private int scorePerPerfectNote = 200;

    private int currMultiplier;
    private int multTracker;
    private int[] multThreshold;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        GHScoreText.text = "Score: " + currentGHScore;
        currMultiplier = 1;
        multiplierText.text = "Multiplier: x" + currMultiplier;
        multThreshold = new int[4] { 2, 4, 6, 8 };
        HPText.text = "" + playerHP;

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

    
    // Note hitting function that passes the modifiers of previous functions (BadHit - PerfectHit) and changes score/multiplier numbers accordingly
    public void NoteHit()
    {
        Debug.Log("Hit on time!");

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
        currentGHScore += scorePerNote * currMultiplier;
        GHScoreText.text = "Score: " + currentGHScore;

        if(playerHP < 100)
        {
            playerHP += 1;
            HPText.text = "" + playerHP;
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
    public void GHBadHit()
    {
        currentGHScore += scorePerBadNote * currMultiplier;
        ChangePortrait("Bad", 0);
        NoteHit();
    }

    public void GHNormalHit()
    {
        currentGHScore += scorePerNote * currMultiplier;
        ChangePortrait("Normal", 0);
        NoteHit();
    }

    public void GHGoodHit()
    {
        currentGHScore += scorePerGoodNote * currMultiplier;
        ChangePortrait("Normal", 0);
        NoteHit();
    }
    public void GHPerfectHit()
    {
        currentGHScore += scorePerPerfectNote * currMultiplier;
        ChangePortrait("Perfect", 0);
        NoteHit();
    }

    // If a player misses a note, then the accuracy count is decreased.
    public void NoteMissed()
    {
        //Debug.Log("Missed note!");
        ChangePortrait("Bad", 0);
        currMultiplier = 1;
        multTracker = 0;
        multiplierText.text = "Multiplier: x" + currMultiplier;

        if(playerHP > 0)
        {
            playerHP -= 5;
            HPText.text = "" + playerHP;
        }

        // Note Accuracy - Updates accuracy value if 
        /* currentNotes -= 1;
        noteAccuracy = Mathf.FloorToInt((currentNotes / maxNotes) * 100);
        accText.text = "Accuracy: " + noteAccuracy + "%"; */
    }
}
