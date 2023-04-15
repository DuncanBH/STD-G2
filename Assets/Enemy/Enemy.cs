using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int healthPoints = 1;

    private Animator _animator;
    private Transform _transform;

    private Vector3 _initScale; 
    public void Start()
    {
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();

        _initScale = _transform.localScale;
    }

    private IEnumerator DmgEffect()
    {
        
        _animator.Play("enemy_hit");

        yield return new WaitForSeconds(.5f);

        _animator.Play("crab_idle");
    }
    
    
    public void Attack(int damage)
    {
        // Debug.Log("HIT");
        healthPoints -= damage;

        StartCoroutine(DmgEffect());
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