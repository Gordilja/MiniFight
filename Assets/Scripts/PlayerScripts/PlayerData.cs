using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int prefabID;
    public int health;
    public int attack;

    public PlayerData() 
    {
        health = 100;
        attack = 5;
    }
}
