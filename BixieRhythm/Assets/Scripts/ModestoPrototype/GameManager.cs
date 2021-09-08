using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author: Herald Hamor
public class GameManager : MonoBehaviour
{
    // Public Variables
    public AudioSource currMusic;
    public bool startPlaying;
    public NoteScroller nScroll;
    public static GameManager instance;

    public Text scoreText;
    public Text multiplierText;

    // Private Variables
    private int currentScore = 0;
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
        scoreText.text = "Score: " + currentScore;
        currMultiplier = 1;
        multiplierText.text = "Multiplier: x" + currMultiplier;
        multThreshold = new int[4] { 2, 4, 6, 8 };
    }

    // Update is called once per frame
    void Update()
    {
        // If the scene has not started playing, then the player as the option to start the level
        // Notes will start to scroll + Music will play
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                nScroll.hasStarted = true;
                //currMusic.Play();
            }
        }
    }

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
        //currentScore += scorePerNote * currMultiplier;
        scoreText.text = "Score: " + currentScore;
    }

    // Different types of hits based on accuracy
    public void NormalHit()
    {
        currentScore += scorePerNote * currMultiplier;
        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currMultiplier;
        NoteHit();
    }
    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currMultiplier;
        NoteHit();
    }

    public void NoteMissed()
    {
        Debug.Log("Missed note!");
        currMultiplier = 1;
        multTracker = 0;
        multiplierText.text = "Multiplier: x" + currMultiplier;
    }
}
