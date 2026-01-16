using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum LifeState { Alive, Dead }
    public enum LocomotionState { Grounded, Rising, Falling }
    public enum ActionState { Normal, Attacking, Hitstun }

    public LifeState Life { get; private set; } = LifeState.Alive;
    public LocomotionState Loco { get; private set; } = LocomotionState.Grounded;
    public ActionState Action { get; private set; } = ActionState.Normal;

    public bool IsBlocking { get; private set; } = false;
    public bool CanDoubleJump { get; private set; } = false;

    public bool IsAlive => Life == LifeState.Alive;
    public bool IsGrounded => Loco == LocomotionState.Grounded;

    public bool CanMove => IsAlive && Action == ActionState.Normal && !IsBlocking;
    public bool CanAttack => IsAlive && Action == ActionState.Normal && !IsBlocking;
    public bool CanJump => IsAlive && Action == ActionState.Normal && !IsBlocking;

    // Optional: expose for debugging in inspector
    [SerializeField] private bool debugLogTransitions = false;

    // -------------------------
    // State setters / transitions
    // -------------------------
    public void SetLife(LifeState state)
    {
        if (Life == state) return;

        Life = state;
        if (debugLogTransitions) Debug.Log($"[State] Life -> {Life}", this);

        if (Life == LifeState.Dead)
        {
            SetAction(ActionState.Normal);
            SetBlocking(false);
            CanDoubleJump = false;
        }
    }

    public void SetAction(ActionState state)
    {
        if (Action == state) return;

        Action = state;
        if (debugLogTransitions) Debug.Log($"[State] Action -> {Action}", this);

        if (Action != ActionState.Normal)
            SetBlocking(false);
    }

    public void SetLocomotion(LocomotionState state)
    {
        if (Loco == state) return;

        Loco = state;
        if (debugLogTransitions) Debug.Log($"[State] Loco -> {Loco}", this);

        if (Loco == LocomotionState.Grounded)
            CanDoubleJump = false;
    }

    public void SetGrounded(bool grounded)
    {
        if (grounded)
        {
            SetLocomotion(LocomotionState.Grounded);
        }
        else if (IsGrounded)
        {
            SetLocomotion(LocomotionState.Falling);
        }
    }

    public void SetBlocking(bool pressed)
    {
        if (!IsAlive || Action != ActionState.Normal)
        {
            IsBlocking = false;
            return;
        }

        IsBlocking = pressed;
        if (debugLogTransitions) Debug.Log($"[State] Blocking -> {IsBlocking}", this);
    }

    // -------------------------
    // “Intent” transitions
    // -------------------------
    public void StartAttack()
    {
        if (!CanAttack) return;
        SetAction(ActionState.Attacking);
    }

    public void EndAttack()
    {
        if (!IsAlive) return;
        if (Action == ActionState.Attacking)
            SetAction(ActionState.Normal);
    }

    public void EnterHitstun()
    {
        if (!IsAlive) return;
        SetAction(ActionState.Hitstun);
    }

    public void ExitHitstun()
    {
        if (!IsAlive) return;
        if (Action == ActionState.Hitstun)
            SetAction(ActionState.Normal);
    }

    public void AllowDoubleJump(bool allowed)
    {
        CanDoubleJump = allowed;
    }
}