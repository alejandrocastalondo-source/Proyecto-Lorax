using System.Collections;
using UnityEngine;

public class WarpGate : MonoBehaviour
{
    public Transform teleportPoint;

    public WarpGate linkedGate;

    public ScreenFader fader;

    public float waitTime = 1f;

    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTeleporting)
            return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    IEnumerator TeleportPlayer(Transform player)
    {
        isTeleporting = true;

        // bloquear portal destino temporalmente
        linkedGate.isTeleporting = true;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.velocity = Vector2.zero;

        // fade negro
        yield return StartCoroutine(fader.FadeOut());

        yield return new WaitForSeconds(waitTime);

        // mover jugador
        player.position = teleportPoint.position;

        // mover cámara 
        CameraFollow cam = FindFirstObjectByType<CameraFollow>();

        if (cam != null)
        {
            cam.SnapToPlayer();
        }

        
        float alpha = 1;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fader.fadeSpeed;

            fader.fadeImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        // cooldown
        yield return new WaitForSeconds(0.5f);

        isTeleporting = false;
        linkedGate.isTeleporting = false;
    }
}