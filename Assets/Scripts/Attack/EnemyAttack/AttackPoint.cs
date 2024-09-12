using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public GameObject explode;
    private Animator _animator;
    private Rigidbody2D _rb;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb= GetComponent<Rigidbody2D>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _animator.SetTrigger("End");
 
        }
    }
    public void Destroy()
    {
        Vector3 adjustedPosition = new Vector3(_rb.position.x, _rb.position.y - 0.6f); 
        GameObject projectile = GameObject.Instantiate(explode, adjustedPosition, Quaternion.identity);
        
        Destroy(gameObject);

    }
}
