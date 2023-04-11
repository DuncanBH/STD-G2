using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    private bool _attackLockedOut = false;

    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;
        }

        if (!_player.IsAttacking)
        {
            _attackLockedOut = false;
        }
        if (_player.IsAttacking && !_attackLockedOut)
        {
            _attackLockedOut = true;
            _animator.Play("Attack");
        }
       
        if (_player.IsGrounded)
        {
            _animator.Play(_player.IsWalking ? "Run" : "Idle");
        }
        else if (_player.IsJumping || _player.RealtimeVelocity.y > 0)
        {
            _animator.Play("Jump");
        }
        else
        {
            _animator.Play("Fall");
        }
    }
}