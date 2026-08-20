using UnityEngine;

public class PipePuzzleManager : MonoBehaviour
{
    public static PipePuzzleManager Instance;

    [Header("Configuracion Puzzle")]
    public int requiredItems = 3;

    private int placedItems = 0;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterPickup(
        Sprite sprite,
        PuzzleItemType type
    )
    {
        Debug.Log(
            "Objeto registrado: " + type
        );
    }

    public bool CheckItem(Sprite usedSprite)
    {
        placedItems++;

        Debug.Log("Objeto colocado correctamente.");

        return true;
    }

    public bool IsCompleted()
    {
        return placedItems >= requiredItems;
    }
}