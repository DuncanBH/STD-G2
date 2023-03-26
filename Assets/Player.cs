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
    [SerializeField] private float jumpStrength = 1;
    [SerializeField] private float jumpModif = 1;
    [SerializeField] private float jumpMinDuration = 0.2f;
    [SerializeField] private float jumpDuration = 1.0f;
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
    private bool _canJump;
    private bool _isJumping;

    //Logic
    private bool _isAirborne = false;
    private float _jumpTime = 0.0f;
    private float _activeGravity;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();

        _width = _collider.size.x / 2;
        _height = _collider.size.y / 2;

        _layermask = ~(1 << LayerMask.NameToLayer("Player"));

        _activeGravity = gravity;
    }

    void Update()
    {
        //Input handling
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");
        _inputJump = Input.GetAxisRaw("Jump");

        if (_inputJump == 0 && !_isAirborne)
        {
            _canJump = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 position = _transform.position;

        Vector2 displacement = Vector3.zero;

        _isAirborne = !DoGroundCheck();

        if (!_isAirborne)
        {
            displacement.y = Mathf.Clamp(displacement.y, 0, Single.MaxValue);
            _jumpTime = 0.0f;
        }

        //Jumping 
        if (_isJumping && _inputJump == 0)
        {
            _isJumping = false;
        }

        if ((_inputJump > 0 && _jumpTime < jumpDuration) || (_isAirborne && _jumpTime < jumpMinDuration) && _isJumping)
        {
            //Initial hit
            if (!_isAirborne && _canJump)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
                _rigidbody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
                _canJump = false;
                _isJumping = true;
            }

            _activeGravity = gravity;

            _jumpTime += Time.fixedDeltaTime;
        }
        else
        {
            _activeGravity = gravity * jumpModif;
            _isJumping = false;
        }

        //Movement application + gravity
        displacement += new Vector2(_inputX * moveSpeed, -_activeGravity);

        //apply constraints
        if (_inputX == 0 && !_isAirborne)
        {
            Vector2 velocity = _rigidbody.velocity;
            _rigidbody.velocity = new Vector2(velocity.x * slowDownModif, velocity.y);
        }
        else if (!_isAirborne && (_rigidbody.velocity.x > 0 && _inputX < 0) ||
                 (_rigidbody.velocity.x < 0 && _inputX > 0))
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


        RaycastHit2D uHit = Physics2D.Raycast(position, Vector3.down, downwardLength, _layermask);
        if (uHit.collider != null)
        {
            displacement.y = Mathf.Clamp(displacement.y, Single.MinValue, 0);
        }

        //apply movement
        _rigidbody.velocity += displacement;
    }

    private bool DoGroundCheck()
    {
        RaycastHit2D midHit = Physics2D.Raycast(transform.position, Vector3.down, downwardLength, _layermask);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * downwardLength), Color.red);
        if (midHit.collider != null)
            return true;

        var position = transform.position;

        RaycastHit2D backHit = Physics2D.Raycast(new Vector2(position.x + _width, position.y), Vector3.down,
            downwardLength, _layermask);
        Debug.DrawLine(new Vector3(position.x + _width, position.y),
            new Vector3(position.x + _width, position.y) + (Vector3.down * downwardLength), Color.blue);

        RaycastHit2D frontHit = Physics2D.Raycast(new Vector2(position.x - _width, position.y), Vector3.down,
            downwardLength, _layermask);
        Debug.DrawLine(new Vector3(position.x - _width, position.y),
            new Vector3(position.x - _width, position.y) + (Vector3.down * downwardLength), Color.green);

        return backHit.collider != null || frontHit.collider != null;
    }
}