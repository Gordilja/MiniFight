using UnityEngine;

[CreateAssetMenu(menuName = "Combat/AttackData")]
public class AttackData : ScriptableObject
{
    public int damage = 10;
    public float knockbackForce = 40f;
    public float hitstun = 0.15f;
    public float blockstun = 0.1f;
}