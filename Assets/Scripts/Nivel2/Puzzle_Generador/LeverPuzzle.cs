using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
    public bool isOn = true;

    public SpriteRenderer spriteRenderer;
    public Sprite onSprite;
    public Sprite offSprite;

    public PuzzleObject controlledObject;
    public EnergyPuzzleManager puzzleManager;
    public bool canBeUsed = false;

    void Start()
    {
        isOn = false;
        UpdateVisual();
    }

    public void Toggle()
    {
        if (!canBeUsed) return;

        isOn = !isOn;

        controlledObject.SetState(isOn);
        UpdateVisual();

        puzzleManager.CheckSolution();
    }

    public void UpdateVisual()
    {
        spriteRenderer.sprite = isOn ? onSprite : offSprite;
    }
}