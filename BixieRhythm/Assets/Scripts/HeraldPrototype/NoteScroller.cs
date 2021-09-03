using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Author: Herald Hamor
public class NoteScroller : MonoBehaviour
{
    // Public Variables
    public float beatTempo;
    public bool hasStarted;

    // Private Variables


    // Start is called before the first frame update
    void Start()
    {
        // BeatTempo can be modified based on desired speed
        beatTempo = beatTempo / 60f;
        hasStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If game has started, notes start to come down
        if (hasStarted)
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }
}
