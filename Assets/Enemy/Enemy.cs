using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int healthPoints = 1;

    protected Animator _animator;

    private Vector3 _initScale;

    private Transform _transform;

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();

        _initScale = _transform.localScale;
    }

    public void Attack(int damage)
    {
        // Debug.Log("HIT");
        healthPoints -= damage;

        _animator.Play("enemy_hit");

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