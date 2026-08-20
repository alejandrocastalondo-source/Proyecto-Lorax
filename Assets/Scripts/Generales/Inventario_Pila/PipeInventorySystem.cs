using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeInventorySystem : MonoBehaviour
{
    public static PipeInventorySystem Instance;
    public GameObject inventoryUI;

    [Header("UI")]
    public List<Image> slotIcons;


    //  ESTRUCTURA DE DATOS: PILA (Stack<Sprite>)
    private Stack<Sprite> itemStack =
        new Stack<Sprite>();

    private bool collectedAllItems = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddItem(Sprite itemSprite)
    {
        if (itemStack.Count >= 3)
            return;

        itemStack.Push(itemSprite); //  Push: añadir al tope

        if (itemStack.Count >= 3)
        {
            collectedAllItems = true;
        }

        UpdateUI();

        Debug.Log("Item agregado al inventario tipo pila.");
    }

    
    public bool HasCollectedAllItems()
    {
        return collectedAllItems;
    }

    public Sprite UseTopItem()
    {
        if (itemStack.Count <= 0)
            return null;

        Sprite item =
            itemStack.Pop();

        UpdateUI();

        return item;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slotIcons.Count; i++)
        {
            slotIcons[i].sprite = null;
            slotIcons[i].color =
                new Color(1, 1, 1, 0);
        }

        Sprite[] items =
            itemStack.ToArray();

        for (int i = 0; i < items.Length; i++)
        {
            slotIcons[i].sprite = items[i];
            slotIcons[i].color = Color.white;
        }
    }

    public void HideInventory()
    {
        if (inventoryUI != null)
            inventoryUI.SetActive(false);
    }
}