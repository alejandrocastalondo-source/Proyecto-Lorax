using UnityEngine;

public class BossLaser : MonoBehaviour
{
    public BossColorState laserColor;

    private bool hasDamaged = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (hasDamaged) return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player != null)
        {
            bool shouldDamage = false;

            // LASER ROJO
            if (laserColor == BossColorState.Red)
            {
                shouldDamage =
                    player.currentState == PowerState.Blue ||
                    player.currentState == PowerState.Normal;
            }

            // LASER AZUL
            if (laserColor == BossColorState.Blue)
            {
                shouldDamage =
                    player.currentState == PowerState.Red ||
                    player.currentState == PowerState.Normal;
            }

            if (shouldDamage)
            {
                hasDamaged = true;
                player.Die();
            }
        }
    }
}