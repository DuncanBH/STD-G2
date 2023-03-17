using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = 1;
    [SerializeField] private float rightLength = 2;
    [SerializeField] private float downwardLength = 2;
    
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;

    private float width;
    private float height;
    
    private float _inputX;
    private float _inputY;
    private int _layermask;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();

        width = _collider.size.x / 2;
        height = _collider.size.y / 2;

        _layermask = ~(1 << LayerMask.NameToLayer("Player"));
    }

    void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 position = _transform.position;
        
        Vector3 displacement = Vector3.zero;

        //Movement application
        displacement += new Vector3(_inputX * moveSpeed, _inputY * moveSpeed, 0);
        
        //Gravity
        displacement.y -= gravity / 10;

        //Collision checks
        RaycastHit2D fHit= Physics2D.Raycast(position, Vector3.right, rightLength, _layermask);
        Debug.DrawRay(position, position + (Vector3.right * rightLength), Color.red);
        if (fHit.collider != null)
        {
            displacement.x = Mathf.Clamp(displacement.x, Single.MinValue, 0);
        }
            
        RaycastHit2D dHit= Physics2D.Raycast(position, Vector3.down, downwardLength, _layermask);
        Debug.DrawRay(position, position + (Vector3.down * downwardLength), Color.red);
        
        if (dHit.collider != null)
        {
            displacement.y = Mathf.Clamp(displacement.y, 0, Single.MaxValue);
        }

        //apply movement
        _transform.position += displacement;
    }
}
