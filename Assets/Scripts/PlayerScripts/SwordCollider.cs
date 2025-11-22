using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    public PlayerController Owner;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerCollision>(out var target))
            return;

        if (target.Sword.Owner == Owner)
            return; // ignore self-hit

        if (target.Blocking())
            return;

        target.TakeDamage();
    }
}