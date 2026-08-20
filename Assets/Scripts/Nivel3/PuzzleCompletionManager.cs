using UnityEngine;

public class PuzzleCompletionManager : MonoBehaviour
{
    public static PuzzleCompletionManager Instance;

    private bool pipePuzzleComplete = false;
    private bool machinePuzzleComplete = false;

    public ContaminatedWater water;

    void Awake()
    {
        Instance = this;
    }

    public void MarkPipePuzzleComplete()
    {
        pipePuzzleComplete = true;
        CheckFinal();
    }

    public void MarkMachinePuzzleComplete()
    {
        machinePuzzleComplete = true;
        CheckFinal();
    }

    void CheckFinal()
    {
        if (pipePuzzleComplete && machinePuzzleComplete)
        {
            Debug.Log("TODOS LOS PUZZLES COMPLETOS");

            water.StartPurification();
        }
    }
}