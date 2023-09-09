using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private PlayerAnimation playerAnimation;

    public void Attack() 
    {
        playerAnimation.AttackAnimEnd();
    }
}
