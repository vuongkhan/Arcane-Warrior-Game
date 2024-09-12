using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TriggerFire: MonoBehaviour
{
    public float damage = 30f;
    private Animator _animator;
    public float knockbackForce = 0;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("isplayer");
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
    private void Destroy()
        {
            Destroy(gameObject);
        }

}
