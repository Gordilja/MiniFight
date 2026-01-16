using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class SimpleFighterAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Ranges")]
    [SerializeField] private float approachRange = 6f;
    [SerializeField] private float attackRange = 2.2f;
    [SerializeField] private float retreatRange = 0.8f;

    [Header("Attack Behaviour")]
    [SerializeField] private float minAttackCooldown = 0.6f;
    [SerializeField] private float maxAttackCooldown = 1.4f;
    [SerializeField, Range(0f, 1f)] private float ultChance = 0.15f;

    [Header("Jump Behaviour")]
    [SerializeField, Range(0f, 1f)] private float jumpChance = 0.15f;
    [SerializeField] private float jumpCooldown = 1.0f;
    private float _nextJumpTime;

    [Header("Jump Attack Behaviour")]
    [SerializeField, Range(0f, 1f)] private float jumpAttackChance = 0.25f;

    [Header("Block Behaviour")]
    [SerializeField] private bool useBlocking = true;
    [SerializeField, Range(0f, 1f)] private float blockProbability = 0.3f;
    [SerializeField] private float blockDuration = 0.5f;
    [SerializeField] private float blockCooldown = 2f;
    [SerializeField] private float blockTriggerRange = 2.8f;

    private PlayerController _controller;
    private PlayerStateMachine _state;

    private float _nextAttackTime;
    private bool _isBlocking;
    private float _blockEndTime;
    private float _nextBlockTime;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _state = _controller.State;
    }

    private void Start()
    {
        if (target == null && GameManager.Instance != null && GameManager.Instance.EnemyPlayer != null)
            target = GameManager.Instance.EnemyPlayer.transform;

        ScheduleNextAttack();
        _nextBlockTime = Time.time + 1.5f;
        _nextJumpTime = Time.time + 1.0f;
    }

    private void Update()
    {
        if (!_state.IsAlive)
        {
            _controller.SetMoveInput(0f);
            _controller.SetBlockInput(false);
            return;
        }

        if (target == null)
        {
            _controller.SetMoveInput(0f);
            return;
        }

        if (GameManager.Instance && !GameManager.Instance.StartGame)
        {
            _controller.SetMoveInput(0f);
            return;
        }

        if (_isBlocking)
        {
            HandleBlocking();
            return;
        }

        if (_state.Action != PlayerStateMachine.ActionState.Normal)
        {
            _controller.SetMoveInput(0f);
            return;
        }

        HandleMovementAndCombat();
    }

    private void HandleMovementAndCombat()
    {
        Vector3 toTarget = target.position - transform.position;
        float dx = toTarget.x;
        float dist = Mathf.Abs(dx);

        float moveInput = 0f;

        if (dist > approachRange)
            moveInput = Mathf.Sign(dx);
        else if (dist > attackRange)
            moveInput = Mathf.Sign(dx);
        else if (dist < retreatRange)
            moveInput = -Mathf.Sign(dx);

        _controller.SetMoveInput(moveInput);

        TryJump(dist);

        if (useBlocking && Time.time >= _nextBlockTime && dist < blockTriggerRange)
        {
            if (Random.value < blockProbability)
            {
                StartBlock();
                return;
            }

            _nextBlockTime = Time.time + blockCooldown;
        }

        if (dist <= attackRange && Time.time >= _nextAttackTime)
            TryAttack();
    }

    private void TryJump(float dist)
    {
        if (Time.time < _nextJumpTime)
            return;

        bool shouldJump = false;

        if (dist > attackRange && Random.value < jumpChance)
            shouldJump = true;

        if (dist < retreatRange && Random.value < (jumpChance * 0.5f))
            shouldJump = true;

        if (!shouldJump)
            return;

        _nextJumpTime = Time.time + jumpCooldown;

        _controller.OnJumpInput();

        if (Random.value < jumpAttackChance)
            Invoke(nameof(PerformJumpAttack), 0.15f);
    }

    private void PerformJumpAttack()
    {
        if (_state.IsGrounded) return;
        if (_state.Action != PlayerStateMachine.ActionState.Normal) return;

        _controller.OnAttackInput();
    }

    private void TryAttack()
    {
        bool useUlt = Random.value < ultChance;

        if (useUlt)
            _controller.OnUltInput();
        else
            _controller.OnAttackInput();

        ScheduleNextAttack();
    }

    private void ScheduleNextAttack()
    {
        _nextAttackTime = Time.time + Random.Range(minAttackCooldown, maxAttackCooldown);
    }

    private void StartBlock()
    {
        _isBlocking = true;
        _blockEndTime = Time.time + blockDuration;
        _nextBlockTime = Time.time + blockCooldown;

        _controller.SetBlockInput(true);
        _controller.SetMoveInput(0f);
    }

    private void HandleBlocking()
    {
        if (Time.time >= _blockEndTime)
        {
            _isBlocking = false;
            _controller.SetBlockInput(false);
        }
        else
        {
            _controller.SetMoveInput(0f);
        }
    }
}