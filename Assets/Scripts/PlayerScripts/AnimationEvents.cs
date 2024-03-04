using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _PlayerAnimation;
    [SerializeField] private PlayerController _PlayerController;

    public void Attack() 
    {
        _PlayerAnimation.AttackAnimEnd();
    }

    public void AttackEnd() 
    {
        _PlayerController.isAttacking = false;
    }
}