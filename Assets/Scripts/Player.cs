using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private static readonly int RunTrigger = Animator.StringToHash("run_trigger");
    private static readonly int IdleTrigger = Animator.StringToHash("idle_trigger");
    private static readonly int JumpTrigger = Animator.StringToHash("jump_trigger");
    private static readonly int FallTrigger = Animator.StringToHash("fall_trigger");
    private static readonly int OnGround = Animator.StringToHash("on_ground");
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int Jumping = Animator.StringToHash("jumping");

    [SerializeField] private float RunSpeed;

    [SerializeField] private float RunAccel;

    [SerializeField] private float JumpSpeed;

    [SerializeField] private float FlipSpeed;

    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private Animator _animator;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Transform _groundCheckPosition;

    [SerializeField] private LayerMask _groundCheckLayerMask;
    
    [SerializeField] private Transform _selfTransform;

    [SerializeField] private float _groundCheckRadius;
    
    private InputAction moveAction;
    private InputAction jumpAction;

    private bool _isOnGround;
    private bool _isJumping;
    
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Update()
    {
    }

    void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        
        var vel = _rigidbody.velocity.x;
        var targetVel = (moveValue.x != 0f ? Mathf.Sign(moveValue.x) : 0f) * RunSpeed;

        vel = Mathf.MoveTowards(vel, targetVel, RunAccel * Time.deltaTime);
        _rigidbody.velocity = new Vector2(vel, _rigidbody.velocity.y);

        _isOnGround = IsOnGround();
        if (_isOnGround && jumpAction.WasPressedThisFrame())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpSpeed);
            _isJumping = true;
        }

        if (_isJumping && _rigidbody.velocity.y < FlipSpeed)
        {
            _isJumping = false;
        }
        
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        
        _animator.SetBool(OnGround, _isOnGround);
        _animator.SetBool(Walking, moveValue.x != 0);
        _animator.SetBool(Jumping, _isJumping);

        if (moveValue.x > 0)
        {
            _selfTransform.localScale = new Vector3(-1, 1, 1);
        } else if (moveValue.x < 0)
        {
            _selfTransform.localScale = Vector3.one;
        }
    }

    private bool IsOnGround()
    {
        var coll = Physics2D.OverlapCircle(_groundCheckPosition.position, _groundCheckRadius, _groundCheckLayerMask);
        return coll is not null;
    }
}
