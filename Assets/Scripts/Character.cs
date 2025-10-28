using System;
using UnityEngine;

public class Character : MonoBehaviour
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

    [SerializeField] private LayerMask _spikeLayerMask;

    public int Health => _health;

    private bool _isOnGround;
    private bool _isJumping;
    private int _health;

    [NonSerialized] public Vector2 MoveInput;

    [NonSerialized] public bool JumpInput;
    
    void Start()
    {
        _health = _startingHealth;
    }

    void FixedUpdate()
    {
        var vel = _rigidbody.velocity.x;
        var targetVel = (MoveInput.x != 0f ? Mathf.Sign(MoveInput.x) : 0f) * RunSpeed;

        vel = Mathf.MoveTowards(vel, targetVel, RunAccel * Time.deltaTime);
        _rigidbody.velocity = new Vector2(vel, _rigidbody.velocity.y);

        _isOnGround = IsOnGround();
        if (_isOnGround && JumpInput)
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
        _animator.SetBool(OnGround, _isOnGround);
        _animator.SetBool(Walking, MoveInput.x != 0);
        _animator.SetBool(Jumping, _isJumping);

        if (MoveInput.x > 0)
        {
            _facingDirectionAnimator.SetBool(FaceRight, true);
        } else if (MoveInput.x < 0)
        {
            _facingDirectionAnimator.SetBool(FaceRight, false);
        }
    }
    
    private bool IsOnGround()
    {
        var coll = Physics2D.OverlapCircle(_groundCheckPosition.position, _groundCheckRadius, _groundCheckLayerMask);
        return coll is not null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((_spikeLayerMask & (1 << other.collider.gameObject.layer)) != 0)
        {
            Hurt(other.GetContact(0).point);
        }
    }

    public void Hurt(Vector3 from)
    {
        var dir = (_selfTransform.position - from).normalized;
        _rigidbody.velocity += (Vector2) dir * HurtSpeed;

        _health--;

        if (_health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
