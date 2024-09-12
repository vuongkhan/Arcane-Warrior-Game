using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private Animator animator;
    public float damage = 20f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("Touch");

        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager healthmanager = collision.gameObject.GetComponent<HealthManager>();

            if (healthmanager != null)
            {
                healthmanager.ReduceHealth(damage);
            }
        }
    }


    public void HandleAnimation()
    {
        Destroy(gameObject);
    }
}
