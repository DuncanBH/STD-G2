using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int healthPoints = 1;

    public void Attack(int damage)
    {
        // Debug.Log("HIT");
        healthPoints -= damage;

        if (healthPoints <= 0)
            DoDie();
    }

    public void DoDie()
    {
        Debug.Log("Destroyed");
        Destroy(gameObject);
    }
}