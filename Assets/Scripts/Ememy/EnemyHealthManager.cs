using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;
    private float _currentHp;
    private EnemyStateMachine _enemyStateMachine;
    private bool hasDied = false;

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
            _enemyStateMachine.QueueNextState(EnemyStateMachine.EnemyState.Die);
        }
        else
        {
            _enemyStateMachine.QueueNextState(EnemyStateMachine.EnemyState.Hit);
        }
    }

    public void ReduceHealth(float amount)
    {
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
