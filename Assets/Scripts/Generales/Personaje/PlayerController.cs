using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PowerState
{
    Normal,
    Blue,
    Red
}

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public float speed = 5f;
    public float jumpForce = 30f;

    private Rigidbody2D rb;

    float groundedBufferTime = 0.1f;
    float groundedTimer = 0f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public float interactionRadius = 2f;
    public LayerMask interactableLayer;

    private Interactable currentInteractable;

    private bool isDead = false;
    private bool isTransforming = false;

    public Sprite deadSprite;
    private SpriteRenderer spriteRenderer;

    public ScreenFader fader;
    public string spawnPointName = "SpawnPoint";

    [Header("Power System")]
    public PowerState currentState = PowerState.Normal;
    public bool hasPowerUp = false;

    [Header("Shooting")]
    public GameObject blueProjectilePrefab;
    public GameObject redProjectilePrefab;
    public Transform shootPoint;
    public float shootCooldown = 1f; // ajustable desde el Inspector
    private float shootTimer = 0f;   // contador interno

    [Header("Ceiling Walk")]
    public bool isOnCeiling = false;
    public LayerMask ceilingLayer;
    public Transform ceilingCheck;
    public float ceilingCheckRadius = 0.3f;
    public float ceilingGravity = -3f;
    public float normalGravity = 3f;


    private MagneticZone currentMagneticZone;
    private int facingDir = 1;
    private Vector3 originalCeilingCheckLocalPos;

    public enum ShootMode
    {
        Normal,
        Fibonacci,
        Fourier
    }

    [Header("Shoot Mode")]
    private ShootMode currentShootMode = ShootMode.Normal;

    private int[] fibSequence = { 1, 1, 2, 3, 5 };
    private int fibIndex = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalCeilingCheckLocalPos = ceilingCheck.localPosition;

        rb.gravityScale = normalGravity;

        GameObject spawn = GameObject.Find(spawnPointName);
        if (spawn != null)
        {
            transform.position = new Vector3(
                spawn.transform.position.x,
                spawn.transform.position.y,
                0f
            );
        }

        animator.SetInteger("PowerState", (int)currentState);
    }

    void Update()
    {
        if (isDead) return;

        if (isTransforming)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput != 0)
            facingDir = moveInput > 0 ? 1 : -1;

        // Rotación (para ponerse boca abajo)
        if (isOnCeiling)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Escala solo para mirar izquierda/derecha
        float direction = facingDir;

        //  INVERTIR DIRECCIÓN SOLO EN EL TECHO
        if (isOnCeiling)
        {
            direction *= -1;
        }

        transform.localScale = new Vector3(direction * 0.7f, 0.7f, 1);

        bool groundedNow;

        if (!isOnCeiling)
        {
            groundedNow = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }
        else
        {
            groundedNow = Physics2D.OverlapCircle(
                ceilingCheck.position,
                ceilingCheckRadius,
                ceilingLayer
            );

            
            if (groundedNow == false)
            {
                groundedNow = true;
            }
        }

        animator.SetFloat("speed", Mathf.Abs(moveInput));
        animator.SetBool("isOnCeiling", isOnCeiling);

        if (groundedNow)
        {
            groundedTimer = groundedBufferTime;
        }
        else
        {
            groundedTimer -= Time.deltaTime;
        }

        bool stableGrounded = groundedTimer > 0;

        animator.SetBool("isGrounded", stableGrounded);

        if (isOnCeiling && groundedNow)
        {
            animator.SetBool("isGrounded", true);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            bool canJump = isOnCeiling ? true : groundedNow;

            if (canJump)
            {
                rb.velocity = new Vector2(
                    rb.velocity.x,
                    isOnCeiling ? -jumpForce : jumpForce
                );
            }
        }

        if (hasPowerUp && Input.GetKeyDown(KeyCode.Q))
            ChangePower();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentState == PowerState.Red)
            {
                // Rojo: alterna entre Normal y Fibonacci
                currentShootMode = currentShootMode == ShootMode.Normal
                    ? ShootMode.Fibonacci
                    : ShootMode.Normal;
            }
            else if (currentState == PowerState.Blue)
            {
                // Azul: alterna entre Normal y Fourier
                currentShootMode = currentShootMode == ShootMode.Normal
                    ? ShootMode.Fourier
                    : ShootMode.Normal;
            }

            fibIndex = 0;
        }

        shootTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;
        }

        CheckCeilingMagnetism();
        DetectInteractable();

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
            currentInteractable.Interact();
    }



    void CheckCeilingMagnetism()
    {
        bool canUseMagnetism = false;

        if (currentMagneticZone != null)
        {
            canUseMagnetism = currentState == currentMagneticZone.requiredPower;
        }

        // Entrar al techo
        if (canUseMagnetism && !isOnCeiling)
        {
            isOnCeiling = true;
            rb.gravityScale = ceilingGravity;
        }

        // Salir del techo
        if ((!canUseMagnetism || currentMagneticZone == null) && isOnCeiling)
        {
            isOnCeiling = false;
            rb.gravityScale = normalGravity;
        }

        if (isOnCeiling)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -1f));
        }
    }

    void ChangePower()
    {
        switch (currentState)
        {
            case PowerState.Normal:
                currentState = PowerState.Red;
                break;
            case PowerState.Red:
                currentState = PowerState.Blue;
                break;
            case PowerState.Blue:
                currentState = PowerState.Normal;
                break;
        }

        animator.SetInteger("PowerState", (int)currentState);

        // resetear al modo Normal al cambiar de estado
        currentShootMode = ShootMode.Normal;
        fibIndex = 0;
    }

    void Shoot()
    {
        if (currentState == PowerState.Normal)
            return;

        GameObject projectilePrefab = null;

        if (currentState == PowerState.Blue)
            projectilePrefab = blueProjectilePrefab;
        else if (currentState == PowerState.Red)
            projectilePrefab = redProjectilePrefab;

        float direction = facingDir;

        int bulletsToShoot = 1;

        if (currentShootMode == ShootMode.Fibonacci)
        {
            bulletsToShoot = fibSequence[fibIndex];

            fibIndex++;

            if (fibIndex >= fibSequence.Length)
                fibIndex = 0;
        }

        for (int i = 0; i < bulletsToShoot; i++)
        {
            float spread = (i - bulletsToShoot / 2f) * 0.2f;

            Vector3 offset = new Vector3(0, spread, 0);

            if (currentShootMode == ShootMode.Fourier)
            {
                offset.y += 1.5f;

               
                offset.y += Mathf.Sin(Time.time * 10f) * 0.2f;
            }

            Vector3 spawnPosition = transform.position;

            // distancia horizontal delante del jugador
            spawnPosition += new Vector3(direction * 0.8f, 0f, 0f);

            // ajuste vertical dependiendo de si está en techo o no
            spawnPosition += new Vector3(0f, isOnCeiling ? -0.3f : 0.3f, 0f);

            // offset extra para disparos múltiples/Fourier
            spawnPosition += offset;

            GameObject projectile = Instantiate(
     projectilePrefab,
     spawnPosition,
     Quaternion.identity
 );

            // Ignorar colisión entre jugador y proyectil
            Collider2D playerCollider = GetComponent<Collider2D>();
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();

            if (playerCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, projectileCollider);
            }

            Vector3 scale = projectile.transform.localScale;

            if (direction < 0)
                scale.x = -Mathf.Abs(scale.x);
            else
                scale.x = Mathf.Abs(scale.x);

            projectile.transform.localScale = scale;

            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                if (currentShootMode == ShootMode.Fourier)
                    projectileScript.ActivateFourier(direction);
                else
                    projectileScript.Launch(direction);
            }
        }
    }

    public void ActivatePowerUp()
    {
        if (hasPowerUp) return;
        StartCoroutine(PowerUpSequence());
    }

    IEnumerator PowerUpSequence()
    {
        isTransforming = true;

        animator.SetTrigger("PowerUp");

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("PowerUp")
        );

        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length
        );

        hasPowerUp = true;
        currentState = PowerState.Normal;

        animator.SetInteger("PowerState", (int)currentState);

        isTransforming = false;
    }

    void DetectInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            interactionRadius,
            interactableLayer
        );

        Interactable closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            Interactable interactable = hit.GetComponent<Interactable>();

            if (interactable != null)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }
        }

        if (currentInteractable != closest)
        {
            if (currentInteractable != null)
                currentInteractable.Deselect();

            currentInteractable = closest;

            if (currentInteractable != null)
                currentInteractable.Select();
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(fader.FadeOut());

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife();

            if (GameManager.Instance.lives <= 0)
            {
                // Sin vidas: vuelve al menú
                GameManager.Instance.ResetLives();
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                // Aún tiene vidas: muestra pantalla de vidas antes de recargar
                // Guarda qué escena volver a cargar
                PlayerPrefs.SetString("NextScene", SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("LivesScreen");
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Die()
    {
        isDead = true;

        //  DESACTIVAR COLISIONES
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        rb.velocity = Vector2.zero;
        rb.gravityScale = normalGravity;

        animator.enabled = false;
        spriteRenderer.sprite = deadSprite;

        rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);

        StartCoroutine(Respawn());
    }

    public void Bounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.6f);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (ceilingCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TOCÓ: " + other.name);

        if (other.CompareTag("Water") && !isDead)
            Die();

        if (other.CompareTag("DeathZone") && !isDead)
            Die();

        MagneticZone zone = other.GetComponent<MagneticZone>();
        if (zone != null)
            currentMagneticZone = zone;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        MagneticZone zone = other.GetComponent<MagneticZone>();

        if (zone != null && currentMagneticZone == zone)
        {
            currentMagneticZone = null;
        }
    }


}