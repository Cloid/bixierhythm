using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTerrain : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            Debug.Log("MOVING");
            this.gameObject.transform.position += Vector3.forward * 62.0f;
        }
    }
}
