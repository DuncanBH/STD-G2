using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int HealthPoints { get; set; }

    public void Attack(int damage)
    {
        Debug.Log("HIT");
        HealthPoints -= damage;
    }

    public void OnDestroy()
    {
        Debug.Log("Destroyed");
        Destroy(gameObject);
    }
}