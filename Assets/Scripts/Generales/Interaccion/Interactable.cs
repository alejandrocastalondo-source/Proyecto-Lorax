using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject outline;
    public GameObject interactionIndicator;
    protected bool isSelected = false;
    public bool canInteract = true;

    public virtual void Interact()
    {
        Debug.Log("Interacción base");
    }

    public void Select()
    {
        if (!canInteract) return;

        isSelected = true;

        if (outline != null)
            outline.SetActive(true);

        if (interactionIndicator != null)
            interactionIndicator.SetActive(true);
    }

    public void Deselect()
    {
        isSelected = false;

        if (outline != null)
            outline.SetActive(false);

        if (interactionIndicator != null)
            interactionIndicator.SetActive(false);
    }


}