using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hud : MonoBehaviour
{
    public Tilemap HudMap { get; private set; }

    public Tile tileClockNumber0;
    public Tile tileClockNumber1;
    public Tile tileClockNumber2;
    public Tile tileClockNumber3;
    public Tile tileClockNumber4;
    public Tile tileClockNumber5;
    public Tile tileClockNumber6;
    public Tile tileClockNumber7;
    public Tile tileClockNumber8;
    public Tile tileClockNumber9;
    public Tile tileClockMinus;

    public Tile tileSmileJoy;
    public Tile tileSmileWonder;
    public Tile tileSmileSad;
    public Tile tileSmileWin;
    public Tile tileSmileJoyPressed;
    public Tile tileMenu;
    public Tile tileMenuPressed;

    private void Awake()
    {
        HudMap = GetComponent<Tilemap>();
    }

    // display the total number of minutes on the MineClock and time on the TimeClock
    public void Clock(int width, int height, int mineCount, int seconds)
    {
        if (width > Game.maxWidthBorder) width = Game.maxWidthBorder;
        if (height > Game.maxHeightBorder) height = Game.maxHeightBorder;

        int abs = Math.Abs(mineCount);
        abs = Math.DivRem(abs, 10, out int digit3);
        abs = Math.DivRem(abs, 10, out int digit2);
        abs = Math.DivRem(abs, 10, out int digit1);
        abs = seconds;
        abs = Math.DivRem(abs, 10, out int digit6);
        abs = Math.DivRem(abs, 10, out int digit5);
        abs = Math.DivRem(abs, 10, out int digit4);

        if (mineCount >= 0) HudMap.SetTile(new Vector3Int(0, height + 1, 0), TileNumber(digit1));
        else HudMap.SetTile(new Vector3Int(0, height + 1, 0), tileClockMinus);

        HudMap.SetTile(new Vector3Int(1, height + 1, 0), TileNumber(digit2));
        HudMap.SetTile(new Vector3Int(2, height + 1, 0), TileNumber(digit3));
        HudMap.SetTile(new Vector3Int(width - 1, height + 1, 0), TileNumber(digit6));
        HudMap.SetTile(new Vector3Int(width - 2, height + 1, 0), TileNumber(digit5));
        HudMap.SetTile(new Vector3Int(width - 3, height + 1, 0), TileNumber(digit4));

        Matrix4x4 matrix;
        matrix = Matrix4x4.TRS(new Vector3(0.25f, 0f, 0f), Quaternion.identity, Vector3.one);
        HudMap.SetTransformMatrix(new Vector3Int(0, height + 1, 0), matrix);
        HudMap.SetTransformMatrix(new Vector3Int(width - 3, height + 1, 0), matrix);
        matrix = Matrix4x4.TRS(new Vector3(-0.25f, 0f, 0f), Quaternion.identity, Vector3.one);
        HudMap.SetTransformMatrix(new Vector3Int(2, height + 1, 0), matrix);
        HudMap.SetTransformMatrix(new Vector3Int(width - 1, height + 1, 0), matrix);
    }

    private Tile TileNumber(int number)
    {
        switch (number)
        {
            case 0: return tileClockNumber0;
            case 1: return tileClockNumber1;
            case 2: return tileClockNumber2;
            case 3: return tileClockNumber3;
            case 4: return tileClockNumber4;
            case 5: return tileClockNumber5;
            case 6: return tileClockNumber6;
            case 7: return tileClockNumber7;
            case 8: return tileClockNumber8;
            case 9: return tileClockNumber9;
            default: return null;
        }
    }

    public void DrawButtonUp (int width, int height, ButtonImage image)
    {
        if (width > Game.maxWidthBorder) width = Game.maxWidthBorder;
        if (height > Game.maxHeightBorder) height = Game.maxHeightBorder;

        Matrix4x4 matrix;

        HudMap.SetTile(new Vector3Int((width / 2), height + 1, 0), ButtonTileType(image));
        
        if (WidthEven(width))
        {
            matrix = Matrix4x4.TRS(new Vector3(-0.5f, 0f, 0f), Quaternion.identity, Vector3.one);
            HudMap.SetTransformMatrix(new Vector3Int((width / 2), height + 1, 0), matrix);
        }
    }

    public void DrawButtonDown(int width, int height, ButtonImage image)
    {
        if (width > Game.maxWidthBorder) width = Game.maxWidthBorder;
        if (height > Game.maxHeightBorder) height = Game.maxHeightBorder;

        Matrix4x4 matrix;

        HudMap.SetTile(new Vector3Int((width / 2), -2, 0), ButtonTileType(image));

        if (WidthEven(width))
        {
            matrix = Matrix4x4.TRS(new Vector3(-0.5f, 0f, 0f), Quaternion.identity, Vector3.one);
            HudMap.SetTransformMatrix(new Vector3Int((width / 2), -2, 0), matrix);
        }
    }

    private Tile ButtonTileType(ButtonImage image)
    {
        switch (image)
        {
            case ButtonImage.Joy: return tileSmileJoy;
            case ButtonImage.Sad: return tileSmileSad;
            case ButtonImage.Wonder: return tileSmileWonder;
            case ButtonImage.Win: return tileSmileWin;
            case ButtonImage.JoyPressed: return tileSmileJoyPressed;
            case ButtonImage.Menu: return tileMenu;
            case ButtonImage.MenuPressed: return tileMenuPressed;
            default: return null;
        }
    }

    private bool WidthEven(int width)
    {
        int reminder;
        int quotient = Math.DivRem(width, 2, out reminder);
        if (reminder == 0) return true;
        else return false;
    }
}