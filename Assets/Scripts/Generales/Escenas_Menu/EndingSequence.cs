using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSequence : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;
    public Image slideImage;

    [Header("Slides")]
    public Sprite[] slides;

    [Header("Timing")]
    public float fadeDuration = 1.5f;
    public float holdTime = 2.5f;

    [Header("Disable")]
    public GameObject player;
    public GameObject hudCanvas;

    bool playing = false;

    void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0); // juego visible al inicio
        slideImage.gameObject.SetActive(false);   // nada de imágenes al inicio
    }

    public void StartEnding()
    {
        if (!playing)
            StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        playing = true;

        if (player) player.SetActive(false);
        if (hudCanvas) hudCanvas.SetActive(false);

        Time.timeScale = 1f;

        for (int i = 0; i < slides.Length; i++)
        {
            //  asegurar negro total antes de cambiar imagen
            yield return FadeToBlackInstant();

            slideImage.gameObject.SetActive(true);
            slideImage.sprite = slides[i];

            //  abrir ojos
            yield return FadeFromBlack();

            // mantener
            yield return new WaitForSeconds(holdTime);

            //  cerrar ojos
            yield return FadeToBlack();
        }

        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeFromBlack()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        fadeImage.color = Color.clear;
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        fadeImage.color = Color.black;
    }

    IEnumerator FadeToBlackInstant()
    {
        fadeImage.color = new Color(0, 0, 0, 0);
        yield return null;
    }
}