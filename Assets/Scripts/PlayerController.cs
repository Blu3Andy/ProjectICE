using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine. InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputMaster playerInput;
    private Animator anim;
    private Rigidbody rigidbody;
    private Transform mainCamera;

    [SerializeField] GameObject gadget;

    [SerializeField] private Transform stalker;

    [SerializeField] private bool lookAtAnker = true;

    private Vector3 movement;

    private Vector2 moveInput;
    private Vector2 rotValue;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotSpeed = 360f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private float dash = 3f;
    [SerializeField] private float dashCooldown = 2f;
     int start = 0;

    private bool isPresingAction = false;

    [SerializeField] private GameObject[] tools;
   
    void Awake()
    {
        playerInput = new InputMaster();
        //anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        mainCamera = Camera.main.transform;

        playerInput.Player.Movement.started += OnMove;
        playerInput.Player.Movement.performed += OnMove;
        playerInput.Player.Movement.canceled += OnMove;

        playerInput.Player.Action.started += i => Action();
        playerInput.Player.Action.canceled +=  i => ActionEnd();

        playerInput.Player.Jump.started += i => Jump();

        playerInput.Player.Scroll.started += Scroll;

        playerInput.Player.CamRotation.started += CamRotation;
        playerInput.Player.CamRotation.performed += CamRotation;
        playerInput.Player.CamRotation.canceled += CamRotation;

        Cursor.visible = false;
    }

    void Update()
    {
        if( rotValue != Vector2.zero) stalker.rotation = Quaternion.Euler(0f, rotValue.x * 5f, 0f);

        if (moveInput != Vector2.zero)//########################
        {
            Movement(speed);
        }

        if (isPresingAction) tools[start].GetComponent<Tool>()?.Execute();

        if (!isPresingAction) tools[start].GetComponent<Tool>()?.Stop();

        

        // Debug.DrawLine(transform.position, transform.position + (transform.forward * movement.z + transform.right * movement.x));
        // Debug.DrawLine(transform.position, transform.position + transform.up * 3);

        //print(Vector3.Angle(transform.position + (transform.forward * movement.z + transform.right * movement.x), transform.up));
    }

    private void Movement(float speed)
    {
        // float targetAngle = Mathf.Atan2((movement.x * transform.forward).magnitude, (movement.z * transform.right).magnitude);              
        // float smoothAngle = Mathf.SmoothDampAngle(transform.up.magnitude, targetAngle, ref rotSpeed, 1 );
        // transform.rotation = Quaternion.AngleAxis(smoothAngle, transform.InverseTransformDirection(transform.up));

        // Vector3 moveDir = Quaternion.AngleAxis(smoothAngle, transform.InverseTransformDirection(transform.up)) * transform.forward;

        //    Quaternion targetRot = Quaternion.LookRotation(transform.position + rigidbody.velocity, transform.up);
        //         // Nur die Y-Achse behalten
        //     targetRot = Quaternion.Euler(0f, targetRot.eulerAngles.y, 0f) * Quaternion.Euler(transform.rotation.eulerAngles.x, targetRot.eulerAngles.y, transform.rotation.eulerAngles.z);
        //     targetRot = Quaternion.Euler(0f, targetRot.eulerAngles.y, 0f) * Quaternion.Euler(transform.rotation.eulerAngles.x, targetRot.eulerAngles.y, transform.rotation.eulerAngles.z);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        // transform.position += speed * Time.deltaTime * (transform.forward * movement.z + transform.right * movement.x);

        print((transform.forward * movement.z + transform.right * movement.x));

        Vector3 actuallForward = Vector3.Cross(mainCamera.right, transform.up);
        Vector3 actualRight = Vector3.Cross(transform.up, actuallForward);

        Vector3 direction = (actuallForward * movement.z + actualRight * movement.x).normalized;

        Debug.DrawLine(transform.position, transform.position + actuallForward * 10, Color.blue);
        Debug.DrawLine(transform.position, transform.position + actualRight * 10, Color.red);

        Debug.DrawLine(transform.position, transform.position + direction * 5, Color.green);
        

        rigidbody.MovePosition(rigidbody.position + direction * (speed * Time.deltaTime));
        
        Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);

        // Sanft rotieren
        Quaternion newRotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, Time.fixedDeltaTime * 3f);
        rigidbody.MoveRotation(newRotation);
    }

    

    private void Jump()
    {
        rigidbody.AddForce(new Vector3(0, jumpForce, 0));
    }

    public void Action()
    {
        isPresingAction = true;
    }

    private void ActionEnd()
    {
        isPresingAction = false;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        movement = new Vector3(moveInput.x, 0, moveInput.y);
        movement = movement.normalized;

    }
    
    private void CamRotation(InputAction.CallbackContext context)
    {
        rotValue = context.ReadValue<Vector2>();
    }

    private void Scroll(InputAction.CallbackContext context)
    {
        float scrollDir = context.ReadValue<Vector2>().y;

        int hotbarSize = tools.Length - 1;

        if (scrollDir < 0 && start == 0)
        {
            start = hotbarSize;
            tools[start].SetActive(true);
            tools[0].SetActive(false);
        }
        else if (scrollDir < 0)
        {
            start--;
            tools[start].SetActive(true);
            tools[start + 1].SetActive(false);
        }

        if (scrollDir > 0 && start == hotbarSize)
        {
            start = 0;

            tools[start].SetActive(true);
            tools[hotbarSize].SetActive(false);
        }
        else if (scrollDir > 0)
        {
            start++;

            tools[start].SetActive(true);
            tools[start - 1].SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision col)
    {
        // if(col.gameObject.CompareTag("ground"))
        // {
        // }
    }

    void OnEnable()
    {
        playerInput.Player.Enable();
    }

    void OnDisable()
    {
        playerInput.Player.Disable();
    }
}