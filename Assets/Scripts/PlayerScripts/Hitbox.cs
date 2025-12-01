using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public PlayerController Owner;
    public AttackData AttackData;

    private bool active = false;

    public void SetActive(bool value)
    {
        active = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
            return;

        if (!other.TryGetComponent<Hurtbox>(out var hurtbox))
            return;

        if (hurtbox.Owner == Owner)
            return; // Ignore self-hit

        // BLOCK CHECK
        if (hurtbox.Owner.IsBlocking && IsFacingAttacker(hurtbox.Owner))
            return;

        // Apply hit through attack controller
        Owner.AttackController.TryApplyHit(hurtbox, AttackData);
    }

    private bool IsFacingAttacker(PlayerController target)
    {
        Vector3 toAttacker = (Owner.transform.position - target.transform.position).normalized;
        float dot = Vector3.Dot(target.transform.forward, toAttacker);
        return dot > 0.25f; // tweak threshold if needed
    }
}