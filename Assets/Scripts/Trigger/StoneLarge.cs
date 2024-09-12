using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneLarge : MonoBehaviour
{
    public float damage = 8f;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager healthmanager = collision.gameObject.GetComponent<HealthManager>();

            if (healthmanager != null)
            {
                healthmanager.ReduceHealth(damage);
            }
        }
    }
}
