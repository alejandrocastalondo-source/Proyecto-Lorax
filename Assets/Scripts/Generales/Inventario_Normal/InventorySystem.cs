using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<Image> slotBackgrounds;
    public List<Image> slotIcons;

    public Sprite slotNormal;
    public Sprite slotSelected;

    private int selectedIndex = 0;


    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        HandleSelection();

        if (selectedIndex >= slotIcons.Count)
            selectedIndex = 0;
    }

    void HandleSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedIndex = 4;

        UpdateUI();
    }

    public void AddItem(Sprite itemSprite)
    {
        for (int i = 0; i < slotIcons.Count; i++)
        {
            if (slotIcons[i].sprite == null)
            {
                slotIcons[i].sprite = itemSprite;
                slotIcons[i].color = Color.white;
                break;
               
        }
    }
}

    public Sprite UseSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= slotIcons.Count)
            return null;

        if (slotIcons[selectedIndex].sprite != null)
        {
            Sprite item = slotIcons[selectedIndex].sprite;
            slotIcons[selectedIndex].sprite = null;
            slotIcons[selectedIndex].color = new Color(1, 1, 1, 0);

            return item;
        }

        return null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slotBackgrounds.Count; i++)
        {
            if (i == selectedIndex)
                slotBackgrounds[i].sprite = slotSelected;
            else
                slotBackgrounds[i].sprite = slotNormal;

            if (slotIcons[i].sprite == null)
                slotIcons[i].color = new Color(1, 1, 1, 0);
            else
                slotIcons[i].color = Color.white;
        }
    }
}