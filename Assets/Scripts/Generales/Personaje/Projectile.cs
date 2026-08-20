using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;

    [Header("Projectile Type")]
    public TerminalPuzzle.ProjectileType projectileType;

    private bool isFourier = false;
    private float time;
    private float direction;
    private Vector3 startPos;
    public float fourierSpeed = 3f;

    public float frequency = 3f;
    public float amplitude = 2.5f;
    public int harmonics = 6;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isFourier)
        {
            time += Time.deltaTime * frequency;

            float y = 0f;

            for (int n = 1; n <= harmonics; n++)
            {
                y += (1f / n) * Mathf.Sin(n * time);
            }

            float x = fourierSpeed * time * direction;

            transform.position = startPos + new Vector3(x, y * amplitude, 0);
        }
    }

    public void Launch(float dir)
    {
        if (isFourier) return;

        rb.velocity = new Vector2(dir * speed, 0f);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        // IGNORAR OBJETOS QUE NO DEBEN DESTRUIR PROYECTIL

        if (
       other.CompareTag("MagneticZone") ||
       other.CompareTag("IgnoreProjectile")
           )
        {
            return;
        }

        // ACTIVAR TERMINALES
        TerminalPuzzle terminal = other.GetComponent<TerminalPuzzle>();

        if (terminal != null)
        {
            terminal.TryActivate(projectileType);
            Destroy(gameObject);
            return;
        }

        WalkerEnemy enemy = other.GetComponent<WalkerEnemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(projectileType);

            Destroy(gameObject);
            return;
        }

        // DAÑO A TORRETAS
        TurretEnemy turret = other.GetComponent<TurretEnemy>();

        if (turret != null)
        {
            bool isBlueProjectile = projectileType == TerminalPuzzle.ProjectileType.Blue;
            turret.TakeDamage(isBlueProjectile);

            Destroy(gameObject);
            return;
        }

        BossEnemy boss = other.GetComponent<BossEnemy>();

        if (boss != null)
        {
            boss.TakeDamage(projectileType);
            Destroy(gameObject);
            return;
        }

        // ACTIVAR PALANCAS DEL PUZZLE
        LeverPuzzle lever = other.GetComponent<LeverPuzzle>();

        if (lever != null)
        {
            if (projectileType == TerminalPuzzle.ProjectileType.Blue)
            {
                lever.Toggle();
            }

            Destroy(gameObject);
            return;
        }

        // Fourier ignora otros triggers
        if (isFourier) return;

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFourier) return;

        Destroy(gameObject);
    }

    public void ActivateFourier(float dir)
    {
        isFourier = true;
        direction = dir;
        startPos = transform.position;
    }
}