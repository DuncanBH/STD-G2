using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxerUnit : MonoBehaviour
{
    public float speed;

    private Transform _camera;

    private float _startPos;
    private float _length;

    private float ParallaxAmount => 1f - speed;

    void Start()
    {
        _camera = Camera.main.transform;
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        var position = _camera.position;
        float temp = position.x * (1 - speed);
        float dist = (position.x * speed);

        if (temp > _startPos + _length)
            _startPos += _length;
        else if (temp < _startPos - _length)
            _startPos -= _length;

        transform.position = new Vector3(_startPos + dist, transform.position.y, transform.position.z);
    }
}