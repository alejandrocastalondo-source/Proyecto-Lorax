using UnityEngine;

public class LockedDoor : Interactable
{
    [Header("Key")]
    public Sprite requiredKey;

    [Header("Door Sprites")]
    public Sprite closedSprite;
    public Sprite openedSprite;

    [Header("Teleport Trigger")]
    public GameObject enterTrigger;

    private InventorySystem inventory;

    private SpriteRenderer sr;

    private bool isOpened = false;

    void Start()
    {
        inventory = FindFirstObjectByType<InventorySystem>();

        sr = GetComponent<SpriteRenderer>();

        sr.sprite = closedSprite;

        if (enterTrigger != null)
            enterTrigger.SetActive(false);
    }

    public override void Interact()
    {
        if (isOpened)
            return;

        Sprite usedItem = inventory.UseSelectedItem();

        // no había item seleccionado
        if (usedItem == null)
            return;

        // item incorrecto
        if (usedItem != requiredKey)
        {
            Debug.Log("Llave incorrecta");
            return;
        }

        OpenDoor();
    }

    void OpenDoor()
    {
        isOpened = true;

        canInteract = false;

        if (outline != null)
            outline.SetActive(false);

        if (interactionIndicator != null)
            interactionIndicator.SetActive(false);

        sr.sprite = openedSprite;

        // DESACTIVAR COLLIDER
        GetComponent<Collider2D>().enabled = false;

        if (enterTrigger != null)
            enterTrigger.SetActive(true);

        Debug.Log("Puerta abierta");
    }
}