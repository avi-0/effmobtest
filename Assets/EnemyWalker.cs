using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : MonoBehaviour
{
    [SerializeField] private Character _character;
    
    [SerializeField] private Transform _groundCheckLeftPosition;
    
    [SerializeField] private Transform _groundCheckRightPosition;
    
    [SerializeField] private Transform _wallCheckLeftPosition;
    
    [SerializeField] private Transform _wallCheckRightPosition;

    [SerializeField] private LayerMask _surroundingsCheckLayerMask;
    
    [SerializeField] private float _surroundingsCheckRadius;

    private bool _canWalkLeft;
    private bool _canWalkRight;
    private bool _isWalkingLeft = false;
    private bool _isWalkingRight = false;

    private void Start()
    {
        _isWalkingRight = true;
    }

    void FixedUpdate()
    {
        CheckSurroundings();
        
        if (_isWalkingRight && !_canWalkRight)
        {
            _isWalkingRight = false;
            _isWalkingLeft = true;
        }

        if (_isWalkingLeft && !_canWalkLeft)
        {
            _isWalkingLeft = false;
            _isWalkingRight = true;
        }
        
        _character.MoveInput = Vector2.zero;
        if (_isWalkingRight)
            _character.MoveInput = Vector2.right;
        if (_isWalkingLeft)
            _character.MoveInput = Vector2.left;
    }

    private void CheckSurroundings()
    {
        var groundLeft = null != Physics2D.OverlapCircle(_groundCheckLeftPosition.position, _surroundingsCheckRadius, _surroundingsCheckLayerMask);
        var groundRight = null != Physics2D.OverlapCircle(_groundCheckRightPosition.position, _surroundingsCheckRadius, _surroundingsCheckLayerMask);
        var wallLeft = null != Physics2D.OverlapCircle(_wallCheckLeftPosition.position, _surroundingsCheckRadius, _surroundingsCheckLayerMask);
        var wallRight = null != Physics2D.OverlapCircle(_wallCheckRightPosition.position, _surroundingsCheckRadius, _surroundingsCheckLayerMask);

        _canWalkLeft = groundLeft && !wallLeft;
        _canWalkRight = groundRight && !wallRight;
    }
}
