using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerHP))]
public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    public PlayerAnimation PlayerAnimation;
    public PlayerCollision PlayerCollision;
    public PlayerHP PlayerHP;
    [SerializeField] private List<Character> _Characters;
    [SerializeField] public Rigidbody _Rb;

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

    // State
    public bool IsHit = false;
    public bool IsGrounded = false;
    public bool IsAttacking = false;
    public bool IsJumping = false;
    public bool CanDoubleJump = false;
    public bool IsAlive = true;
    public bool IsBlocking = false; // TODO: later add facing check in collision

    // Input cache
    private float _moveInputX;

    private void Awake()
    {
        for (int i = 0; i < _Characters.Count; i++)
        {
            if (i == (int)_PlayerData.type)
            {
                _Characters[i].gameObject.SetActive(true);
                PlayerAnimation = _Characters[i].Animation;
                PlayerCollision.Sword = _Characters[i].Sword;
            }
            else 
            {
                _Characters[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (!IsAlive || IsHit)
            return;

        HandleHorizontalMovement();
        PlayerAnimation.UpdateLocomotion(_moveInputX, IsGrounded, IsBlocking);
    }

    private void FixedUpdate()
    {
        if (!IsGrounded && _Rb.linearVelocity.y < 0f)
        {
            // Apply extra gravity when falling
            _Rb.AddForce(Vector3.down * 30f, ForceMode.Acceleration);
        }
    }

    private void HandleHorizontalMovement()
    {
        var pos = transform.position;
        pos.x += _moveInputX * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;

        if (_moveInputX < -0.01f)
            transform.rotation = Quaternion.Euler(0f, charRotationLeft, 0f);
        else if (_moveInputX > 0.01f)
            transform.rotation = Quaternion.Euler(0f, charRotationRight, 0f);
    }

    public void SetGrounded(bool grounded)
    {
        IsGrounded = grounded;
        if (grounded && IsJumping) 
        {
            IsJumping = false;
        }
    }

    // --------- called from InputManager ---------

    public void SetMoveInput(float value)
    {
        _moveInputX = value;
    }

    public void OnJumpInput()
    {
        if (!IsAlive)
            return;

        if (IsGrounded && !IsJumping)
        {
            _Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            IsJumping = true;
            CanDoubleJump = true;
            IsGrounded = false;
        }
        else if (CanDoubleJump)
        {
            _Rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            CanDoubleJump = false;
            IsGrounded = false;
        }
    }

    public void OnUltInput()
    {
        if (!IsAlive)
            return;

        IsAttacking = true;
        PlayerAnimation.PlayUlt();
    }

    public void OnAttackInput()
    {
        if (!IsAlive || IsAttacking)
            return;

        IsAttacking = true;
        int attackIndex = Random.value > 0.5f ? 0 : 1;
        PlayerAnimation.PlayAttack(attackIndex);
    }

    public void OnDownSlamInput()
    {
        if (!IsAlive || IsGrounded)
            return;

        _Rb.AddForce(Vector3.down * 35f, ForceMode.Impulse);
    }

    public void SetBlockInput(bool pressed)
    {
        IsBlocking = pressed; // TODO: collision side check will decide if block succeeds
    }
}