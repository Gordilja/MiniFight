using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonEvents : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void StartGame()
    {
        UILobby.Instance.TutorialPanel.SetActive(false);
        UILobby.Instance.LobbyPanel.SetActive(false);
        GameManager.Instance.StartGame = true;
    }

    public void OptionPanel() 
    {
        // Open option panel
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
