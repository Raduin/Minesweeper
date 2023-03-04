using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Mouse mouse;

    public void BeginnerPressed()
    {
        game.width = 8;
        game.height = 8;
        game.mineCount = 10;
        Flags();
        game.NewGame();
    }

    public void Intermediate()
    {
        game.width = 16;
        game.height = 16;
        game.mineCount = 40;
        Flags();
        game.NewGame();
    }

    public void Expert()
    {
        game.width = 30;
        game.height = 16;
        game.mineCount = 99;
        Flags();
        game.NewGame();
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Flags()
    {
        game.enabled = true;
        mouse.enabled = true;
        canvas.enabled = false;
    }
}
