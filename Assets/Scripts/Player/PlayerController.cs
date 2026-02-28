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
    private PlayerAnimation playerAnimation;
    public CapsuleCollider2D coll;
    public float hurtForce;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("状态")]
    public bool isHurt; 
    public bool isDead;
    public bool isAttack;
   
    private void Awake()
    {
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();

        //jump
        inputControl.GamePlay.Jump.started += jump;
        //attack
        inputControl.GamePlay.Attack.started += PlayerAttack;
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
        CheckState();
    }
    private void FixedUpdate()
    {
        if(!isHurt && !isAttack) {
            Move();
        }
    }
    public void Move()
    {
        rb.velocity = new Vector2(speed * Time.deltaTime * inputDirection.x , rb.velocity.y);
        // realize player flip
        if (inputDirection.x != 0)
        {
            transform.localScale = new Vector3(inputDirection.x < 0 ? -1 : 1, 1, 1);
        }
    }

    private void jump(InputAction.CallbackContext context)
    {
        // Debug.Log("Jump");
        if(physicsCheck.isGround)
            rb.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;  
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);

    }
    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();
    }
    #endregion 
    public void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }
}
