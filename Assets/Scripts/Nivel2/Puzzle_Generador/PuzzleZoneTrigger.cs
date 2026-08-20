using UnityEngine;

public class PuzzleZoneTrigger : MonoBehaviour
{
    public EnergyPuzzleManager puzzleManager;
    private bool activated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            puzzleManager.ShowEnergyUI();
        }
    }
}