using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private PlayerAnimation _PlayerAnimation;
    [SerializeField] private PlayerDistanceControl _PlayerDistanceControl;
    [SerializeField] private ParticleSystem _bloodEffect;
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

        _bloodEffect.Play();
         _PlayerHealth -= damageDealt;
        _currentHealth = (float)_PlayerHealth / 100;
        _healthBar.fillAmount = _currentHealth;
        var opposite = OppositeDir();
        Debug.Log($"Opposite: {opposite}");
        _PlayerController._Rb.AddForce(opposite * 40, ForceMode.Impulse);
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

    private Vector3 OppositeDir() 
    {
        if (_PlayerDistanceControl._enemyPlayer.transform.rotation.y == 0)
        {
            return Vector3.left;
        }
        else 
        {
            return Vector3.right;
        }
    }
}
