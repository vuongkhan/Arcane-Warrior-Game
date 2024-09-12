using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;
    private float _currentHp;

    [SerializeField] private float _maxMana = 100f; 
    private float _currentMana;

    [SerializeField] private float _manaRegenRate = 1f; 
    private float _manaRegenCooldown = 1f; 
    private float _manaRegenTimer = 0f;

    private PlayerStateMachine _playerStateMachine;
    [SerializeField] private UIManager _uiManager;

    private void Awake()
    {
        _currentHp = _maxHp;
        _currentMana = _maxMana;
        _playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        RegenerateManaOverTime();
    }

    private void RegenerateManaOverTime()
    {
        _manaRegenTimer += Time.deltaTime;
        if (_manaRegenTimer >= _manaRegenCooldown)
        {
            RestoreMana(_manaRegenRate); 
            _manaRegenTimer = 0f; 
        }
    }

    public void TakeDamage()
    {
        if (_currentHp <= 0)
        {
            GameOver();
            _playerStateMachine.QueueNextState(PlayerStateMachine.PlayerState.Die);
        }
        else if (_currentHp > 0)
        {
            _playerStateMachine.QueueNextState(PlayerStateMachine.PlayerState.Hit);
        }
    }

    public void ReduceHealth(float amount)
    {
        _currentHp -= amount;
        if (_currentHp < 0) _currentHp = 0;
        TakeDamage();
    }

    public void ReduceMana(float amount)
    {
        _currentMana -= amount;
        if (_currentMana < 0) _currentMana = 0;
    }

    public void RestoreMana(float amount)
    {
        _currentMana += amount;
        if (_currentMana > _maxMana) _currentMana = _maxMana; 
    }


    private void GameOver()
    {
        if (_uiManager != null)
        {
            _uiManager.DisplayGameOverScreen();
        }
        else
        {
            Debug.LogError("UIManager cannot found!");
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

    public float GetCurrentMana()
    {
        return _currentMana;
    }

    public float GetMaxMana()
    {
        return _maxMana;
    }
}
