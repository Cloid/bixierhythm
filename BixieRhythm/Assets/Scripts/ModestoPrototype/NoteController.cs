using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    // Author: Herald Hamor
    // Based on the original NoteObject.cs but for the Modesto Prototype
    // Public Variables
    public bool canBePressed;
    public bool canBeReleased;
    // public bool isLongNote;
    public KeyCode keyToPress;

    // Private Variables
    private GameObject currButton;
    private PlayerController pCtrl;
    private float noteModifier;
    private float chordModifier;

    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
        canBeReleased = false;
        pCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // Determines button assignment based on color/key
        switch (keyToPress.ToString())
        {
            case "Q":
                currButton = pCtrl.greenButton;
                break;
            case "W":
                currButton = pCtrl.redButton;
                break;
            case "E":
                currButton = pCtrl.yellowButton;
                break;
            case "R":
                currButton = pCtrl.blueButton;
                break;
            case "T":
                currButton = pCtrl.orangeButton;
                break;
            default:
                currButton = pCtrl.greenButton;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If it is pressed and the button is able to be pressed, then deactivate the note and calculate the score
        if(Input.GetKeyDown(keyToPress) && canBePressed)
        {
            // Deactivates game object, but can also destroy the gameObject directly
            //Debug.Log("Note hit!");
            //gameObject.SetActive(false);

            if (gameObject.CompareTag("Note") || (gameObject.CompareTag("ChordNote") && GameManager.instance.GHIsOnChord))
            {
                //Debug.Log("Hit a note!");
                Destroy(gameObject);
                // Add accuracy calculation
                // Debug statement to determine position of the note respective to the button
                /*if (currButton = pCtrl.greenButton)
                {
                    //Debug.Log(Mathf.Abs(transform.position.z - currButton.transform.position.z));
                }*/
                // Calculating accuracy based on z values
                float currDistance = Mathf.Abs(transform.position.z - currButton.transform.position.z);

                if (currDistance < 0.1)
                {
                    // If the difference is 0 then it is a perfect hit
                    //Debug.Log("It is a perfect hit!");
                    noteModifier = 2f;
                    GameManager.instance.GHPerfectHit(noteModifier);
                }
                else if (currDistance > 0.1 && currDistance <= 0.2)
                {
                    // If the difference is above 0 then it is a good hit
                    //Debug.Log("It is a good hit!");
                    noteModifier = 1.5f;
                    GameManager.instance.GHGoodHit(noteModifier);
                }
                else if (currDistance > 0.2 && currDistance < 0.3)
                {
                    // If the difference is 0.1 or above then it is a normal hit
                    //Debug.Log("It is a normal hit!");
                    noteModifier = 1f;
                    GameManager.instance.GHNormalHit(noteModifier);
                }
                else if (currDistance >= 0.3)
                {
                    // If the difference is 0.2 or above then it is a bad hit
                    //Debug.Log("It is a bad hit!");
                    noteModifier = 0.5f;
                    GameManager.instance.GHBadHit(noteModifier);
                }
            }
        }

        if(Input.GetKeyDown(keyToPress) && GameManager.instance.GHIsOnChord)
        {
            StartCoroutine(ChordNoteTrack());
        }

        if(Input.GetKeyUp(keyToPress) && canBeReleased)
        {
            Destroy(gameObject);
            float currDistance = Mathf.Abs(transform.position.z - currButton.transform.position.z);

            Debug.Log(chordModifier);

            if (currDistance < 0.1)
            {
                // If the difference is 0 then it is a perfect hit
                //Debug.Log("It is a perfect hit!");
                GameManager.instance.GHPerfectHit(chordModifier);
            }
            else if (currDistance > 0.1 && currDistance <= 0.2)
            {
                // If the difference is above 0 then it is a good hit
                //Debug.Log("It is a good hit!");
                GameManager.instance.GHGoodHit(chordModifier);
            }
            else if (currDistance > 0.2 && currDistance < 0.3)
            {
                // If the difference is 0.1 or above then it is a normal hit
                //Debug.Log("It is a normal hit!");
                GameManager.instance.GHNormalHit(chordModifier);
            }
            else if (currDistance >= 0.3)
            {
                // If the difference is 0.2 or above then it is a bad hit
                //Debug.Log("It is a bad hit!");
                GameManager.instance.GHBadHit(chordModifier);
            }

            chordModifier = 0;
        }
    }

    // Collision detection when note collides with button
    private void OnTriggerEnter(Collider other)
    {
        // Checks if the button is a note button and the note is tagged as a normal note
        // Activates note's pressed variable, allowing it to be hit
        if (other.tag == "NoteButton" && gameObject.tag == "Note")
        {
            canBePressed = true;
            //Debug.Log("Note can be pressed!");
        }
        // Checks if the button is a note button and the note is tagged as a chord note
        // If a chord hasn't started, then it's a starting chord note and must be pressed
        // If a chord has started, then it's an ending chord note and must be released
        else if (other.tag == "NoteButton" && gameObject.tag == "ChordNote")
        {
            //Debug.Log(GameManager.instance.GHIsOnChord);

            if (!GameManager.instance.GHIsOnChord && !canBePressed)
            {
                canBePressed = true;
                Debug.Log("ChordNote can be pressed!");
            }
            else if (GameManager.instance.GHIsOnChord && !canBePressed)
            {
                canBeReleased = true;
                Debug.Log("ChordNote can be released!");
            }
        }
        // Checks if the button is a note button and the note is tagged as a chord 
        // Starts the chord state for GameManager
        else if (other.tag == "NoteButton" && gameObject.tag == "Chord")
        {
            GameManager.instance.GHIsOnChord = true;
        }
    }

    // Collision detection when note exits button's collider
    // canBePressed is false and deletes/deactivates the note
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NoteButton") && !gameObject.CompareTag("Chord"))
        {
            canBePressed = false;
            //Debug.Log("Button cannot be pressed! Note missed!");
            StartCoroutine(DestroyNote(other));
        } 

        if(other.CompareTag("NoteButton") && gameObject.CompareTag("Chord"))
        {
            GameManager.instance.GHIsOnChord = false;
            StartCoroutine(DestroyChord());
        }
    }

    IEnumerator ChordNoteTrack()
    {
        while (GameManager.instance.GHIsOnChord)
        {
            chordModifier += 0.01f;
            yield return null;
        }
    }

    IEnumerator DestroyChord()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

     // Coroutine that deletes/deactivates the note
    IEnumerator DestroyNote(Collider other)
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.NoteMissed(other.gameObject);
        Destroy(gameObject);
    }
}
