
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public bool isPlayButton = true;
    private MainMenu mainMenu;

    void Start()
    {
        mainMenu = FindObjectOfType<MainMenu>();
    }

    void OnMouseDown()
    {
        if (isPlayButton)
            mainMenu.OnPlayButton();
        else
            mainMenu.OnQuitButton();
    }
}