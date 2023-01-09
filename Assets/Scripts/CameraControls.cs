using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody cameraobject;
    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;
    public Transform orientation;
    private Vector2 turn;
    //private float field_of_view = 60f;//normal
    private float mouseScrollWheel;
    //private Vector2 move;
    private bool click, clickflag = false;
    void Start()
    {
        //MyInput();
        //CameraMovement();
        cameraobject = GetComponent<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked;//hide cursor
        
    }
    private void FixedUpdate()
    {
        CameraMovement();
    }
    // Update is called once per frame
    void Update()
    {
        MyInput();
        if (clickflag)
        {
            turn.x += Input.GetAxis("Mouse X");
            turn.y += Input.GetAxis("Mouse Y");
            transform.localRotation = Quaternion.Euler((-turn.y * 3), (turn.x * 3), 0);
        }
        //myCamera.velocity = new Vector3((40 * move.x), 0, (40 * move.y));
       
        //if (Input.GetKey(KeyCode.Z))
        //{ field_of_view -= 20f; }
        //if (Input.GetKey(KeyCode.X))
        //{ field_of_view += 20f; }
        //GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, field_of_view, Time.deltaTime * 5);

        if(mouseScrollWheel > 0 && transform.position.y >= 16)
        {
            //GetComponent<Camera>().fieldOfView--;
            GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y - 8.0f, transform.position.z);
            //transform.Rotate(-1,0,0);        
        }

        if (mouseScrollWheel < 0 )
        {
            //GetComponent<Camera>().fieldOfView++;
            GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 8.0f, transform.position.z);
            //transform.Rotate(1, 0, 0);
        }

    }

    void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        click = Input.GetMouseButtonDown(0);
        if(click)
        { 
            if (clickflag)
            { clickflag = false; }
            else clickflag = true;
        }
    }
    void CameraMovement()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //cameraobject.AddForce(moveDirection.normalized * 40f, ForceMode.Force);
        cameraobject.velocity = moveDirection * 50;
    }

    
}
