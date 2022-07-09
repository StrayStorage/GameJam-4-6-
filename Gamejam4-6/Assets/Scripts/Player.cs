using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;

    private float playerSpeed = 2.0f;
    private float gravityValue = -9.81f;
    public Animator anima;
    private Vector3 mousePos;
    private Vector3 mouseWorldPos;

    private float distFromScreen;
    public LineRenderer lr;
    public float rotationSpeed = 20.0f;

    private AttachToPlayer cameraFollowRef;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraFollowRef = Camera.main.transform.parent.GetComponent<AttachToPlayer>();
    }

    void Update()
    {
        if (cameraFollowRef.enableFollow)
        {
            distFromScreen = 2f;
        }
        else
        {
            distFromScreen = 10f;
        }
        
        mousePos = Input.mousePosition;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos + new Vector3(0,0, distFromScreen));

        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, mouseWorldPos);


        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            //gameObject.transform.forward = move;

            anima.SetBool("isMoving", true);

        }
        else
        {
            anima.SetBool("isMoving", false);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        Vector3 targetDirection = mouseWorldPos - transform.position;

        float singleStep = rotationSpeed;// * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z));
    }

    /*
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 20, 250, 120));
        GUILayout.Label("Mouse position: " + mousePos.ToString("F3"));
        GUILayout.Label("Mouse World position: " + mouseWorldPos.ToString("F3"));
        GUILayout.EndArea();
    }
    */
}
