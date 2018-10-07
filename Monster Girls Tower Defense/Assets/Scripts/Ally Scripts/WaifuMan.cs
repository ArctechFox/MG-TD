using UnityEngine;

public class WaifuMan : Tower
{
    private void Start()
    {
        AttackType = TowerAttackType.Melee;
        TargetingStyle = TowerTargetingStyle.SingleTarget;

        AttackTimer.Start();

        // DetermineObjective every 1/30th of a second, 
        // instead of once per frame like in normal Update method (which realistically could've been up to 200 times a second depending on framerate, so this way is more efficient).
        //InvokeRepeating("DetermineObjective", 0f, .030f);
    }

    public override void InitializeComponents(Transform AnchorPoint)
    {
        base.InitializeComponents(AnchorPoint);

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
        // If we have enemies in target range, attack.
        if (EnemiesInRange.Count > 0)
        {
            Attack();
        }

        // If we don't have any enemies to attack, and we have enemies in our "radar", then move towards the first one.
        else if (EnemiesTargeted.Count > 0)
        {
            MoveToTarget(EnemiesTargeted[0].transform.position);
        }

        // If there's no enemy to attack or move towards, then return to the AnchorPoint. 
        else
        {
            if (Vector3.Distance(AnchorPoint.position, transform.position) > AnchorPointProximity_ForgivenessMargin)
            {
                MoveToTarget(AnchorPoint.position);
            }
        }
    }

    public void Update()
    {
        DetermineObjective();
    }

    protected override void Attack()
    {
        if (AttackTimer.ElapsedMilliseconds >= Agility) // If we've waited as long or longer than it takes for the attack to cooldown
        {
            AttackTimer.Restart();

            // TODO: Activate attack animation
            InflictDamage(EnemiesInRange[0]);
        }        
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
