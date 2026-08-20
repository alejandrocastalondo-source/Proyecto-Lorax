using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 8f;

    [Header("Projectile Color")]
    public BossColorState projectileColor;


    void Start()
    {

        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController =
            other.GetComponent<PlayerController>();

        // SI GOLPEA AL JUGADOR
        if (playerController != null)
        {
            bool shouldDamage = false;

            // Bala roja daña azul o normal
            if (projectileColor == BossColorState.Red)
            {
                shouldDamage =
                    playerController.currentState == PowerState.Blue ||
                    playerController.currentState == PowerState.Normal;
            }

            // Bala azul daña rojo o normal
            if (projectileColor == BossColorState.Blue)
            {
                shouldDamage =
                    playerController.currentState == PowerState.Red ||
                    playerController.currentState == PowerState.Normal;
            }

            if (shouldDamage)
            {
                playerController.Die();
            }

            Destroy(gameObject);
            return;
        }

        // SI CHOCA CON CUALQUIER OTRA COSA
        Destroy(gameObject);
    }
}