using System.Collections.Generic;
using UnityEngine;

public class BalancePlatform : MonoBehaviour
{
    [Header("Balance Settings")]
    public float maxAngle = 45f;

    [Header("Physics Feel")]
    public float torqueForce = 40f;
    public float recoveryForce = 8f;
    public float angularDamping = 0.98f;

    private List<BalanceWeight> weights =
        new List<BalanceWeight>();

    private float angularVelocity;
    private float currentAngle;

    void Update()
    {
        float totalForce = 0f;

        // CALCULAR FUERZA TOTAL
        foreach (BalanceWeight weightObject in weights)
        {
            if (weightObject == null) continue;

            float localX = transform
                .InverseTransformPoint(
                    weightObject.transform.position
                ).x;

            // FUNCIÓN:
            // F(x,y) = -x * y
            float force =
                -localX * weightObject.weight;

            totalForce += force;
        }

        // Aplicar torque
        angularVelocity +=
            totalForce *
            torqueForce *
            Time.deltaTime;

        // Recuperación
        angularVelocity -=
            currentAngle *
            recoveryForce *
            Time.deltaTime;

        // Fricción
        angularVelocity *= angularDamping;

        // Actualizar ángulo
        currentAngle +=
            angularVelocity *
            Time.deltaTime;

        // Limitar
        currentAngle = Mathf.Clamp(
            currentAngle,
            -maxAngle,
            maxAngle
        );

        // Rotar
        transform.rotation =
            Quaternion.Euler(
                0,
                0,
                currentAngle
            );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BalanceWeight weight =
            collision.gameObject.GetComponent<BalanceWeight>();

        if (weight != null)
        {
            if (!weights.Contains(weight))
            {
                weights.Add(weight);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        BalanceWeight weight =
            collision.gameObject.GetComponent<BalanceWeight>();

        if (weight != null)
        {
            if (weights.Contains(weight))
            {
                weights.Remove(weight);
            }
        }
    }
}