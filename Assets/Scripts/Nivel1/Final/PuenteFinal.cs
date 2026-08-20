using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuenteFinal : MonoBehaviour
{
    public SpriteRenderer puenteSprite;

    public Sprite puente1;
    public Sprite puente2;
    public Sprite puenteAbajo;

    public BoxCollider2D puenteCollider;

    private int tarrosCompletados = 0;

    void Start()
    {
        puenteCollider.enabled = false;
    }

    public void TarroCompletado()
    {
        tarrosCompletados++;

        if (tarrosCompletados == 1)
        {
            puenteSprite.sprite = puente1;
        }
        else if (tarrosCompletados == 2)
        {
            puenteSprite.sprite = puente2;
        }
        else if (tarrosCompletados == 3)
        {
            puenteSprite.sprite = puenteAbajo;

            puenteCollider.enabled = true;
        }
    }
}