using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float leftLimit = -3f;
    public float rightLimit = 3f;

    public float smoothSpeed = 5f;

    private float originalY;

    void Start()
    {
        originalY = transform.position.y;
    }

    void LateUpdate()
    {
        float cameraX = transform.position.x;
        float playerX = player.position.x;

        float targetX = cameraX;

        if (playerX > cameraX + rightLimit)
            targetX = playerX - rightLimit;

        if (playerX < cameraX + leftLimit)
            targetX = playerX - leftLimit;

        Vector3 targetPosition = new Vector3(
            targetX,
            originalY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    public void SnapToPlayer()
    {
        transform.position = new Vector3(
            player.position.x,
            originalY,
            transform.position.z
        );
    }
}