using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire : MonoBehaviour
{
    public float damage=10f;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManager healthmanager = other.gameObject.GetComponent<HealthManager>();

            if (healthmanager != null)
            {
                healthmanager.ReduceHealth(damage);
            }
        }
    }
}
