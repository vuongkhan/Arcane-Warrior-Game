using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image _hpGreenImage;
    [SerializeField] private Image _manaBlueImage; 
    [SerializeField] private HealthManager _healthManager;

    private void Update()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }

    private void UpdateHealthBar()
    {
        if (_healthManager != null && _hpGreenImage != null)
        {
            float health = _healthManager.GetCurrentHealth();
            float maxHealth = _healthManager.GetMaxHealth();
            float healthPercentage = health / maxHealth;
            _hpGreenImage.fillAmount = healthPercentage;
        }
    }

    private void UpdateManaBar()
    {
        if (_healthManager != null && _manaBlueImage != null)
        {
            float mana = _healthManager.GetCurrentMana();
            float maxMana = _healthManager.GetMaxMana();
            float manaPercentage = mana / maxMana;
            _manaBlueImage.fillAmount = manaPercentage;
        }
    }
}
