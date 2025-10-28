using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private static readonly int RunTrigger = Animator.StringToHash("run_trigger");
    private static readonly int IdleTrigger = Animator.StringToHash("idle_trigger");

    [SerializeField] public float RunSpeed;

    [SerializeField] public float RunAccel;

    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private Animator _animator;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private InputAction moveAction;
    private InputAction jumpAction;
    
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Update()
    {
        UpdateVisuals();
    }

    void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        
        var vel = _rigidbody.velocity.x;
        var targetVel = (moveValue.x != 0f ? Mathf.Sign(moveValue.x) : 0f) * RunSpeed;

        vel = Mathf.MoveTowards(vel, targetVel, RunAccel * Time.deltaTime);
        _rigidbody.velocity = new Vector2(vel, _rigidbody.velocity.y);
    }

    void UpdateVisuals()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        
        if (moveValue.x != 0)
        {
            _animator.SetTrigger(RunTrigger);
        }
        else
        {
            _animator.SetTrigger(IdleTrigger);
        }

        if (moveValue.x > 0)
        {
            _spriteRenderer.flipX = true;
        } else if (moveValue.x < 0)
        {
            _spriteRenderer.flipX = false;
        }
    }
}
