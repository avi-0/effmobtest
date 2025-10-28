using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float Gravity;

    [SerializeField] private Rigidbody2D _rigidbody;
    
    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        //_rigidbody.velocity += Gravity * Time.deltaTime * Vector2.down;
    }
}
