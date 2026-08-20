using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIndicator : MonoBehaviour
{
    public float speed = 100f;
    public float floatSpeed = 2f;
    public float floatAmount = 0.2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Rotar
        transform.Rotate(0, 0, speed * Time.deltaTime);

        // Flotar
        float y = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.localPosition = startPos + new Vector3(0, y, 0);
    }
}
