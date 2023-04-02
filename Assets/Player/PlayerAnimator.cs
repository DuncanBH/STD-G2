using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    void Start()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_player.IsGrounded)
        {
            if (_player.IsWalking)
                _animator.Play("Run");
            else
                _animator.Play("Idle");
        }
        else
        {
            //TODO: play fall
        }
    }
}