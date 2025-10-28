using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private static readonly int OnGround = Animator.StringToHash("on_ground");
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int Jumping = Animator.StringToHash("jumping");
    private static readonly int FaceRight = Animator.StringToHash("face_right");

    [SerializeField] private float RunSpeed;

    [SerializeField] private float RunAccel;

    [SerializeField] private float JumpSpeed;

    [SerializeField] private float FlipSpeed;

    [SerializeField] private float HurtSpeed;

    [SerializeField] private int _startingHealth;

    [SerializeField] private Transform _selfTransform;
    
    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private Animator _animator;

    [SerializeField] private Animator _facingDirectionAnimator;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Transform _groundCheckPosition;

    [SerializeField] private LayerMask _groundCheckLayerMask;

    [SerializeField] private float _groundCheckRadius;
    
    [SerializeField] private TMP_Text _scoreText;
    
    [SerializeField] private TMP_Text _healthText;

    [SerializeField] private LayerMask _spikeLayerMask;
    
    private InputAction moveAction;
    private InputAction jumpAction;

    private bool _isOnGround;
    private bool _isJumping;
    private int _score = 0;
    private int _health;
    
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        _health = _startingHealth;
        UpdateHud();
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
            _facingDirectionAnimator.SetBool(FaceRight, true);
        } else if (moveValue.x < 0)
        {
            _facingDirectionAnimator.SetBool(FaceRight, false);
        }
    }

    private void UpdateHud()
    {
        _scoreText.text = $"score: {_score}";
        _healthText.text = $"health: {_health}";
    }

    private bool IsOnGround()
    {
        var coll = Physics2D.OverlapCircle(_groundCheckPosition.position, _groundCheckRadius, _groundCheckLayerMask);
        return coll is not null;
    }

    public void AddScore(int delta)
    {
        _score += delta;
        UpdateHud();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print("hey");
        if ((_spikeLayerMask & (1 << other.collider.gameObject.layer)) != 0)
        {
            Hurt(other.GetContact(0).point);
        }
    }

    private void Hurt(Vector3 from)
    {
        var dir = (_selfTransform.position - from).normalized;
        _rigidbody.velocity += (Vector2) dir * HurtSpeed;

        _health--;
        UpdateHud();

        if (_health <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
