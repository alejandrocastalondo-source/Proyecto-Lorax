using UnityEngine;

public class OscillatingPlatform : MonoBehaviour
{
    public float moveDistance = 3f;
    public float speed = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.PingPong(Time.time * speed, moveDistance * 2) - moveDistance;

        transform.position = new Vector3(
            startPosition.x,
            startPosition.y + offsetY,
            startPosition.z
        );
    }
}