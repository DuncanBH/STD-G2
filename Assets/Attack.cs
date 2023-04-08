using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    private Collider2D _collider2D;

    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (enemy)
        {
            enemy.Attack(damage);
        }
    }
}