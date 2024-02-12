using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerCollision))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _PlayerAnimation;
    [SerializeField] private PlayerCollision _PlayerCollision;
    [SerializeField] private PlayerHP _PlayerHP;
    public Rigidbody _Rb;
    public PlayerData _PlayerData;
    private Vector3 _moveDirection;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 3.0f;
    public float doubleJumpForce = 1.0f;
    private float charRotation = 180f;

    [Header("Movement Limits")]
    public float minX = -33.0f;
    public float maxX = -5.0f;

    public bool isHit = false;
    public bool isGrounded = false;
    public bool isAttacking = false;
    public bool isJumping = false;
    public bool canDoubleJump = false;

    private void Start()
    {
        _PlayerData = new PlayerData();
        _Rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (gameObject.name is "Player" && !isHit)
            PlayerMovement();
    }

    private void PlayerMovement() 
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal");
        Vector3 newPosition = transform.position + new Vector3(moveX * moveSpeed * Time.deltaTime, 0f, 0f); // Calculate new position
        transform.position = newPosition;

        // Clamp the X-axis position
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;

        PlayerAnim(moveX);

        // Flip character if moving in the opposite direction
        if (moveX < 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (moveX > 0)
            transform.rotation = Quaternion.Euler(0, charRotation, 0);

        if (isGrounded && !isJumping) isJumping = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded && isJumping)
            {
                _Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _moveDirection.y = jumpForce;
                isJumping = false;
                canDoubleJump = true;
                isGrounded = false;
            }
            else if (canDoubleJump)
            {
                // Apply double jump force only on the y-component
                _Rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
                canDoubleJump = false;
                isGrounded = false;
            }
        }

        // Attacking
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.S) && !isGrounded)
        {
            _Rb.AddForce(Vector3.down * 35, ForceMode.Impulse);
        }
    }

    private void PlayerAnim(float input) 
    {
        if (input != 0 && isGrounded && !isAttacking)
        {
            _PlayerAnimation.RunAnim();
        }
        else if (input == 0 && isGrounded && !isAttacking) 
        {
            _PlayerAnimation.IdleAnim();
        }
        else if(!isGrounded && !isAttacking)
        {
            _PlayerAnimation.JumpAnim();
        }
    }

    public void Attack()
    {
        isAttacking = true;
        _PlayerAnimation.AttackAnim();
    }
}