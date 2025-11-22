using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController _PlayerController;
    [HideInInspector] public SwordCollider Sword;

    private bool IsHit = false;

    public void TakeDamage()
    {
        _PlayerController.PlayerHP.DealDmg(5);
    }

    public void SetHit(bool active) 
    {
        IsHit = active;
    }

    public bool CheckHit()
    {
        return IsHit;
    }

    public bool Blocking() 
    {
        return _PlayerController.IsBlocking;
    }
}