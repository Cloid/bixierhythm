using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllerKeys : MonoBehaviour
{
    // Public Variables
    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress;

    // Private Variables
    private SpriteRenderer sRend;

    // Start is called before the first frame update
    void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Pressed button interface
        if (Input.GetKeyDown(keyToPress))
        {
            sRend.sprite = pressedImage;
        } else if (Input.GetKeyUp(keyToPress)) 
        {
            sRend.sprite = defaultImage;
        }
    }
}