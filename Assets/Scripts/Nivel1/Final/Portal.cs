using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Portal : Interactable
{
    public Animator animator;
    public ScreenFader screenFader;

    
    public GameObject endLevelObject;

    private bool activado = false;
    private bool puedeTeletransportar = false;

    public override void Interact()
    {
        if (!activado)
        {
            Deselect();
            StartCoroutine(ActivarPortal());
        }
    }

    IEnumerator ActivarPortal()
    {
        activado = true;
        canInteract = false;

        animator.SetBool("Activado", true);

        yield return new WaitForSeconds(1f);

        puedeTeletransportar = true;

        //  activa el EndLevel cuando el portal se activa
        if (endLevelObject != null)
            endLevelObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && puedeTeletransportar)
        {
            StartCoroutine(CambiarNivel());
        }
    }

    IEnumerator CambiarNivel()
    {
        yield return StartCoroutine(screenFader.FadeOut());

        //  Guarda el siguiente nivel y pasa por LivesScreen
        PlayerPrefs.SetString("NextScene", "Nivel2"); 
        SceneManager.LoadScene("LivesScreen");
    }
}