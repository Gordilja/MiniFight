using UnityEngine;

public class Character : MonoBehaviour
{
    public CharType CharType;
    public PlayerAnimation Animation;
    public SwordCollider Sword;
}

public enum CharType 
{
    Knight,
    Valkyrie
}