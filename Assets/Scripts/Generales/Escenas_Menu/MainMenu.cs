using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public void OnPlayButton()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetLives();

        
        SceneManager.LoadScene("LivesScreen");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}