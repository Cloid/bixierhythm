using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    // Author: Herald Hamor
    // Public Variables
    public bool canBePressed;
    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If pressed, then note is deactivated
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);

                print(Mathf.Abs(transform.position.y));

                if (Mathf.Abs(transform.position.y) > 1f)
                {
                    Debug.Log("Normal Hit!");
                    GameManager.instance.GHNormalHit();
                } else if (Mathf.Abs(transform.position.y) > 0.75f)
                {
                    Debug.Log("Good Hit!");
                    GameManager.instance.GHGoodHit();
                } else if (Mathf.Abs(transform.position.y) > 0.5f) {
                    Debug.Log("Perfect Hit!");
                    GameManager.instance.GHPerfectHit();
                }
            }
        }
    }

    // Collision detection when a note enters a button's collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "NoteButton")
        {
            canBePressed = true;
            print("Can be pressed!");
        }
    }

    // Collision detection when a note exits a button's collider
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NoteButton")
        {
            if (gameObject.activeSelf)
            {
                canBePressed = false;
                print("Cannot be pressed!");
                GameManager.instance.NoteMissed(other.gameObject);
            }
        }
    }
}
