using UnityEngine;
using UnityEngine.UI;
using System;

public class Zombie : Enemy
{
    public Zombie()
    {
        Target = Waypoints.points[0];
    }

    private void Start()
    {
        SetHealthBar();
    }

    private void Update()
    {
        if (HealthRemaining <= 0)
        {
            Die();
            return;
        }

        base.Move();
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            var tower = collision.gameObject.transform.parent.GetComponent<Tower>();
            TakeDamage(tower.Damage);
            Destroy(collision.gameObject);
        }
    }
}