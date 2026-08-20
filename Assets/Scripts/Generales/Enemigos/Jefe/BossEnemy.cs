using System.Collections;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("States")]
    public BossState state = BossState.Idle;
    public BossColorState colorState = BossColorState.Red;

    [Header("Combat")]
    public int hitCount = 0;
    public int hitsPerPhase = 5;
    public int totalHitsToDie = 20;

    [Header("Detection")]
    public Transform player;
    public float detectionRange = 8f;

    [Header("Shooting")]
    public GameObject redProjectile;
    public GameObject blueProjectile;
    public Transform shootPoint;
   

    [Header("Laser")]
    public GameObject laserPrefab;
    public Transform laserPoint;
    private GameObject currentLaser;




    private Animator animator;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Aura")]
    public GameObject redAura;
    public GameObject blueAura;




    void Start()
    {
        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        originalScale = transform.localScale;
        originalPosition = transform.position;

        state = BossState.Idle;

        UpdateAura();
    }

    void UpdateBossVisual()
    {
        if (state == BossState.Reloading || state == BossState.Laser)
        {
            transform.localScale = originalScale * 1.3f;
        }
        else if (state == BossState.Defeated)
        {
            transform.localScale = originalScale * 1.5f;

            transform.position += new Vector3(0f, -0.5f, 0f);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
    void UpdateAura()
    {
        redAura.SetActive(colorState == BossColorState.Red);

        blueAura.SetActive(colorState == BossColorState.Blue);
    }


    public void StartLaser()
    {
        currentLaser =
            Instantiate(
                laserPrefab,
                laserPoint.position,
                Quaternion.identity,
                transform
            );

        BossLaser laserScript =
            currentLaser.GetComponent<BossLaser>();

        laserScript.laserColor = colorState;
    }

    public void StopLaser()
    {
        if (currentLaser != null)
        {
            Destroy(currentLaser);
        }
    }

    void Update()
    {
        if (state == BossState.Defeated) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (state == BossState.Idle && distance <= detectionRange)
        {
            StartShooting();
        }
    }

    void StartShooting()
    {
        if (state != BossState.Idle) return;

        state = BossState.Shooting;

       

        animator.SetBool("isPlayerNear", true);
    }


    public void ShootProjectile()
    {
        if (state != BossState.Shooting) return;

        GameObject prefab = (colorState == BossColorState.Red)
            ? redProjectile
            : blueProjectile;

        GameObject bullet =
            Instantiate(prefab, shootPoint.position, Quaternion.identity);

        Vector2 direction =
            (player.position - shootPoint.position).normalized;

        bullet.transform.right = direction;
    }
    public void TakeDamage(TerminalPuzzle.ProjectileType type)
    {
        if (state == BossState.Defeated) return;

        // lógica de daño por color
        bool correctHit =
            (colorState == BossColorState.Red && type == TerminalPuzzle.ProjectileType.Blue) ||
            (colorState == BossColorState.Blue && type == TerminalPuzzle.ProjectileType.Red);

        if (!correctHit) return;

        hitCount++;

        StartCoroutine(DamageFlash());

        if (hitCount >= totalHitsToDie)
        {
            StartCoroutine(Die());
            return;
        }

        if (hitCount % hitsPerPhase == 0)
        {
            StartCoroutine(ReloadAndLaser());
        }
    }

    IEnumerator ReloadAndLaser()
    {
        state = BossState.Reloading;
        UpdateBossVisual();

        animator.SetTrigger("Reload");

        yield return new WaitForSeconds(1f);

        state = BossState.Laser;
        UpdateBossVisual();

        animator.SetTrigger("Laser");

        yield return new WaitUntil(() => state == BossState.Shooting);

        UpdateBossVisual();
    }

    void ToggleColor()
    {
        colorState = (colorState == BossColorState.Red)
            ? BossColorState.Blue
            : BossColorState.Red;

        UpdateAura();
    }

   

    IEnumerator Die()
    {
        state = BossState.Defeated;

        UpdateBossVisual();

        animator.SetTrigger("Die");

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

    public void FinishLaser()
    {
        ToggleColor();

        state = BossState.Shooting;

        UpdateBossVisual();

        animator.SetTrigger("BackToShoot");
    }

    IEnumerator DamageFlash()
    {
        spriteRenderer.color = new Color(1f, 0.4f, 0.4f);

        yield return new WaitForSeconds(0.12f);

        spriteRenderer.color = originalColor;
    }
}