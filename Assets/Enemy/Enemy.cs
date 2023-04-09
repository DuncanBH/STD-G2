using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int healthPoints = 1;

    private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void Attack(int damage)
    {
        // Debug.Log("HIT");
        healthPoints -= damage;

        if (healthPoints <= 0)
            DoDie();
    }

    public void DoDie()
    {
        // Debug.Log("Destroyed");
        _animator.Play("enemy_die");
        Destroy(gameObject, 1.1f);
        
    }
}