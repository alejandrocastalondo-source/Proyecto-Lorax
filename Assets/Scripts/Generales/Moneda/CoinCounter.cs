using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter Instance;

    private int ecologicalActions = 0; // x

    public int EcologicalActions => ecologicalActions;

    private bool bonusLifeGranted = false;


    // C(x) = 100 - 5x
    public float GetContaminationLevel()
    {
        return 100f - 5f * ecologicalActions;
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCoin()
    {
        ecologicalActions++;
        ecologicalActions = Mathf.Clamp(ecologicalActions, 0, 20);
        UIManager.Instance.UpdateUI(ecologicalActions, GetContaminationLevel());

        // Solo suma la vida una vez por sesión de nivel
        if (ecologicalActions >= 20 && !bonusLifeGranted)
        {
            bonusLifeGranted = true;
            GameManager.Instance.GainLife();
        }
    }

}