using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxController : MonoBehaviour
{
    public Sprite closedSprite;
    public Sprite halfOpenSprite;
    public Sprite openSprite;

    public GameObject objectToSpawn;
    public Transform spawnPoint;

    private SpriteRenderer sr;
    private bool opened = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedSprite;
    }

    public void OpenBox()
    {
        if (!opened)
        {
            StartCoroutine(OpenAnimation());
        }
    }

    IEnumerator OpenAnimation()
    {
        opened = true;

        sr.sprite = halfOpenSprite;
        yield return new WaitForSeconds(0.3f);

        sr.sprite = openSprite;
        yield return new WaitForSeconds(0.2f);

        Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);
    }
}