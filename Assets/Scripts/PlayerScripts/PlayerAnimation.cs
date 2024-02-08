using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerController _controller;
    [SerializeField] Animator playerAnimator;
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
        ChangeAnimState(player_IDLE, playerAnimator);
        SetLayerWeight(0);
    }

    public void RunAnim()
    {
        ChangeAnimState(player_RUN, playerAnimator);
        SetLayerWeight(1);
    }

    public void JumpAnim()
    {
        ChangeAnimState(player_JUMP, playerAnimator);
        SetLayerWeight(1);
    }

    public void AttackAnim()
    {
        ChangeAnimState(player_ATTACK, playerAnimator);
        SetLayerWeight(0);
    }

    public void DieAnim()
    {
        ChangeAnimState(player_DIE, playerAnimator);
        SetLayerWeight(0);
    }

    public void VictoryAnim()
    {
        ChangeAnimState(player_VICTORY, playerAnimator);
        SetLayerWeight(0);
    }

    private void SetLayerWeight(float weight) 
    {
        playerAnimator.SetLayerWeight(layer_UPPERBODY, weight);
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
        _controller.isAttacking = false;
        // Do dmg
    }

}