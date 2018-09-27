using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarScript : MonoBehaviour
{
    public Tower tower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            tower.EnemiesTargeted.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            tower.EnemiesTargeted.Remove(enemy);
        }
    }
}
