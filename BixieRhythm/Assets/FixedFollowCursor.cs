using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollowCursor : MonoBehaviour
{

    public float offset;
    public float moveSpeed = 0.1f;
    
    private Camera MainCamera;
    private Vector3 MousePosition;
    private Rigidbody rb;
    private Vector3 position = new Vector3(0f, 0f, 0f);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        //this.transform.position = new Vector3(MousePosition.x,offset, -9);
        position = Vector3.Lerp(transform.position, MousePosition, moveSpeed);
    }

    void FixedUpdate()
    {
        rb.MovePosition(position);
    }
}
