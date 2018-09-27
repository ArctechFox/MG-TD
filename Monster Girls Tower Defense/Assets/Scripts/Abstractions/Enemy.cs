using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Enemy : MonoBehaviour
{
    public Transform HealthBar;
    protected Slider HealthBarSlider;

    public int HealthMax;
    public int HealthRemaining;

    public float MovementSpeed;
    public int Damage;

    public float HealthBarPosition;

    protected Transform Target { get; set; }
    protected int WaypointIndex { get; set; } = 0;

    protected const float WaypointProximity_ForgivenessMargin = 0.1f;

    // ***************************************** //

    public void TakeDamage(int damage)
    {
        //TODO: Factor in resistances, armor, etc
        this.HealthRemaining -= damage;
        HealthBarSlider.value -= damage;
    }

    protected void SetHealthBar()
    {
        HealthRemaining = HealthMax;
        var healthBarPosition = transform.position;
        healthBarPosition.y += HealthBarPosition;

        HealthBar = Instantiate(HealthBar, healthBarPosition, transform.rotation);
        HealthBar.SetParent(transform, true);
        HealthBarSlider = HealthBar.Find("HBGuts").GetComponent<Slider>();
        HealthBarSlider.maxValue = HealthMax;
        HealthBarSlider.value = HealthRemaining;
    }

    public abstract void Die();

    protected virtual void GetNextWaypoint()
    {
        if (WaypointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        WaypointIndex++;
        Target = Waypoints.points[WaypointIndex];
    }

    protected virtual void Move()
    {
        if (Target == null) GetNextWaypoint();

        Vector3 dir = Target.position - transform.position;
        transform.Translate(dir.normalized * MovementSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, Target.position) <= WaypointProximity_ForgivenessMargin)
        {
            GetNextWaypoint();
        }
    }
}
