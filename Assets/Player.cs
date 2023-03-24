using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player : MonoBehaviour
{
    //Parameters
    [SerializeField] private float moveSpeed;
    [SerializeField] private float slowDownModif = 0.5f;
    [SerializeField] private float jumpModif = 1;
    [SerializeField] private float gravity = 1;
    [SerializeField] private float rightLength = 2;
    [SerializeField] private float downwardLength = 2;

    //Set up
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    
    private int _layermask;
    private float _width;
    private float _height;

    //Input 
    private float _inputX;
    private float _inputY;
    private float _inputJump;
    
    //Logic
    private bool _isAirborne = false;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();

        _width = _collider.size.x / 2;
        _height = _collider.size.y / 2;

        _layermask = ~(1 << LayerMask.NameToLayer("Player"));
    }

    void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");
        _inputJump = Input.GetAxisRaw("Jump");
    }

    private void FixedUpdate()
    {
        Vector3 position = _transform.position;

        Vector2 displacement = Vector3.zero;

        //Movement application + gravity
        displacement += new Vector2(_inputX * moveSpeed, -( gravity / 10));

        //Jumping 
        if (_inputJump > 0 && !_isAirborne)
        {
            _rigidbody.AddForce(Vector2.up * jumpModif, ForceMode2D.Impulse);
            _isAirborne = true;
        }
        
        //apply constraints
        if (_inputX == 0 && !_isAirborne)
        {
            Vector2 velocity = _rigidbody.velocity;
            _rigidbody.velocity = new Vector2(velocity.x * slowDownModif, velocity.y);
        } else if (!_isAirborne && (_rigidbody.velocity.x > 0 && _inputX < 0) || (_rigidbody.velocity.x < 0 && _inputX > 0))
        {
            Vector2 velocity = _rigidbody.velocity;
            _rigidbody.velocity = new Vector2(velocity.x * slowDownModif, velocity.y);
        }
        
        //Collision checks
        RaycastHit2D fHit = Physics2D.Raycast(position, Vector3.right, rightLength, _layermask);
        if (fHit.collider != null)
        {
            displacement.x = Mathf.Clamp(displacement.x, Single.MinValue, 0);
        }

        RaycastHit2D bHit = Physics2D.Raycast(position, -Vector3.right, rightLength, _layermask);
        if (bHit.collider != null)
        {
            displacement.x = Mathf.Clamp(displacement.x, 0, Single.MaxValue);
        }

        RaycastHit2D dHit = Physics2D.Raycast(position, Vector3.down, downwardLength, _layermask);
        if (dHit.collider != null)
        {
            displacement.y = Mathf.Clamp(displacement.y, 0, Single.MaxValue);
            _isAirborne = false;
        }

        RaycastHit2D uHit = Physics2D.Raycast(position, Vector3.down, downwardLength, _layermask);
        if (uHit.collider != null)
        {
            displacement.y = Mathf.Clamp(displacement.y, Single.MinValue, 0);
        }
        
        //apply movement
        _rigidbody.velocity += displacement;
    }
}