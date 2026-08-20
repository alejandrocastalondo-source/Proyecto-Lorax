using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int lives = 3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoseLife()
    {
        lives--;
    }

    public void ResetLives()
    {
        lives = 3;
    }

    public void GainLife()
    {
        lives++;
    }
}