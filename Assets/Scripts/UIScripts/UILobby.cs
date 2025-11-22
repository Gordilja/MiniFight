using UnityEngine;

public class UILobby : MonoBehaviour
{
    public static UILobby Instance;

    public GameObject TutorialPanel;
    public GameObject LobbyPanel;

    private void Awake()
    {
        Instance = this;
    }
}