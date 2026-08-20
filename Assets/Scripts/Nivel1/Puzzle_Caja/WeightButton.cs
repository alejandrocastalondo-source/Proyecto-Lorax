using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightButton : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite pressedSprite;

    public BoxController compuerta;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = normalSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Caja"))
        {
            sr.sprite = pressedSprite;
            compuerta.OpenBox();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Caja"))
        {
            sr.sprite = normalSprite;
        }
    }
}