using UnityEngine;

public class PipePuzzle : Interactable
{
    [Header("Sprites")]
    public SpriteRenderer pipeRenderer;

    public Sprite state0;
    public Sprite state1;
    public Sprite state2;
    public Sprite state3;

    private int currentStep = 0;

    private PipeInventorySystem pipeInventory;

    [Header("Agua")]
    public ContaminatedWater water;

    void Start()
    {
        pipeInventory =
            FindFirstObjectByType<PipeInventorySystem>();

        pipeRenderer.sprite = state0;
    }

    public override void Interact()
    {
        // Solo permite usar objetos si ya se recolectaron los 3
        if (!pipeInventory.HasCollectedAllItems())
        {
            Debug.Log(
                "Debes recoger las 3 piezas primero."
            );

            return;
        }

        Sprite usedItem =
            pipeInventory.UseTopItem(); // Pop de la pila

        if (usedItem == null)
            return;

        bool correct =
            PipePuzzleManager.Instance.CheckItem(
                usedItem
            );

        if (correct)
        {
            currentStep++;

            UpdatePipeSprite();

            if (PipePuzzleManager.Instance.IsCompleted())
            {
                CompletePuzzle();
            }
        }
    }

    void UpdatePipeSprite()
    {
        switch (currentStep)
        {
            case 1:
                pipeRenderer.sprite = state1;
                break;

            case 2:
                pipeRenderer.sprite = state2;
                break;

            case 3:
                pipeRenderer.sprite = state3;
                break;
        }
    }

    void CompletePuzzle()
    {
        Debug.Log("Puzzle completado.");

        PuzzleCompletionManager.Instance.MarkPipePuzzleComplete();
        canInteract = false;

        Deselect();

        //  OCULTAR INVENTARIO DEL PUZZLE
        PipeInventorySystem.Instance.HideInventory();
    }
}