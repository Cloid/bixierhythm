using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeiPlayerController : MonoBehaviour
{

    public float X_OffsetFromMouse = 0f;
    public float Y_OffsetFromMouse = 0f;
    public float MoveSpeed = 0.1f;
    public float RotationSpeed = 0.1f;
    
    private Camera MainCamera;
    private Vector3 MousePosition;
    private Rigidbody rb;
    private Vector3 position = new Vector3(0f, 0f, 0f);

    public GameObject MeiPlayerModel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        MousePosition += Vector3.up * X_OffsetFromMouse;
        MousePosition += Vector3.left * Y_OffsetFromMouse;
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce((MousePosition-transform.position).normalized * MoveSpeed * Time.deltaTime);
        Quaternion xRot_target = Quaternion.Euler(0, 0, -rb.velocity.x);
        Quaternion yRot_target = Quaternion.Euler(-rb.velocity.y, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, xRot_target * yRot_target, Time.deltaTime * RotationSpeed);
    }
}
