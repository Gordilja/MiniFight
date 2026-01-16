using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerStateMachine))]
public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    public AttackController AttackController;
    public PlayerAnimation Animation;
    public PlayerHP HP;

    [SerializeField] public Rigidbody _Rb;
    [SerializeField] public PlayerStateMachine State;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpForce = 3.0f;
    [SerializeField] private float doubleJumpForce = 1.0f;

    [Header("Movement Limits")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;

    private const float charRotationRight = 180f;
    private const float charRotationLeft = 0f;

    public PlayerData _PlayerData;

    // Input cache
    private float _moveInputX;

    private void Awake()
    {
        if (!_Rb) _Rb = GetComponent<Rigidbody>();
        if (!State) State = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        if (!State.IsAlive || State.Action == PlayerStateMachine.ActionState.Hitstun)
            return;

        HandleHorizontalMovement();
        Animation.UpdateLocomotion(_moveInputX, State.IsGrounded, State.IsBlocking);
    }

    private void FixedUpdate()
    {
        UpdateLocomotionFromPhysics();

        if (!State.IsGrounded && _Rb.linearVelocity.y < 0f)
        {
            _Rb.AddForce(Vector3.down * 30f, ForceMode.Acceleration);
        }
    }

    private void HandleHorizontalMovement()
    {
        if (!State.CanMove)
            return;

        var pos = transform.position;
        pos.x += _moveInputX * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;

        if (_moveInputX < -0.01f)
            transform.rotation = Quaternion.Euler(0f, charRotationLeft, 0f);
        else if (_moveInputX > 0.01f)
            transform.rotation = Quaternion.Euler(0f, charRotationRight, 0f);
    }

    private void UpdateLocomotionFromPhysics()
    {
        if (State.IsGrounded)
            return;

        float vy = _Rb.linearVelocity.y;

        if (vy > 0.05f)
            State.SetLocomotion(PlayerStateMachine.LocomotionState.Rising);
        else if (vy < -0.05f)
            State.SetLocomotion(PlayerStateMachine.LocomotionState.Falling);
    }

    // Call this from your ground check / collision
    public void SetGrounded(bool grounded)
    {
        State.SetGrounded(grounded);
    }

    // -------------------------
    // INPUT / INTENT API
    // -------------------------
    public void SetMoveInput(float value)
    {
        _moveInputX = value;
    }

    public void SetBlockInput(bool pressed)
    {
        State.SetBlocking(pressed);

        // If block is pressed, stop drift
        if (State.IsBlocking)
            _moveInputX = 0f;
    }

    public void OnJumpInput()
    {
        if (!State.CanJump)
            return;

        if (State.IsGrounded)
        {
            _Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            State.SetGrounded(false);
            State.SetLocomotion(PlayerStateMachine.LocomotionState.Rising);
            State.AllowDoubleJump(true);
        }
        else if (State.CanDoubleJump)
        {
            _Rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            State.SetGrounded(false);
            State.SetLocomotion(PlayerStateMachine.LocomotionState.Rising);
            State.AllowDoubleJump(false);
        }
    }

    public void OnAttackInput()
    {
        if (!State.CanAttack)
            return;

        State.StartAttack();
        int attackIndex = Random.value > 0.5f ? 0 : 1;
        Animation.PlayAttack(attackIndex);
    }

    public void OnUltInput()
    {
        if (!State.CanAttack)
            return;

        State.StartAttack();
        Animation.PlayUlt();
    }

    public void OnDownSlamInput()
    {
        if (!State.IsAlive || State.IsGrounded)
            return;

        _Rb.AddForce(Vector3.down * 35f, ForceMode.Impulse);
    }

    // -------------------------
    // Animation / external hooks
    // -------------------------
    public void NotifyAttackEnded()
    {
        State.EndAttack();
    }

    public void EnterHitstun()
    {
        State.EnterHitstun();
        _moveInputX = 0f;
    }

    public void ExitHitstun()
    {
        State.ExitHitstun();
    }

    public void Kill()
    {
        State.SetLife(PlayerStateMachine.LifeState.Dead);
        _moveInputX = 0f;
        State.SetBlocking(false);
    }
}