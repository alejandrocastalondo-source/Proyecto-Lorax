using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarroReciclaje : Interactable
{
    public Sprite objetoCorrecto;

    public SpriteRenderer spriteTarro;
    public Sprite spriteCerrado;

    public PuenteFinal puente;

    private bool cerrado = false;

    private InventorySystem inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<InventorySystem>();
    }

    void Update()
    {
        if (isSelected && !cerrado && Input.GetKeyDown(KeyCode.E))
        {
            IntentarDepositar();
        }
    }

    void IntentarDepositar()
    {
        //  TOMAR SOLO EL ITEM SELECCIONADO
        Sprite item = inventory.UseSelectedItem();

        if (item == objetoCorrecto)
        {
            spriteTarro.sprite = spriteCerrado;

            cerrado = true;
            canInteract = false;
            Deselect();

            puente.TarroCompletado();
        }
        else
        {
            Debug.Log("Ese objeto no va en este tarro");

            //  DEVOLVER EL ITEM SI ERA INCORRECTO
            if (item != null)
            {
                inventory.AddItem(item);
            }
        }
    }
}