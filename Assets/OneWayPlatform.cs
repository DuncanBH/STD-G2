using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private BoxCollider2D _trigger;
    private EdgeCollider2D _edge;

    // Start is called before the first frame update
    void Start()
    {
        _edge = GetComponent<EdgeCollider2D>();
        _trigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Player>().Velocity.y >= 0)
            _edge.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _edge.enabled = true;
    }
}