using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;  
    private float _currentHp;
    private Boss1StateMachine _boss1StateMachine;
    [SerializeField] private GameObject[] _effect; 

    private void Awake()
    {
        _currentHp = Mathf.Clamp(_maxHp, 0, _maxHp); 
        _boss1StateMachine = GetComponent<Boss1StateMachine>();
    }

    public void TakeDamage(float amount)
    {
        _currentHp -= amount;
        _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);

        if (_currentHp <= 0)
        {
            _boss1StateMachine.QueueNextState(Boss1StateMachine.Boss1State.Die);
        }
        else if (_boss1StateMachine._canHurt && amount >= 30)
        {
            _boss1StateMachine.QueueNextState(Boss1StateMachine.Boss1State.Hit);
        }
    }

    public void ReduceHealth(float amount)
    {
        if (_effect.Length > 0 && _effect[0] != null)
        {
            Instantiate(_effect[0], transform.position, Quaternion.identity);
        }

        TakeDamage(amount);
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
