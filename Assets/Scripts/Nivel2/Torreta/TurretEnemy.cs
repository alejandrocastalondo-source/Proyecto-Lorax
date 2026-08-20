using System.Collections;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    public enum TurretColor
    {
        Red,
        Blue
    }

    [Header("Turret Color")]
    public TurretColor turretColor = TurretColor.Red;

    [Header("References")]
    public Animator animator;
    public Transform firePoint;
    public GameObject turretProjectilePrefab;
    public Transform player;

    [Header("Combat")]
    public int maxHealth = 7;
    private int currentHealth;

    [Header("Hit Flash")]
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.white;
    public float flashDuration = 0.1f;

    [Header("Drop")]
    public GameObject batteryPrefab;
    public Transform dropPoint;

    private Color originalColor;

    public float shootCooldown = 2f;

    private bool playerDetected = false;
    private bool isDestroyed = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (isDestroyed) return;
        if (!playerDetected) return;

        LookAtPlayer();
    }

    void OnBecameVisible()
    {
        if (!playerDetected)
        {
            playerDetected = true;
            StartCoroutine(ShootLoop());
        }
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        // Sprite original mirando a la izquierda
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    IEnumerator ShootLoop()
    {
        while (!isDestroyed)
        {
            animator.SetTrigger("Reload");

            yield return new WaitForSeconds(1f);

            Shoot();

            yield return new WaitForSeconds(shootCooldown);
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(
            turretProjectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        float dir = transform.localScale.x > 0 ? -1f : 1f;

        TurretProjectile projectile = proj.GetComponent<TurretProjectile>();
        projectile.Launch(dir);
    }

    public void TakeDamage(bool isBlueProjectile)
    {
        if (isDestroyed) return;

        bool shouldTakeDamage = false;

        // Torreta roja → recibe azul
        if (turretColor == TurretColor.Red && isBlueProjectile)
            shouldTakeDamage = true;

        // Torreta azul → recibe rojo
        if (turretColor == TurretColor.Blue && !isBlueProjectile)
            shouldTakeDamage = true;

        if (!shouldTakeDamage)
            return;

        StartCoroutine(HitFlash());

        currentHealth--;

        Debug.Log("Vida torreta: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(DestroySequence());
        }
    }

    IEnumerator DestroySequence()
    {
        isDestroyed = true;

        animator.SetTrigger("Destroy");

        yield return new WaitForSeconds(1f);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (batteryPrefab != null)
        {
            Instantiate(
                batteryPrefab,
                dropPoint != null ? dropPoint.position : transform.position,
                Quaternion.identity
            );
        }
    }

    IEnumerator HitFlash()
    {
        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
    }
}