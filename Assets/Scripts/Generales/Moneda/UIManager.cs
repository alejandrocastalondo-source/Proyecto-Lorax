using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI contaminationText;

    // sprite de la moneda
    public Image coinIcon;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        AnclarArribaIzquierda(coinText.rectTransform);
        AnclarArribaIzquierda(contaminationText.rectTransform);

        //  Mostrar valores por defecto al iniciar
        coinText.text = "X 0";
        contaminationText.text = "Contaminacion: 100%";
    }

    void AnclarArribaIzquierda(RectTransform rt)
    {
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
    }

    public void UpdateUI(int x, float cx)
    {
        coinText.text = $"X {x}";
        // cx va de 100 a 0, lo mostramos como porcentaje
        contaminationText.text = $"Contaminacion: {cx}%";
    }
}