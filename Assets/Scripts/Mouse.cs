using UnityEngine;
using UnityEngine.Tilemaps;

public class Mouse : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Field field;
    [SerializeField] private Border border;
    [SerializeField] private Hud hud;
    [SerializeField] private Canvas canvasMenu;

    void OnMouseUp()
    {
        if (game.winWindow.activeInHierarchy) return;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = hud.HudMap.WorldToCell(worldPosition);
        if (cellPosition.y > 0) 
        {
            hud.DrawButtonUp(game.width, game.height, ButtonImage.Joy);
            game.NewGame();
        }
        else
        {
            hud.DrawButtonDown(game.width, game.height, ButtonImage.Menu);
            ReturnToMainMenu();
        }
    }

    void OnMouseDrag()
    {
        if (game.winWindow.activeInHierarchy) return;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = hud.HudMap.WorldToCell(worldPosition);
        if (cellPosition.y >0) hud.DrawButtonUp(game.width, game.height, ButtonImage.JoyPressed);
        else hud.DrawButtonDown(game.width, game.height, ButtonImage.MenuPressed);
    }

    private void Update()
    {
        if (game.winWindow.activeInHierarchy) return;
        if (Input.GetKeyDown(KeyCode.Escape)) ReturnToMainMenu();
    }

    private void ReturnToMainMenu()
    {
        field.FieldMap.ClearAllTiles();
        hud.HudMap.ClearAllTiles();
        border.BorderMap.ClearAllTiles();
        canvasMenu.enabled = true;
        game.enabled = false;
        this.enabled = false;
        game.beginnerOn = false;
        game.intermediateOn = false;
        game.expertOn = false;
    }
}
