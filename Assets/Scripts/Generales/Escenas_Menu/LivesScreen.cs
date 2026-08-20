using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public class LivesScreen : MonoBehaviour
{
    [Header("UI")]
    public Image lifeIcon;       
    public TextMeshProUGUI livesText; 
   

    public string nextSceneName;

    void Start()
    {
        int currentLives = GameManager.Instance != null ? GameManager.Instance.lives : 3;

        livesText.text = "x " + currentLives;

        // Si nextSceneName viene vacío, busca si viene de una muerte
        if (PlayerPrefs.HasKey("NextScene"))
        {
            nextSceneName = PlayerPrefs.GetString("NextScene");
            PlayerPrefs.DeleteKey("NextScene");
        }

        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(nextSceneName);
    }
}