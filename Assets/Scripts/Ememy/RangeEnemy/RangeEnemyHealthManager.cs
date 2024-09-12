using UnityEngine;

public class RangeEnemyHealthManager : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;
    private float _currentHp;
    private RangeEnemyStateMachine _enemyStateMachine;
    private bool hasDied = false;
    public GameObject[] _fire;
    private void Awake()
    {
        _currentHp = _maxHp;
        _enemyStateMachine = GetComponent<RangeEnemyStateMachine>();
    }
    public void TakeDamage(float amount)
    {
        if (hasDied) return; 
        _currentHp -= amount;
        _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);  
        if (_currentHp <= 0)
        {
            foreach (GameObject fireObject in _fire)
            {
                if (fireObject != null)  
                {
                    Destroy(fireObject);
                }
            }

            Debug.Log("kich hoat");
            hasDied = true;
            _enemyStateMachine.QueueNextState(RangeEnemyStateMachine.RangeEnemy.Die);
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
