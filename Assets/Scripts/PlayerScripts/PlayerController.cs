using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCollision))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private UIDocument uiDoc;
    [SerializeField] private VisualElement uiElements;
    public PlayerData playerData;
    private Vector3 moveDirection;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public float doubleJumpForce = 5.0f;
    private float charRotation = 180f;

    [Header("Movement Limits")]
    public float minX = -33.0f;
    public float maxX = -5.0f;

    public bool isGrounded = false;
    public bool isAttacking = false;
    public bool isJumping = false;
    public bool canDoubleJump = false;

    private void Start()
    {
        playerData = new PlayerData();
        uiElements = uiDoc.rootVisualElement;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement() 
    {
        isGrounded = characterController.isGrounded;
        uiElements.Q<ProgressBar>("HP1").value = playerData.health;
        // Movement
        float moveX = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(moveX * moveSpeed, moveDirection.y, 0f); // Only modify the x-component for movement
        moveDirection.y += (Physics.gravity.y * 2.5f) * Time.deltaTime; // Apply gravity

        characterController.Move(moveDirection * Time.deltaTime);

        // Clamp the X-axis position
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;

        PlayerAnim(moveX);

        // Flip character if moving in the opposite direction
        if (moveDirection.x < 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (moveDirection.x > 0)
            transform.rotation = Quaternion.Euler(0, charRotation, 0);

        if (characterController.isGrounded && !isJumping) isJumping = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (characterController.isGrounded && isJumping)
            {
                // Apply jump force only on the y-component
                moveDirection.y = jumpForce;
                isJumping = false;
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                // Apply double jump force only on the y-component
                moveDirection.y = doubleJumpForce;
                canDoubleJump = false;
            }
        }

        // Attacking
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            Attack();
        }

        if (!Input.GetKey(KeyCode.J) && isAttacking)
        {
            isAttacking = false;
        }
    }

    private void PlayerAnim(float input) 
    {
        if (input != 0 && characterController.isGrounded && !isAttacking)
        {
            playerAnimation.RunAnim();
        }
        else if (input == 0 && characterController.isGrounded && !isAttacking) 
        {
            playerAnimation.IdleAnim();
        }
        else if(!characterController.isGrounded && !isAttacking)
        {
            playerAnimation.JumpAnim();
        }
    }

    public void Attack()
    {
        isAttacking = true;
        playerAnimation.AttackAnim();
    }
}