using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    
    [Header("基本参数")]
    public float speed;
    public float JumpForce;

    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;

    private void Awake()
    {
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();

        inputControl.GamePlay.Jump.started += jump;
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        Move();    
    }
    public void Move()
    {
        rb.velocity = new Vector2(speed * Time.deltaTime * inputDirection.x , rb.velocity.y);
        // realize player flip
        transform.localScale = new Vector3(inputDirection.x < 0 ? -1 : 1, 1, 1);
    }

    private void jump(InputAction.CallbackContext context)
    {
        // Debug.Log("Jump");
        if(physicsCheck.isGround)
            rb.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
    }
}
