using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private PlayerController _controller;
    [SerializeField] private PlayerAnimation _anim;
    [SerializeField] private ParticleSystem _bloodEffect;

    [Header("Health params")]
    [SerializeField] private Image _healthBar;
    [SerializeField] private Color midHealth;
    [SerializeField] private Color lowHealth;

    private int maxHealth = 100;
    private int _currentHealth;
    private Color _fullColor;

    public void Init(PlayerController c)
    {
        _controller = c;
    }

    private void Awake()
    {
        if (!_controller) _controller = GetComponent<PlayerController>();
        if (!_anim) _anim = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;

        if (_healthBar)
        {
            _fullColor = _healthBar.color;
            _healthBar.fillAmount = 1;
        }
    }

    public void DealDamage(int dmg, PlayerController attacker, float knockback, bool isUlt)
    {
        if (!_controller.State.IsAlive)
            return;

        if (_bloodEffect) _bloodEffect.Play();

        _currentHealth = Mathf.Max(0, _currentHealth - dmg);
        UpdateBar();

        if(isUlt)
            ApplyKnockback(attacker, knockback);

        if (_currentHealth <= 0)
            Die();
    }

    private void ApplyKnockback(PlayerController attacker, float force)
    {
        Vector3 dir = (transform.position - attacker.transform.position).normalized;
        dir.y = 0;
        _controller._Rb.AddForce(dir * force, ForceMode.Impulse);
    }

    private void UpdateBar()
    {
        if (!_healthBar) return;

        float normalized = (float)_currentHealth / maxHealth;
        _healthBar.fillAmount = normalized;

        if (normalized > 0.6f)
            _healthBar.color = _fullColor;
        else if (normalized > 0.2f)
            _healthBar.color = midHealth;
        else
            _healthBar.color = lowHealth;
    }

    private void Die()
    {
        _controller.State.SetLife(PlayerStateMachine.LifeState.Dead);
        _anim.PlayDie();
    }
}