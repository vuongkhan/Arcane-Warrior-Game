using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Stone : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;
    private float _currentHp;
    private EnemyStateMachine _enemyStateMachine;
    private bool hasDied = false;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject objectdestroy;
    [SerializeField] private GameObject isdamage;
    private void Awake()
    {
        _currentHp = _maxHp;
        _enemyStateMachine = GetComponent<EnemyStateMachine>();
    }

    public void TakeDamage(float amount)
    {

        if (hasDied) return;  
        _currentHp -= amount;
        _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);  
        if (_currentHp <= 0)
        {
            hasDied = true;  
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(objectdestroy);
            Destroy(gameObject);
        }
    }

    public void ReduceHealth(float amount)
    {
        Instantiate(isdamage, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        if (!hasDied)
        {
            TakeDamage(amount);
        }
    }

    public float GetCurrentHealth()
    {
        return _currentHp;
    }

    public float GetMaxHealth()
    {
        return _maxHp;
    }
}
