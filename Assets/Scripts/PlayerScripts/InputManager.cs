using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Target player")]
    [SerializeField] private PlayerController _Player;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference _MoveAction;
    [SerializeField] private InputActionReference _JumpAction;
    [SerializeField] private InputActionReference _AttackAction;
    [SerializeField] private InputActionReference _BlockAction;
    [SerializeField] private InputActionReference _UltAction;

    private void OnEnable()
    {
        if (_Player == null)
        {
            Debug.LogError("InputManager: PlayerController reference not set.");
            enabled = false;
            return;
        }

        if (_MoveAction != null)
        {
            _MoveAction.action.Enable();
            _MoveAction.action.performed += OnMovePerformed;
            _MoveAction.action.canceled += OnMoveCanceled;
        }

        if (_JumpAction != null)
        {
            _JumpAction.action.Enable();
            _JumpAction.action.performed += OnJumpPerformed;
        }

        if (_AttackAction != null)
        {
            _AttackAction.action.Enable();
            _AttackAction.action.performed += OnAttackPerformed;
        }

        if (_BlockAction != null)
        {
            _BlockAction.action.Enable();
            _BlockAction.action.performed += OnBlockPerformed;
            _BlockAction.action.canceled += OnBlockCanceled;
        }

        if (_UltAction != null)
        {
            _UltAction.action.Enable();
            _UltAction.action.performed += OnUltPerformed;
        }
    }

    private void OnDisable()
    {
        if (_MoveAction != null)
        {
            _MoveAction.action.performed -= OnMovePerformed;
            _MoveAction.action.canceled -= OnMoveCanceled;
            _MoveAction.action.Disable();
        }

        if (_JumpAction != null)
        {
            _JumpAction.action.performed -= OnJumpPerformed;
            _JumpAction.action.Disable();
        }

        if (_AttackAction != null)
        {
            _AttackAction.action.performed -= OnAttackPerformed;
            _AttackAction.action.Disable();
        }

        if (_BlockAction != null)
        {
            _BlockAction.action.performed -= OnBlockPerformed;
            _BlockAction.action.canceled -= OnBlockCanceled;
            _BlockAction.action.Disable();
        }

        if (_UltAction != null)
        {
            _UltAction.action.performed -= OnUltPerformed;
            _UltAction.action.Disable();
        }
    }

    // ------- callbacks -------

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        _Player.SetMoveInput(v.x);
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        _Player.SetMoveInput(0f);
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        _Player.OnJumpInput();
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        _Player.OnAttackInput();
    }

    private void OnBlockPerformed(InputAction.CallbackContext ctx)
    {
        if(!_Player.IsBlocking)
            _Player.SetBlockInput(true);
    }

    private void OnBlockCanceled(InputAction.CallbackContext ctx)
    {
        _Player.SetBlockInput(false);
    }

    private void OnUltPerformed(InputAction.CallbackContext ctx)
    {
        _Player.OnUltInput();
    }
}