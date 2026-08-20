using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public enum ProjectileColor
    {
        Red,
        Blue
    }

    [Header("Projectile Color")]
    public ProjectileColor projectileColor = ProjectileColor.Red;

    public float speed = 12f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(float dir)
    {
        rb.velocity = new Vector2(dir * speed, 0f);

        Vector3 scale = transform.localScale;

        if (dir < 0)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // golpea jugador
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            bool shouldKill = false;

            // Proyectil azul → mata rojo o normal
            if (projectileColor == ProjectileColor.Blue)
            {
                if (player.currentState != PowerState.Blue)
                    shouldKill = true;
            }

            // Proyectil rojo → mata azul o normal
            if (projectileColor == ProjectileColor.Red)
            {
                if (player.currentState != PowerState.Red)
                    shouldKill = true;
            }

            if (shouldKill)
            {
                player.SendMessage("Die");
            }

            Destroy(gameObject);
            return;
        }

        // colisión con proyectil del jugador
        Projectile projectile = other.GetComponent<Projectile>();

        if (projectile != null)
        {
            Destroy(projectile.gameObject);
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}