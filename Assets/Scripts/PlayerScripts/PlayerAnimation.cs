using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private PlayerCollision _PlayerDistanceControl;
    [SerializeField] private Animator _PlayerAnimator;
    [SerializeField] private PlayerHP _PlayerHP;

    private string currentState;
    const int layer_UPPERBODY = 1;
    const string player_IDLE = "Idle";
    const string player_RUN = "Run";
    const string player_JUMP = "Jump";
    const string player_ATTACK = "Attack";
    const string player_DIE = "Die";
    const string player_VICTORY = "Victory";

    public void IdleAnim()
    {    
        ChangeAnimState(player_IDLE, _PlayerAnimator);
        SetLayerWeight(0);
    }

    public void RunAnim()
    {
        ChangeAnimState(player_RUN, _PlayerAnimator);
        SetLayerWeight(1);
    }

    public void JumpAnim()
    {
        ChangeAnimState(player_JUMP, _PlayerAnimator);
        SetLayerWeight(1);
    }

    public void AttackAnim()
    {
        ChangeAnimState(player_ATTACK, _PlayerAnimator);
        SetLayerWeight(0);
    }

    public void DieAnim()
    {
        ChangeAnimState(player_DIE, _PlayerAnimator);
        SetLayerWeight(0);
    }

    public void VictoryAnim()
    {
        ChangeAnimState(player_VICTORY, _PlayerAnimator);
        SetLayerWeight(0);
    }

    private void SetLayerWeight(float weight) 
    {
        _PlayerAnimator.SetLayerWeight(layer_UPPERBODY, weight);
    }

    private void ChangeAnimState(string newState, Animator animator)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    public void AttackAnimEnd() 
    {
        _PlayerController.isAttacking = false;
    }
}