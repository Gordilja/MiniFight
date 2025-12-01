using UnityEngine;

public class Character : MonoBehaviour
{
    public CharType CharType;
    public PlayerAnimation Animation;
    public Hurtbox Sword;
}

public enum CharType 
{
    Knight,
    Valkyrie
}