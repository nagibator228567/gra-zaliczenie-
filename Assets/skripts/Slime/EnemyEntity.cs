using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyEntity : MonoBehaviour
{
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    [SerializeField] private int _maxhealth;
    private int _currentHealth;
    private PolygonCollider2D _polygonCollider2d;

    private void Awake()
    {
        _polygonCollider2d = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        _currentHealth = _maxhealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attack");
    }
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this,EventArgs.Empty);
        DetectDeath();
    }
    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _polygonCollider2d.enabled = false;
            OnDeath?.Invoke(this,EventArgs.Empty);

        }
    }
    public void PolygonColliderTurnOff()
    {
        _polygonCollider2d.enabled = false;
    }
    public void PolygonColliderTurnOn()
    {
        _polygonCollider2d.enabled = true;
    }
}

