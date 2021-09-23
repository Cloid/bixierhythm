 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author: Herald Hamor
public class GameManager : MonoBehaviour
{
    // Public Variables
    public GameObject chartLoader;
    public static GameManager instance;

    public Text scoreText;
    public Text accText;
    public Text multiplierText;

    // Private Variables
    private int currentScore = 0;
    private float maxNotes;
    private float currentNotes;
    private int noteAccuracy;
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
        scoreText.text = "Score: " + currentScore;
        currMultiplier = 1;
        multiplierText.text = "Multiplier: x" + currMultiplier;
        multThreshold = new int[4] { 2, 4, 6, 8 };

        // This is delayed with a coroutine because the notes spawn after the first frame
        StartCoroutine(UpdateAcc());
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Determines overall accuracy of the player based on how many notes were missed by the player vs the notes spawned by ChartEditor
    IEnumerator UpdateAcc()
    {
        yield return new WaitForSeconds(0.01f);
        maxNotes = chartLoader.transform.childCount;
        currentNotes = maxNotes;
        noteAccuracy = Mathf.FloorToInt((currentNotes / maxNotes) * 100);
        accText.text = "Accuracy: " + noteAccuracy + "%";
    }

    
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
        currentScore += scorePerNote * currMultiplier;
        scoreText.text = "Score: " + currentScore;
    }

    // Different types of hits based on note accuracy
    public void BadHit()
    {
        currentScore += scorePerBadNote * currMultiplier;
        NoteHit();
    }

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

    // If a player misses a note, then the accuracy count is decreased.
    public void NoteMissed()
    {
        Debug.Log("Missed note!");
        currMultiplier = 1;
        multTracker = 0;
        multiplierText.text = "Multiplier: x" + currMultiplier;
        currentNotes -= 1;
        noteAccuracy = Mathf.FloorToInt((currentNotes / maxNotes) * 100);
        accText.text = "Accuracy: " + noteAccuracy + "%";
    }
}
