using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Hitbox hitbox;

    private bool hasHit = false;
    private bool isUlt = false;

    private void Awake()
    {
        if (!player) player = GetComponent<PlayerController>();
        if (!hitbox) hitbox = GetComponentInChildren<Hitbox>();

        if (!hitbox)
        {
            Debug.LogError("AttackController: Hitbox reference missing.", this);
            enabled = false;
            return;
        }

        hitbox.Owner = player;
    }

    public void EnableHitbox(bool ult = false)
    {
        isUlt = ult;
        hasHit = false;
        hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitbox.SetActive(false);
    }

    public void TryApplyHit(Hurtbox target, AttackData attack)
    {
        if (hasHit)
            return;

        if (target == null || target.Owner == null || target.Owner.HP == null)
            return;

        target.Owner.HP.DealDamage(attack.damage, player, attack.knockbackForce, isUlt);

        if (attack != null && attack.hitstun > 0f)
            StartCoroutine(ApplyHitstun(target.Owner, attack.hitstun));

        hasHit = true;
    }
    private IEnumerator ApplyHitstun(PlayerController target, float duration)
    {
        target.EnterHitstun();
        yield return new WaitForSeconds(duration);
        target.ExitHitstun();
    }
}