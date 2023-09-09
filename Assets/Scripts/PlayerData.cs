using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public GameObject prefab;
    public int health;
    public float attack;

    public PlayerData() 
    {
        health = 100;
        attack = 5;
    }
}
