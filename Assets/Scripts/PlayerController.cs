using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private Transform anker;

    [SerializeField] private bool lookAtAnker = true;

    private Vector3 movement;

    private Vector2 moveInput;

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

        Cursor.visible = false;
    }

    void Update()
    {
        if (moveInput != Vector2.zero)//########################
        {
            Movement(speed);
        }

        if (isPresingAction) tools[start].GetComponent<Tool>()?.Execute();

        if (!isPresingAction) tools[start].GetComponent<Tool>()?.Stop();

        Debug.DrawLine(transform.position, transform.position + (transform.forward * movement.z + transform.right * movement.x));
        Debug.DrawLine(transform.position, transform.position + transform.up * 3);

        print(Vector3.Angle(transform.position + (transform.forward * movement.z + transform.right * movement.x), transform.up));
    }

    private void Movement(float speed)
    {
       

        transform.position += speed * Time.deltaTime * (transform.forward * movement.z + transform.right * movement.x);

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
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        movement = new Vector3(moveInput.x, 0, moveInput.y);
        movement = movement.normalized;

    }

    private void Scroll(InputAction.CallbackContext context)
    {
        float scrollDir = context.ReadValue<Vector2>().y;

        int hotbarSize = tools.Length-1;

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

        print(start);
        
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