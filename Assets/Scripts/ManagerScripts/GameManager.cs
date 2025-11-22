using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;

    public bool StartGame = false;
    public GameState State;
    public GameObject EnemyPlayer;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public enum GameState
{
    Joining,
    Start,
    End
}