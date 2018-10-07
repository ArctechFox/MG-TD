using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class Tower : Buildable
{
    #region Components

    public GameObject RadarPrefab;

    protected CircleCollider2D RadarReference;
    public CircleCollider2D HitboxReference;

    #endregion

    protected const float AnchorPointProximity_ForgivenessMargin = 0.15f;

    /// <summary>
    /// BaseDamage is the initial damage modifier associated with the tower. Value doesn't change.
    /// </summary>
    public static int BaseDamage;

    /// <summary>
    /// Damage determines how much Health is reduced from an enemy when they get attacked. It is initialized using BaseDamage value, but may scale based on other modifiers.
    /// </summary>
    public int Damage = BaseDamage;

    /// <summary>
    /// Agility determines how much time (in miliseconds) must ellapse between the unit's last attack and their next attack (aka AttackSpeed).
    /// </summary>
    public long Agility;

    /// <summary>
    /// Range determines the size of the circular area in which a unit can target an enemy.
    /// </summary>
    public float Range;
    
    /// <summary>
    /// Speed determines how quickly the unit moves (walks/runs) towards a target.
    /// </summary>
    public float Speed;
    
    protected Stopwatch AttackTimer = new Stopwatch();
    
    public TowerAttackType AttackType;
    public TowerTargetingStyle TargetingStyle;    

    public List<Enemy> EnemiesTargeted = new List<Enemy>();
    public List<Enemy> EnemiesInRange = new List<Enemy>();


    // *************************************************************************************************** //


    protected abstract void DetermineObjective();
    protected abstract void Attack();
    
    protected void InflictDamage(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (RadarReference != null)
        {
            // Show "Radar" collider in green outline
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(RadarReference.transform.position, RadarReference.radius);
        }

        if (HitboxReference != null)
        {
            // Show "Hitbox" collider in red outline
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(HitboxReference.transform.position, HitboxReference.radius);
        }
    }

    protected virtual void MoveToTarget(Vector3 target)
    {
        // TODO: Activate movement animation?
        Vector3 dir = target - this.transform.position;
        transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
    }
}

public enum TowerAttackType
{
    Melee,
    RangePhysical,
    RangeMagical
}

public enum TowerTargetingStyle
{
    SingleTarget,
    MultiTarget,
    AOE
}
