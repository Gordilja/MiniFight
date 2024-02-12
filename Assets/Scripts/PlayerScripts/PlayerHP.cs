using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public PlayerController _PlayerController;
    public int _PlayerHealth;

    // OnSpawnPlayer later
    private void Start()
    {
        _PlayerHealth = _PlayerController._PlayerData.health;
    }

    public void DealDmg(int damageDealt) 
    {
         _PlayerHealth -= damageDealt;
    }
}
