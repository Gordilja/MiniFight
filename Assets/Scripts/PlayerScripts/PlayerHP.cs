using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public PlayerController _PlayerController;
    public PlayerAnimation _PlayerAnimation;
    public int _PlayerHealth;

    [Header("Health params")]
    public Image _healthBar;
    public Color midHealth;
    public Color lowHealth;

    private float _currentHealth;

    // OnSpawnPlayer later
    private void Start()
    {
        _PlayerHealth = _PlayerController._PlayerData.health;
        _healthBar.fillAmount = 1;
        _currentHealth = 1;
    }

    public void DealDmg(int damageDealt) 
    {
        if (!_PlayerController.isAlive) 
            return;

         _PlayerHealth -= damageDealt;
        _currentHealth = (float)_PlayerHealth / 100;
        _healthBar.fillAmount = _currentHealth;
        if (_currentHealth <= 0)
        {
            // isDead tru
            _PlayerController.isAlive = false;
            _PlayerAnimation.DieAnim();
        }
        HealthBarColor();
    }

    private void HealthBarColor() 
    {
        if (_currentHealth > 0.2f && _currentHealth <= 0.6f)
        {
            _healthBar.color = midHealth;
        }
        else if (_currentHealth <= 0.2f)
        {
            _healthBar.color = lowHealth;
        }
    }
}
