using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxerUnit : MonoBehaviour
{
    public float speed;

    private Vector3 _previousCameraPosition;
    private Transform _camera;
    private Vector3 _targetPosition;

    private float ParallaxAmount => 1f - speed;

    Vector3 CameraMovement
    {
        get
        {
            Vector3 movement = _camera.position - _previousCameraPosition;
            _previousCameraPosition = _camera.position;
            return movement;
        }
    }

    private void Awake()
    {
        _camera = Camera.main.transform;
        _previousCameraPosition = _camera.position;
    }

    private void LateUpdate()
    {
        Vector3 movement = CameraMovement;
        if (movement == Vector3.zero) return;

        _targetPosition = new Vector3(transform.position.x + movement.x * ParallaxAmount, transform.position.y,
            transform.position.z);

        // transform.position = _targetPosition;
        transform.SetPositionAndRotation(_targetPosition, Quaternion.identity);
    }
}