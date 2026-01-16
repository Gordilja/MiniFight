using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private Animator _Anim;

    // Animator params
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int GroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int BlockingHash = Animator.StringToHash("IsBlocking");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int UltTrigger = Animator.StringToHash("Ult");
    private static readonly int AttackIndexHash = Animator.StringToHash("AttackIndex");

    // Clip names for direct play
    private const string STATE_DIE = "Die";
    private const string STATE_VICTORY = "Victory";
    private const string STATE_GETUP = "GetUp";

    public void UpdateLocomotion(float moveInputX, bool isGrounded, bool isBlocking)
    {
        _Anim.SetFloat(SpeedHash, Mathf.Abs(moveInputX));
        _Anim.SetBool(GroundedHash, isGrounded);
        _Anim.SetBool(BlockingHash, isBlocking);
    }

    /// AttackIndex = 0 or 1, Animator graph chooses which clip.
    public void PlayAttack(int attackIndex)
    {
        _PlayerController.AttackController.EnableHitbox();
        _Anim.SetLayerWeight(1, 1);
        _Anim.SetFloat(AttackIndexHash, attackIndex);
        _Anim.SetTrigger(AttackTrigger);
    }

    /// <summary>
    /// Called from Animation Event at the end of both attack clips.
    /// </summary>
    public void OnAttackAnimationFinished()
    {
        _PlayerController.NotifyAttackEnded();
        _PlayerController.AttackController.DisableHitbox();
    }

    public void PlayUlt()
    {
        _PlayerController.AttackController.EnableHitbox(true);
        _Anim.SetTrigger(UltTrigger);
    }

    // ========================
    // CINEMATIC STATES (direct)
    // ========================

    public void PlayDie()
    {
        _Anim.SetLayerWeight(1, 0);
        _Anim.Play(STATE_DIE);    // or CrossFade if you prefer blend
    }

    public void PlayVictory()
    {
        // Lock player input however you want (new flag, or reuse something)
        _Anim.Play(STATE_VICTORY, 0, 0f);
        // Or: _anim.SetTrigger(VictoryTrigger); if you decide to keep it as a param
    }

    public void PlayGetUp()
    {
        // You can call this when you want to re-enable the player
        _Anim.Play(STATE_GETUP, 0, 0f);
        // After GetUp animation finishes (animation event),
        // you can re-enable input / clear IsDead etc.
    }

    // Optional: Animation Event at the end of GetUp
    public void OnGetUpFinished()
    {
        // Reset to locomotion blend tree automatically via params
    }
}