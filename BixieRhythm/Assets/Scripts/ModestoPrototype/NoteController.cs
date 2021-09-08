using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    // Author: Herald Hamor
    // Based on the original NoteObject.cs but for the Modesto Prototype
    // Public Variables
    public bool canBePressed;
    public KeyCode keyToPress;

    // Private Variables

    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
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
            // Add scoring value here
        }
    }

    // Collision detection when note collides with button
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NoteButton")
        {
            canBePressed = true;
            Debug.Log("Button can be pressed!");
        }
    }

    // Collision detection when note exits button's collider
    // canBePressed is false and deletes/deactivates the note
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NoteButton")
        {
            canBePressed = false;
            Debug.Log("Button cannot be pressed! Note missed!");
            StartCoroutine(DestroyNote());
        } 
    }

    // Coroutine that deletes/deactivates the note
    IEnumerator DestroyNote()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
