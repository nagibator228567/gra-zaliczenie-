using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 2;
    public event EventHandler OnSwordSwing;

    private PolygonCollider2D _polygonCollider2d;
    private void Awake()
    {
        _polygonCollider2d = GetComponent<PolygonCollider2D>();
    }
    private void Start()
    {
        AttackColliderTurnOff();

    }
    public void Attack()
    {
        AttackColliderTurnOffOn();
        OnSwordSwing?.Invoke(this,EventArgs.Empty);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(_damageAmount);
        }
    }
    public void AttackColliderTurnOff()
    {
        _polygonCollider2d.enabled = false;
    }
    private void AttackColliderTurnOn()
    {
        _polygonCollider2d.enabled = true;
    }
     
    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }

}
