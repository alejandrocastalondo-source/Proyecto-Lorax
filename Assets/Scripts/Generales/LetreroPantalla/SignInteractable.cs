using UnityEngine;
using TMPro;

public class SignInteractable : Interactable
{
    [Header("UI")]
    public GameObject signCanvas;

    public TextMeshProUGUI signText;

    [TextArea(4, 10)]
    public string message;

    [Header("Ending")]
    public bool triggersEnding = false;

    public EndingSequence endingSequence;

    private bool isReading = false;

    public override void Interact()
    {
        if (!isReading)
        {
            OpenSign();
        }
        else
        {
            CloseSign();
        }
    }

    void OpenSign()
    {
        isReading = true;

        Deselect();

        signCanvas.SetActive(true);

        signText.text = message;

        Time.timeScale = 0f;
    }

    void CloseSign()
    {
        isReading = false;

        signCanvas.SetActive(false);

        Time.timeScale = 1f;

        
        if (triggersEnding && endingSequence != null)
        {
            endingSequence.StartEnding();
        }
    }

    void Update()
    {
        if (isReading && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSign();
        }
    }
}