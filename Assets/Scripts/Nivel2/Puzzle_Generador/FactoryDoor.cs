using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FactoryDoor : MonoBehaviour
{
    public Animator animator;

    [Header("Cambio de nivel")]
    public string nextSceneName;
    public ScreenFader screenFader;

    private bool opened = false;

    void Start()
    {
        animator.SetBool("Open", false);
    }

    public void OpenDoor()
    {
        if (opened) return;

        opened = true;
        animator.SetBool("Open", true);
    }

    public void CloseDoor()
    {
        if (!opened) return;

        opened = false;
        animator.SetBool("Open", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!opened) return; 

        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("NextScene", nextSceneName);
            StartCoroutine(GoToNextLevel());
        }
    }

    IEnumerator GoToNextLevel()
    {
        yield return StartCoroutine(screenFader.FadeOut());
        SceneManager.LoadScene("LivesScreen");
    }
}