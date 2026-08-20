using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndLevel : MonoBehaviour
{
    public string nextSceneName;
    public ScreenFader screenFader; //  Arrastra el FadePanel aquí

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("NextScene", nextSceneName);
            StartCoroutine(IrAlSiguienteNivel());
        }
    }

    IEnumerator IrAlSiguienteNivel()
    {
        yield return StartCoroutine(screenFader.FadeOut()); // Primero el fade
        SceneManager.LoadScene("LivesScreen");              // Luego la pantalla de vidas
    }
}