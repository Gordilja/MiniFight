using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonEvents : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Lobby");
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
