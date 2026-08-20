using System.Collections;
using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    public Transform teleportPoint;

    public ScreenFader fader;

    public float waitTime = 3f;

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

        PlayerController controller = player.GetComponent<PlayerController>();

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // congelar jugador
        if (rb != null)
            rb.velocity = Vector2.zero;

        // pantalla negra
        yield return StartCoroutine(fader.FadeOut());

        // esperar
        yield return new WaitForSeconds(waitTime);

        // mover jugador
        player.position = teleportPoint.position;

        CameraFollow cam = FindFirstObjectByType<CameraFollow>();

        if (cam != null)
        {
            cam.SnapToPlayer();
        }

        // fade in
        float alpha = 1;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fader.fadeSpeed;

            fader.fadeImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        isTeleporting = false;
    }
}