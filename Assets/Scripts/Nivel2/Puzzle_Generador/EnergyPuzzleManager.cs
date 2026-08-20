using UnityEngine;
using TMPro;

public class EnergyPuzzleManager : MonoBehaviour
{
    public LeverPuzzle lampLever;
    public LeverPuzzle fanLever;
    public LeverPuzzle ovenLever;

    public FactoryDoor door;
    public TMP_Text energyValueText;

    public GameObject energyUI;
    public bool puzzleSolved = false;


    void Start()
    {
        energyUI.SetActive(false);

        fanLever.controlledObject.SetState(false);
        lampLever.controlledObject.SetState(false);
        ovenLever.controlledObject.SetState(false);

        fanLever.canBeUsed = false;
        lampLever.canBeUsed = false;
        ovenLever.canBeUsed = false;
    }

    public void ActivatePuzzle()
    {
        lampLever.canBeUsed = true;
        fanLever.canBeUsed = true;
        ovenLever.canBeUsed = true;

        //  sincronizar estado inicial de palancas
        lampLever.isOn = true;
        fanLever.isOn = true;
        ovenLever.isOn = true;

        lampLever.UpdateVisual();
        fanLever.UpdateVisual();
        ovenLever.UpdateVisual();

        //  sincronizar objetos
        lampLever.controlledObject.SetState(true);
        fanLever.controlledObject.SetState(true);
        ovenLever.controlledObject.SetState(true);

        UpdateEnergyUI();
    }
    public void CheckSolution()
    {
        UpdateEnergyUI();

        bool lampOff = !lampLever.isOn;
        bool fanOn = fanLever.isOn;
        bool ovenOff = !ovenLever.isOn;

        if (lampOff && fanOn && ovenOff)
        {
            door.OpenDoor();
        }
        else
        {
            door.CloseDoor();
        }
    }

    public void UpdateEnergyUI()
    {
        int energy = 0;

        if (lampLever.isOn)
            energy += 25;

        if (fanLever.isOn)
            energy += 15;

        if (ovenLever.isOn)
            energy += 60;

        energyValueText.text = "Energia: " + energy + "%";
    }

    public void ShowEnergyUI()
    {
        energyUI.SetActive(true);
    }


}