using System.Collections;
using UnityEngine;

public class WalkerEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;

    [Tooltip("Cuánto tiempo camina antes de voltearse")]
    public float turnTime = 2f;

    private float turnTimer;

    private int direction = 1;

    [Header("Energy")]
    public EnemyEnergy energyType;

    [Header("Drop")]
    public GameObject itemDropPrefab;

    [Header("Aura")]
    public GameObject redAura;
    public GameObject blueAura;

    [Header("Death")]
    public float destroyDelay = 1.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        turnTimer = turnTime;

        UpdateAura();
    }

    void Update()
    {
        if (isDead) return;

        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        turnTimer -= Time.deltaTime;

        if (turnTimer <= 0f)
        {
            Flip();
            turnTimer = turnTime;
        }
    }

    void Flip()
    {
        direction *= -1;

        Vector3 scale = transform.localScale;
        scale.x = direction > 0
            ? Mathf.Abs(scale.x)
            : -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    void UpdateAura()
    {
        if (redAura != null)
            redAura.SetActive(energyType == EnemyEnergy.Red);

        if (blueAura != null)
            blueAura.SetActive(energyType == EnemyEnergy.Blue);
    }

    public void TakeDamage(TerminalPuzzle.ProjectileType projectileType)
    {
        if (isDead) return;

        bool correctHit = false;

        // Rojo muere con azul
        if (
            energyType == EnemyEnergy.Red &&
            projectileType == TerminalPuzzle.ProjectileType.Blue
        )
        {
            correctHit = true;
        }

        // Azul muere con rojo
        if (
            energyType == EnemyEnergy.Blue &&
            projectileType == TerminalPuzzle.ProjectileType.Red
        )
        {
            correctHit = true;
        }

        if (correctHit)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Detener movimiento
        rb.velocity = Vector2.zero;

        //  IMPORTANTE
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator.SetTrigger("Die");

        // Desactivar collider para no matar al jugador después
        col.enabled = false;

        // Dropear objeto
        if (itemDropPrefab != null)
        {
            Instantiate(
                itemDropPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject, destroyDelay);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player =
                collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.Die();
            }
        }
    }
}