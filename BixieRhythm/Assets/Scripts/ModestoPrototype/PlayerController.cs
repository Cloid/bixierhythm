using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Author: Herald Hamor
public class PlayerController : MonoBehaviour
{
    // Public Variables

    // Private Variables
    //      Button gameObjects
    private GameObject greenButton;
    private GameObject redButton;
    private GameObject yellowButton;
    private GameObject blueButton;
    private GameObject orangeButton;

    //      Player Model
    private GameObject playerModel;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the gameobjects of each button using the index
        // If the order of the buttons gets changed in the Player gameobject, then the indices must also be changed accordingly
        greenButton = transform.GetChild(2).gameObject;
        redButton = transform.GetChild(3).gameObject;
        yellowButton = transform.GetChild(4).gameObject;
        blueButton = transform.GetChild(5).gameObject;
        orangeButton = transform.GetChild(6).gameObject;

        // Gets the gameobject of the player model
        // The player model is separate from the player object as the parent player object moves the children, including the camera and buttons.
        playerModel = transform.GetChild(7).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // If input keys of buttons is pressed, then the player model will move towards the position of the respective button
        if (Input.inputString != "")
        {
            string keyInput = Input.inputString;
            switch (keyInput)
            {
                case "q":
                    playerModel.transform.position = greenButton.transform.position;
                    break;
                case "w":
                    playerModel.transform.position = redButton.transform.position;
                    break;
                case "e":
                    playerModel.transform.position = yellowButton.transform.position;
                    break;
                case "r":
                    playerModel.transform.position = blueButton.transform.position;
                    break;
                case "t":
                    playerModel.transform.position = orangeButton.transform.position;
                    break;
            }
        }
    }
}
