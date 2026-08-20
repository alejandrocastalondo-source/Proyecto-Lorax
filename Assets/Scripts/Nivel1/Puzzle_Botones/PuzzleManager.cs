using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PuzzleManager : MonoBehaviour
{
    public int[] correctOrder = { 1, 3, 2 };

    private List<int> playerOrder = new List<int>();

    private PuzzleButton[] buttons;

    void Start()
    {
        buttons = FindObjectsByType<PuzzleButton>(FindObjectsSortMode.None);
    }

    public void RegisterButton(int id)
    {
        playerOrder.Add(id);

        if (playerOrder.Count == correctOrder.Length)
        {
            CheckOrder();
        }
    }

    void CheckOrder()
    {
        bool correct = true;

        for (int i = 0; i < correctOrder.Length; i++)
        {
            if (playerOrder[i] != correctOrder[i])
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            Debug.Log("Puzzle correcto");
            PuzzleCompleted();
        }
        else
        {
            Debug.Log("Puzzle incorrecto");
            ResetPuzzle();
        }
    }

    void ResetPuzzle()
    {
        playerOrder.Clear();

        foreach (PuzzleButton button in buttons)
        {
            button.ResetButton();
        }
    }

    public BoxController box;

    void PuzzleCompleted()
    {
        Debug.Log("Puzzle correcto");
        box.OpenBox();
    }
}