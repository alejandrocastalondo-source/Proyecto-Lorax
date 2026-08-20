using UnityEngine;

public class PipePickupItem : MonoBehaviour
{
    public Sprite itemSprite;

    public PuzzleItemType puzzleType;

    private PipeInventorySystem pipeInventory;

    void Start()
    {
        pipeInventory =
            FindFirstObjectByType<PipeInventorySystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pipeInventory.AddItem(itemSprite); // Llama Push internamente

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