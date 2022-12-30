using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody camera;
    private Vector2 move;
    private Vector2 turn;
    //private float zoom;
    //private Vector3 deltamove;
    //private float sensetivity;
    

    void Start()
    {
        camera = GetComponent<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked;//hide cursor
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        camera.velocity = new Vector3((40 * move.x), 0, (40 * move.y));
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        transform.localRotation = Quaternion.Euler((-turn.y * 5), ( turn.x * 5), 0);

        
        if(Input.GetKey(KeyCode.Z))
            camera.velocity = new Vector3(camera.velocity.x, 40, camera.velocity.y);

        if (Input.GetKey(KeyCode.X))
            camera.velocity = new Vector3(camera.velocity.x, -40, camera.velocity.y);

    }
}
