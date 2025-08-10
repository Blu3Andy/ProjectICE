using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputMaster playerInput;
    private Animator anim;
    private Rigidbody rigidbody;
    private Transform mainCamera;

    [SerializeField] GameObject gadget;

    [SerializeField] private Transform anker;

    [SerializeField] private bool lookAtAnker = true;

    private Vector3 movement;
    private Vector3 camRelMovement;

    private Vector2 moveInput;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotSpeed = 360f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private float dash = 3f;
    [SerializeField] private float dashCooldown = 2f;
     int start = 0;

    private bool isPresingAction = false;
   
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

        Cursor.visible = false;
    }

    void Update()
    {
        if (moveInput != Vector2.zero)//########################
        {
            Movement(speed);
        }
        
        if(isPresingAction) gadget.GetComponent<SphereCollider>().enabled = true;

        if(isPresingAction) gadget.GetComponent<Vacuum>().SuckIn();
    }

    private void Movement(float speed)
    {
        // var relative = (transform.position + camRelMovement) - transform.position;
        // var rot = Quaternion.LookRotation(relative, Vector3.up);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotSpeed * Time.deltaTime);

        float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + mainCamera.eulerAngles.y + movement.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotSpeed, 0.1f);
        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, smoothAngle, 0f) * Vector3.forward;
         
          transform.position += moveDir * speed * Time.deltaTime;
    }

    private void Jump()
    {
        rigidbody.AddForce(new Vector3(0,jumpForce,0));
    }

    public void Action()
    {
        isPresingAction = true;
    }

    private void ActionEnd()
    {
        isPresingAction = false;
        gadget.GetComponent<Vacuum>().stopSuckIn();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        movement = new Vector3(moveInput.x, 0, moveInput.y);
        movement = movement.normalized;

        var matrix = Matrix4x4.Rotate(anker.rotation);

        if (!lookAtAnker) matrix = Matrix4x4.Rotate(transform.rotation);

        camRelMovement = matrix.MultiplyPoint3x4(movement);
    }

    private void Scroll(InputAction.CallbackContext context)
    {
        float scrollDir = context.ReadValue<Vector2>().y;

        int hotbarSize = 2;

        print(start);
        if (scrollDir > 0)
        {
            if (start < hotbarSize)
            {
                start++;
            }
            else
            {
                start = 0;
            }
        }
        if (scrollDir < 0)
        {
            if (start <= 0)
            {
                start = hotbarSize;
            }
            else
            {
                start--;
            }
        }
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "ground")
        {
        }
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