using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    public int buttonID;

    public Sprite normalSprite;
    public Sprite pressedSprite;

    private SpriteRenderer sr;
    private PuzzleManager puzzleManager;

    private bool isPressed = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        puzzleManager = FindFirstObjectByType<PuzzleManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPressed)
        {
            PressButton();
        }
    }

    void PressButton()
    {
        isPressed = true;
        sr.sprite = pressedSprite;

        puzzleManager.RegisterButton(buttonID);
    }

    public void ResetButton()
    {
        isPressed = false;
        sr.sprite = normalSprite;
    }
}