using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Canvas canvasMenu;
    [SerializeField] private Mouse mouse;

    public void BeginnerPressed()
    {
        game.width = 8;
        game.height = 8;
        game.mineCount = 10;
        Flags();
        game.beginnerOn = true;
        game.NewGame();
    }

    public void Intermediate()
    {
        game.width = 16;
        game.height = 16;
        game.mineCount = 40;
        Flags();
        game.intermediateOn = true;
        game.NewGame();
    }

    public void Expert()
    {
        game.width = 30;
        game.height = 16;
        game.mineCount = 99;
        Flags();
        game.expertOn = true;
        game.NewGame();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Flags()
    {
        game.enabled = true;
        mouse.enabled = true;
        canvasMenu.enabled = false;
    }
}
