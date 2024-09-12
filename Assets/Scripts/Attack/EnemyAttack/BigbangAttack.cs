using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BigbangAttack : MonoBehaviour
{
    public float damage = 30f;
    public float knockbackForce = -8f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.ReduceHealth(damage);
            }
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackVelocity = new Vector2(knockbackForce, 0);  
                playerRb.velocity = knockbackVelocity;
            }
        }
    }

}
