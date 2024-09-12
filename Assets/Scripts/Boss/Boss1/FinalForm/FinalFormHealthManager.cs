using UnityEngine;

public class FinalFormHealthManager : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;
    private float _currentHp;
    private FinalFormStateMachine _finalformStateMachine;
    private UIManager _uiManager;
    [SerializeField] private GameObject[] _effect;

    private void Awake()
    {
        _currentHp = _maxHp;
        _finalformStateMachine = GetComponent<FinalFormStateMachine>();
        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UIManager not found in the scene. Make sure there is exactly one UIManager in the scene.");
        }
    }

    public void TakeDamage(float amount)
    {
        _currentHp -= amount;
        _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);
        if (_currentHp <= 0)
        {
            _finalformStateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Die);
            Invoke("DisplayWinScreen", 2f); // Invoke the method after 2 seconds
        }
        else if (amount >= 20 && _finalformStateMachine._canHurt)
        {
            _finalformStateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Hit);
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

    private void DisplayWinScreen()
    {
        if (_uiManager != null)
        {
            _uiManager.DisplayWinScreen(); 
        }
    }
}
