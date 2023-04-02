using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int HealthPoints { get; set; } = 1;

    public void Attack(int damage)
    {
        Debug.Log("HIT");
        HealthPoints -= damage;
        
        if (HealthPoints <= 0) 
            DoDie();
    }

    public void DoDie()
    {
        Debug.Log("Destroyed");
        Destroy(gameObject);
    }
}