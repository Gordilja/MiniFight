using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _PlayerAnimation;

    public void Attack() 
    {
        _PlayerAnimation.AttackAnimEnd();
    }
}