using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    // Author: Herald Hamor
    // Based on the original NoteObject.cs but for the Modesto Prototype
    // Public Variables
    
    public bool canBePressed;
    // public bool isLongNote;
    public KeyCode keyToPress;

    // Private Variables
    private GameObject currButton;
    private PlayerController pCtrl;

    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
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
        // If it is pressed and the button is able to be pressed, then deactivate the button 
        if(Input.GetKeyDown(keyToPress) && canBePressed)
        {
            // Deactivates game object, but can also destroy the gameObject directly
            Debug.Log("Note hit!");
            Destroy(gameObject);
            //gameObject.SetActive(false);

            // Add accuracy calculation
            // Debug statement to determine position of the note respective to the button
            if (currButton = pCtrl.greenButton)
            {
                Debug.Log(Mathf.Abs(transform.position.z - currButton.transform.position.z));
            }
            // Calculating accuracy based on z values
            float currDistance = Mathf.Abs(transform.position.z - currButton.transform.position.z);

            if (currDistance < 0.1)
            {
                // If the difference is 0 then it is a perfect hit
                Debug.Log("It is a perfect hit!");
                GameManager.instance.GHPerfectHit();
            } else if (currDistance > 0.1 && currDistance <= 0.2)
            {
                // If the difference is above 0 then it is a good hit
                Debug.Log("It is a good hit!");
                GameManager.instance.GHGoodHit();
            } else if (currDistance > 0.2 && currDistance < 0.3)
            {
                // If the difference is 0.1 or above then it is a normal hit
                Debug.Log("It is a normal hit!");
                GameManager.instance.GHNormalHit();
            } else if (currDistance >= 0.3)
            {
                // If the difference is 0.2 or above then it is a bad hit
                Debug.Log("It is a bad hit!");
                GameManager.instance.GHBadHit();
            }
        }
    }

    // Collision detection when note collides with button
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NoteButton")
        {
            canBePressed = true;
            //Debug.Log("Button can be pressed!");
        }
    }

    // Collision detection when note exits button's collider
    // canBePressed is false and deletes/deactivates the note
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NoteButton")
        {
            canBePressed = false;
            //Debug.Log("Button cannot be pressed! Note missed!");
            GameManager.instance.NoteMissed(other.gameObject);
            StartCoroutine(DestroyNote());
        } 
    }

    // Coroutine that deletes/deactivates the note
    IEnumerator DestroyNote()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
