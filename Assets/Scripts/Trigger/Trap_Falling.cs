using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Falling : MonoBehaviour
{
    public GameObject trap;
    private Rigidbody2D _trapRb;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (trap != null)
        {
            if (other.CompareTag("Player"))
            {
                _trapRb = trap.GetComponent<Rigidbody2D>();
                if (_trapRb == null)
                {
                    _trapRb = trap.AddComponent<Rigidbody2D>();
                }
                _trapRb.bodyType = RigidbodyType2D.Dynamic;
                _trapRb.gravityScale = 2f;
                Destroy(gameObject);
            }
        }
    }
}
