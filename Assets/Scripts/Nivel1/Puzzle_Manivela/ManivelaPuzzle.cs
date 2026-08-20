using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManivelaPuzzle : Interactable
{
    public SpriteRenderer manivelaSprite;
    public Sprite spriteCerrada;
    public Sprite spriteGirada;

    public SpriteRenderer tuberia;
    public Sprite tuberiaCerrada;

    public GameObject objetoQueSale;

    private bool activado = false;

    void Start()
    {
        
        manivelaSprite.sprite = spriteCerrada;
    }

    //  ESTE MÉTODO LO LLAMA EL PLAYER CON LA TECLA E
    public override void Interact()
    {
        if (!activado)
        {
            activado = true;

            manivelaSprite.sprite = spriteGirada;
            tuberia.sprite = tuberiaCerrada;
            objetoQueSale.SetActive(true);

            //  QUITAR OUTLINE Y EVITAR QUE VUELVA
            canInteract = false;
            Deselect();
        }
    }
}