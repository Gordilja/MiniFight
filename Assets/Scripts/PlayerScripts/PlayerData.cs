using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string userName;
    public int prefabID;
    public int health;
    public int attack;
    public CharType type;

    public PlayerData() 
    {
        health = 100;
        attack = 5;
        type = CharType.Knight;
    }
}
