using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    void Start()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_player.IsGrounded)
        {
            _animator.Play(_player.IsWalking ? "Run" : "Idle");
        }
        else if (_player.IsJumping || _player.Velocitiy.y > 0)
        {
            _animator.Play("Jump");
        }
        else
        {
            _animator.Play("Fall");
        }
    }
}