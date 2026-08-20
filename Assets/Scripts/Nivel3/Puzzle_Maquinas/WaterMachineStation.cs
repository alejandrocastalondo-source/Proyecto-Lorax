using UnityEngine;

public class WaterMachineStation : MonoBehaviour
{
    [Header("Tipo")]
    public WaterMachineType machineType;

    [Header("Sprites")]
    public SpriteRenderer machineRenderer;
    public SpriteRenderer buttonRenderer;

    public Sprite machineOffSprite;
    public Sprite machineOnSprite;

    public Sprite buttonOffSprite;
    public Sprite buttonOnSprite;

    private bool activated = false;

    void Start()
    {
        TurnOff();
    }

    // El jugador entra en el trigger de la máquina

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (activated)
            return;

        ActivateStation();
    }

    void ActivateStation()
    {
        activated = true;

        machineRenderer.sprite = machineOnSprite;
        buttonRenderer.sprite = buttonOnSprite;

        Debug.Log(machineType + " ACTIVADO");

        WaterMachineQueueManager.Instance.EnqueueStation(this);
    }

    public void TurnOff()
    {
        activated = false;

        machineRenderer.sprite = machineOffSprite;
        buttonRenderer.sprite = buttonOffSprite;
    }

    public WaterMachineType GetMachineType()
    {
        return machineType;
    }
}