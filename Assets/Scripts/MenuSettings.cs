using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    [SerializeField] private Game game;

    private int currentWindowWidth;
    private int currentWindowHeight;
    private Vector2Int currentWindowPosition;
    [SerializeField] private GameObject settingsMenu;

    public void QuestionMark()
    {
        game.questionMark = !game.questionMark;
    }

    public void FullScreenSize()
    {
        if (!Screen.fullScreen)
        {
            currentWindowWidth = Screen.width;
            currentWindowHeight = Screen.height;
            currentWindowPosition = Screen.mainWindowPosition;

            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else
        {
            Screen.SetResolution(currentWindowWidth, currentWindowHeight, false);
            Screen.MoveMainWindowTo(Screen.mainWindowDisplayInfo, currentWindowPosition);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) settingsMenu.SetActive(false);
    }
}
