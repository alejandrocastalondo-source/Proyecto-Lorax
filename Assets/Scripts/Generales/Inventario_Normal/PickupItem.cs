using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Sprite itemSprite;

    public PuzzleItemType puzzleType;

    private InventorySystem inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<InventorySystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inventory.AddItem(itemSprite);

            if (puzzleType != PuzzleItemType.None)
            {
                PipePuzzleManager.Instance.RegisterPickup(
                    itemSprite,
                    puzzleType
                );
            }

            Destroy(gameObject);
        }
    }
}