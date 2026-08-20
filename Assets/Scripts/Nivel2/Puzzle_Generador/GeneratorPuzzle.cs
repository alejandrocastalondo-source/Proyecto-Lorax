using UnityEngine;

public class GeneratorPuzzle : Interactable
{
    public SpriteRenderer spriteRenderer;
    public Sprite offSprite;
    public Sprite onSprite;

    public EnergyPuzzleManager puzzleManager;
    private bool activated = false;
    private InventorySystem inventory;

    

    void Start()
    {
        inventory = FindFirstObjectByType<InventorySystem>();
        spriteRenderer.sprite = offSprite;
    }

    public override void Interact()
    {
        if (activated) return;

        Sprite item = inventory.UseSelectedItem();

        if (item != null)
        {
            activated = true;
            spriteRenderer.sprite = onSprite;

            puzzleManager.ActivatePuzzle();

            Deselect(); 
            canInteract = false;
        }
    }
}