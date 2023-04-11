using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player : MonoBehaviour
{
    //Parameters
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float airSpeedModif = 2;
    [SerializeField] private float slowDownModif = 0.5f;
    [SerializeField] private float jumpStrength = 1;
    [SerializeField] private float jumpModif = 1;
    [SerializeField] private float jumpMinDuration = 0.2f;
    [SerializeField] private float jumpDuration = 1.0f;
    [SerializeField] private float gravity = 1;
    [SerializeField] private float rightLength = 2;
    [SerializeField] private float downwardLength = 2;

    //Exposed
    public bool FacingRight { get; private set; } = true;
    public bool IsWalking { get; private set; } = false;
    public bool IsJumping { get; private set; } = false;
    public bool IsAttacking { get; private set; } = false;
    public bool IsGrounded => _isGrounded;
    public Vector3 RealtimeVelocity { get; private set; }

    //Set up
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;

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
    private bool _isGrounded = false;
    private float _jumpTime = 0.0f;
    private float _activeGravity;
    private float _inputFire;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        var size = _collider.size;
        _width = size.x / 2;
        _height = size.y / 2;

        _layermask = ~(1 << LayerMask.NameToLayer("Player"));

        _activeGravity = gravity;
    }

    void Update()
    {
        //Input handling
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");
        _inputJump = Input.GetAxisRaw("Jump");
        _inputFire = Input.GetAxisRaw("Fire1");
        
        RealtimeVelocity = _rigidbody.velocity;
        
        if (_inputJump == 0 && _isGrounded)
        {
            _canJump = true;
        }

        IsWalking = (_inputX != 0);

        IsAttacking = _inputFire > 0;
        
        //Left/Right rotation
        if (_isGrounded)
        {
            if (FacingRight && _inputX > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                FacingRight = false;
            }
            else if (!FacingRight && _inputX < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180,0);
                FacingRight = true;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 position = _transform.position;

        Vector2 displacement = Vector3.zero;

        _isGrounded = DoGroundCheck();

        if (_isGrounded)
        {
            _jumpTime = 0.0f;
        }

        //Jumping 
        if (_isJumping && _inputJump == 0)
        {
            _isJumping = false;
        }

        if ((_inputJump > 0 && _jumpTime < jumpDuration) ||
            (!_isGrounded && _jumpTime < jumpMinDuration) && _isJumping)
        {
            //Initial propulsion
            if (_isGrounded && _canJump)
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
        if (_inputX == 0 && _isGrounded) // no direction on ground
        {
            Vector2 velocity = _rigidbody.velocity;
            _rigidbody.velocity = new Vector2(velocity.x * slowDownModif, velocity.y);
        }
        else if (_isGrounded && (_rigidbody.velocity.x > 0 && _inputX < 0) ||
                 (_rigidbody.velocity.x < 0 && _inputX > 0)) // opposite direction on ground
        {
            Vector2 velocity = _rigidbody.velocity;
            _rigidbody.velocity = new Vector2(velocity.x * slowDownModif, velocity.y);
        }

        //Collision checks

        if (_isGrounded)
        {
            displacement.y = Mathf.Clamp(displacement.y, 0, Single.MaxValue);
        }

        if (DoForwardCheck())
        {
            displacement.x = Mathf.Clamp(displacement.x, Single.MinValue, 0);
        }

        if (DoBackwardCheck())
        {
            displacement.x = Mathf.Clamp(displacement.x, 0, Single.MaxValue);
        }


        RaycastHit2D uHit = Physics2D.Raycast(position, Vector3.down, downwardLength, _layermask);
        if (uHit.collider != null)
        {
            displacement.y = Mathf.Clamp(displacement.y, Single.MinValue, 0);
        }

        //apply movement
        var rb2dvelocity = _rigidbody.velocity += displacement;

        var realMax = _isGrounded ? maxSpeed : maxSpeed * airSpeedModif;
        _rigidbody.velocity = Vector2.ClampMagnitude(rb2dvelocity, realMax);
    }

    private bool DoForwardCheck()
    {
        var position = transform.position;

        RaycastHit2D midHit = Physics2D.Raycast(position, Vector3.right, rightLength, _layermask);
        Debug.DrawLine(position, new Vector2(position.x + rightLength, position.y), Color.red);
        if (midHit.collider != null)
            return true;

        var gap = _height * 0.80f;

        RaycastHit2D topHit = Physics2D.Raycast(new Vector2(position.x, position.y + gap), Vector3.right,
            rightLength, _layermask);
        Debug.DrawLine(new Vector2(position.x, position.y + gap),
            new Vector2(position.x + rightLength, position.y + gap), Color.blue);

        RaycastHit2D bottomHit = Physics2D.Raycast(new Vector2(position.x, position.y - gap), Vector3.right,
            rightLength, _layermask);
        Debug.DrawLine(new Vector2(position.x, position.y - gap),
            new Vector2(position.x + rightLength, position.y - gap), Color.blue);

        return topHit.collider != null || bottomHit.collider != null;
    }

    private bool DoBackwardCheck()
    {
        var position = transform.position;

        RaycastHit2D midHit = Physics2D.Raycast(position, -Vector3.right, rightLength, _layermask);
        Debug.DrawLine(position, new Vector2(position.x - rightLength, position.y), Color.red);
        if (midHit.collider != null)
            return true;

        var gap = _height * 0.80f;

        RaycastHit2D topHit = Physics2D.Raycast(new Vector2(position.x, position.y + gap), -Vector3.right,
            rightLength, _layermask);
        Debug.DrawLine(new Vector2(position.x, position.y + gap),
            new Vector2(position.x - rightLength, position.y + gap), Color.blue);

        RaycastHit2D bottomHit = Physics2D.Raycast(new Vector2(position.x, position.y - gap), -Vector3.right,
            rightLength, _layermask);
        Debug.DrawLine(new Vector2(position.x, position.y - gap),
            new Vector2(position.x - rightLength, position.y - gap), Color.blue);

        return topHit.collider != null || bottomHit.collider != null;
    }

    private bool DoGroundCheck()
    {
        var position = transform.position;
        RaycastHit2D midHit = Physics2D.Raycast(position, Vector3.down, downwardLength, _layermask);
        Debug.DrawLine(position, position + (Vector3.down * downwardLength), Color.red);
        if (midHit.collider != null)
            return true;

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