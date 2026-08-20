using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform targetPoint;
    public float moveSpeed = 2f;

    private bool shouldMove = false;
    private Vector3 destination;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        destination = startPosition;
    }

    void Update()
    {
        if (!shouldMove) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            transform.position = destination;
            shouldMove = false;
        }
    }

    public void TogglePlatform(bool activate)
    {
        destination = activate ? targetPoint.position : startPosition;
        shouldMove = true;
    }
}