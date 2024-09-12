using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInvoke : MonoBehaviour
{
    public float timeBeforeDestroy = 1.0f;
    public float damage = 25f;
    void Start()
    {
        Invoke("seflfDestroy", timeBeforeDestroy);
    }
    private void seflfDestroy()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthManager healthManager = collision.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.ReduceHealth(damage);
            }
        }
    }
}
