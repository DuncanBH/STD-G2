using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrabEnemy : Enemy
{
    private static Dictionary<string, EnemyState> _states;

    private float _timer;

    private EnemyState activeState;

    private bool walking;

    public void Awake()
    {
        _states = new Dictionary<string, EnemyState>
        {
            {
                "idle",
                new EnemyState
                {
                    Name = "idle", Animation = "crab_idle", Tick = () =>
                    {
                        if (_timer > 1f)
                        {
                            walking = true;
                            _timer = 0.0f;
                        }
                        else
                        {
                            _timer += Time.deltaTime;
                        }
                    },
                    Transitions = new[]
                    {
                        new EnemyState.Transition {Condition = () => walking, Result = () => _states["walk"]}
                    }
                }
            },
            {
                "walk",
                new EnemyState
                {
                    Name = "walk", Animation = "crab_walk", Tick = () =>
                    {
                        if (_timer > 2f)
                        {
                            walking = false;
                            _timer = 0.0f;
                        }
                        else
                        {
                            _timer += Time.deltaTime;
                            _rigidbody.velocity = new Vector2(Random.Range(-20f, 20f), 0);
                        }
                    },
                    Transitions = new[]
                    {
                        new EnemyState.Transition {Condition = () => !walking, Result = () => _states["idle"]}
                    }
                }
            }
        };
    }

    protected override void Start()
    {
        activeState = _states["idle"];

        base.Start();
    }

    public void Update()
    {
        activeState.Tick();

        foreach (var transition in activeState.Transitions)
            if (transition.Condition())
                activeState = transition.Result();
    }

    private class EnemyState
    {
        public Action Tick;

        public Transition[] Transitions;

        public string Name { get; set; }
        public string Animation { get; set; }

        public class Transition
        {
            public Func<bool> Condition { get; set; }
            public Func<EnemyState> Result { get; set; }
        }
    }
}