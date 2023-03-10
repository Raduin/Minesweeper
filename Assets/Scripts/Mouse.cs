using UnityEngine;
using UnityEngine.Tilemaps;

public class Mouse : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Field field;
    [SerializeField] private Border border;
    [SerializeField] private Hud hud;
    [SerializeField] private Canvas canvas;

    void OnMouseUp()
    {
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
            field.FieldMap.ClearAllTiles();
            hud.HudMap.ClearAllTiles();
            border.BorderMap.ClearAllTiles();
            canvas.enabled = true;
            game.enabled = false;
            this.enabled = false;
        }
    }

    void OnMouseDrag()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = hud.HudMap.WorldToCell(worldPosition);
        if (cellPosition.y >0) hud.DrawButtonUp(game.width, game.height, ButtonImage.JoyPressed);
        else hud.DrawButtonDown(game.width, game.height, ButtonImage.MenuPressed);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            field.FieldMap.ClearAllTiles();
            hud.HudMap.ClearAllTiles();
            border.BorderMap.ClearAllTiles();
            canvas.enabled = true;
            game.enabled = false;
            this.enabled = false;
        }
    }
}
