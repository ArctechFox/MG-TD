using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Tower
{
    public Transform ArrowPrefab;
    public float ProjectileSpeed = 500f;

    private void Start()
    {
        AttackType = TowerAttackType.RangePhysical;
        TargetingStyle = TowerTargetingStyle.SingleTarget;

        // DetermineObjective every 1/30th of a second, 
        // instead of once per frame like in normal Update method (which realistically could've been up to 200 times a second depending on framerate, so this way is more efficient).
        //InvokeRepeating("DetermineObjective", 0f, .030f);
    }

    public void Update()
    {
        DetermineObjective();
    }

    public override void InitializeComponents(Transform AnchorPoint)
    {
        base.InitializeComponents(AnchorPoint);

        HitboxReference = null;

        var radar = Instantiate(RadarPrefab, AnchorPoint.position, AnchorPoint.rotation);
        radar.transform.parent = AnchorPoint;

        RadarReference = radar.GetComponent<CircleCollider2D>();
        RadarReference.radius = Range;
        RadarReference.GetComponent<RadarScript>().tower = this;

        transform.parent = radar.transform;
        this.AnchorPoint = AnchorPoint;        
    }

    protected override void DetermineObjective()
    {
        // If we have enemies in our "radar", then Attack() the first one.
        if (EnemiesTargeted.Count > 0)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        AttackTimer.Start();
        if (AttackTimer.ElapsedMilliseconds >= Agility) // If we've waited as long or longer than it takes for the attack to cooldown
        {
            AttackTimer.Reset();

            FireProjectile();
        }
    }

    protected virtual void FireProjectile()
    {
        var enemyTarget = EnemiesTargeted[0];

        Vector3 directionToFire = (transform.position - enemyTarget.transform.position).normalized *-1;

        var arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.LookRotation(directionToFire));
        arrow.rotation = Quaternion.AngleAxis(90f, directionToFire);
        arrow.parent = transform;

        var arrowRigidbody = arrow.GetComponent<Rigidbody2D>();

        arrowRigidbody.AddForce(directionToFire * ProjectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            EnemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            EnemiesInRange.Remove(enemy);
        }
    }
}
