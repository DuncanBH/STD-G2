using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeAway : MonoBehaviour
{
    private Tilemap _tilemap;

    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _tilemap.color = new Color(255, 255, 255, 0);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        _tilemap.color = new Color(255, 255, 255, 255);
    }
}
