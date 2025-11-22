using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private ParticleSystem _BloodEffect;

    [Header("Health params")]
    private int maxHealth = 100;
    [SerializeField] private Image healthBar;
    [SerializeField] private Color midHealth;
    [SerializeField] private Color lowHealth;

    private int _currentHealth;
    private Color _fullHealthColor;

    private void Start()
    {
        int dataHealth = _PlayerController._PlayerData != null
            ? _PlayerController._PlayerData.health
            : maxHealth;

        _currentHealth = dataHealth > 0 ? dataHealth : maxHealth;
        maxHealth = Mathf.Max(maxHealth, _currentHealth);

        if (healthBar)
        {
            _fullHealthColor = healthBar.color;
            healthBar.fillAmount = 1f;
        }
    }

    public void DealDmg(int damageDealt)
    {
        if (!_PlayerController.IsAlive)
            return;

        if (damageDealt <= 0)
            return;

        if (_BloodEffect)
            _BloodEffect.Play();

        _currentHealth = Mathf.Max(0, _currentHealth - damageDealt);

        float normalized = (float)_currentHealth / maxHealth;
        if (healthBar)
        {
            healthBar.fillAmount = normalized;
            UpdateHealthBarColor(normalized);
        }

        ApplyKnockback(GameManager.Instance.EnemyPlayer.transform);

        if (_currentHealth <= 0)
        {
            _PlayerController.IsAlive = false;
            _PlayerController.PlayerAnimation.PlayDie();
            // TODO: disable input, send RPC, etc.
        }

        _PlayerController.PlayerCollision.SetHit(false);
    }

    private void ApplyKnockback(Transform attacker)
    {
        if (!_PlayerController._Rb)
            return;

        Vector3 dir = (transform.position - attacker.position).normalized;
        dir.y = 0f;
        _PlayerController._Rb.AddForce(dir * 40f, ForceMode.Impulse);
    }

    private void UpdateHealthBarColor(float normalized)
    {
        if (!healthBar) return;

        if (normalized > 0.6f)
            healthBar.color = _fullHealthColor;
        else if (normalized > 0.2f)
            healthBar.color = midHealth;
        else
            healthBar.color = lowHealth;
    }
}