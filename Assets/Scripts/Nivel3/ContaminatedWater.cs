using System.Collections;
using UnityEngine;

public class ContaminatedWater : MonoBehaviour
{
    public Animator animator;

    public Collider2D waterCollider;

    public MovingPlatform platform;

    public void StartPurification()
    {
        StartCoroutine(PurificationSequence());
    }

    IEnumerator PurificationSequence()
    {
        animator.SetTrigger("Purify");

        yield return new WaitForSeconds(2f);

        waterCollider.enabled = false;

        platform.TogglePlatform(true);
    }
}