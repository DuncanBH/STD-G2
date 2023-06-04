using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrabEnemy : Enemy
{
    private static Dictionary<string, EnemyState> _states;

    private EnemyState _activeState;

    private float _timer;

    private bool _walking;

    public void Awake()
    {
        _states = new Dictionary<string, EnemyState>
        {
            {
                "idle",
                new EnemyState
                {
                    Name = "idle", Animation = "crab_idle",
                    Tick = () =>
                    {
                        if (_timer > 1f)
                            _walking = true;
                        else
                            _timer += Time.deltaTime;
                    },
                    Init = () => { _timer = 0.0f; },
                    Transitions = new[]
                    {
                        new EnemyState.Transition {Condition = () => _walking, Result = () => _states["walk"]}
                    }
                }
            },
            {
                "walk",
                new EnemyState
                {
                    Name = "walk", Animation = "crab_walk",
                    Tick = () =>
                    {
                        if (_timer > 2f)
                        {
                            _walking = false;
                        }
                        else
                        {
                            _timer += Time.deltaTime;
                            _rigidbody.velocity = new Vector2(Random.Range(-20f, 20f), 0);
                        }
                    },
                    Init = () => { _timer = 0.0f; },
                    Transitions = new[]
                    {
                        new EnemyState.Transition {Condition = () => !_walking, Result = () => _states["idle"]}
                    }
                }
            }
        };
    }

    protected override void Start()
    {
        _activeState = _states["idle"];

        base.Start();
    }

    public void Update()
    {
        _activeState.Tick();
        CheckTransitions();
    }

    private void CheckTransitions()
    {
        foreach (var transition in _activeState.Transitions)
            if (transition.Condition())
            {
                _activeState.Exit?.Invoke();
                _activeState = transition.Result();
                _activeState.Init?.Invoke();
            }
    }

    private class EnemyState
    {
        [CanBeNull] public Action Exit;
        [CanBeNull] public Action Init;
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