using System.Collections;
using UnityEngine;

public class GoombaEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;

    //  LISTA ENLAZADA: puntero al nodo actual
    [Header("Patrol Linked List")]
    public PatrolNode currentNode;

    [Header("Death")]
    public float deathDuration = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Patrol();
    }

    void Patrol()
    {
        if (currentNode == null) return;

        Vector2 targetPosition = currentNode.transform.position;

        // Mover hacia el nodo actual
        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            targetPosition,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        // Dirección visual SIEMPRE basada en el nodo actual
        if (targetPosition.x > rb.position.x)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }

        // Cambio de nodo DESPUÉS del movimiento
        if (Vector2.SqrMagnitude(rb.position - targetPosition) < 0.0025f)
        {
            currentNode = currentNode.nextNode;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    Die();

                    if (player != null)
                        player.Bounce();

                    return;
                }
            }

            if (player != null)
                player.Die();
        }
    }

    void Die()
    {
        isDead = true;

        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        animator.SetTrigger("Die");

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(deathDuration);
        Destroy(gameObject);
    }
}