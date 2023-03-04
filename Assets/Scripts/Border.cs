using UnityEngine;
using UnityEngine.Tilemaps;

public class Border : MonoBehaviour
{
    public Tilemap BorderMap { get; private set; }

    public Tile tileBorderDown;
    public Tile tileBorderLeft;
    public Tile tileBorderRight;
    public Tile tileBorderUp;
    public Tile tileClear;
    public Tile tileClockBorderDown;
    public Tile tileClockBorderUp;
    public Tile tileClockBorderLeft;
    public Tile tileClockBorderRight;
    public Tile tileClockCornerLeftDown;
    public Tile tileClockCornerLeftUp;
    public Tile tileClockCornerRightDown;
    public Tile tileClockCornerRightUp;
    public Tile tileCornerLeftDown;
    public Tile tileCornerLeftUp;
    public Tile tileCornerRightDown;
    public Tile tileCornerRightUp;
    public Tile tileFieldBorderLeft;
    public Tile tileFieldBorderRight;
    public Tile tileIntersectionLeftDown;
    public Tile tileIntersectionLeftUp;
    public Tile tileIntersectionRightDown;
    public Tile tileIntersectionRightUp;

    public Tile tileBorderDownMark;
    public Tile tileBorderUpMark;
    public Tile tileFieldBorderLeftMark;
    public Tile tileFieldBorderRightMark;
    public Tile tileClockBorderDownMark;
    public Tile tileClockCornerLeftDownMark;
    public Tile tileClockCornerRightDownMark;


    private void Awake()
    {
        BorderMap = GetComponent<Tilemap>();
    }

    public void DrawBorder(int width, int height)
    {
        BorderMap.SetTile(new Vector3Int(-1, -3, 0), tileCornerLeftDown);
        BorderMap.SetTile(new Vector3Int(-1, -2, 0), tileBorderLeft);
        BorderMap.SetTile(new Vector3Int(-1, -1, 0), tileIntersectionLeftDown);
        BorderMap.SetTile(new Vector3Int(-1, height, 0), tileIntersectionLeftUp);
        BorderMap.SetTile(new Vector3Int(-1, height + 1, 0), tileBorderLeft);
        BorderMap.SetTile(new Vector3Int(-1, height + 2, 0), tileCornerLeftUp);
        BorderMap.SetTile(new Vector3Int(width, height + 2, 0), tileCornerRightUp);
        BorderMap.SetTile(new Vector3Int(width, height + 1, 0), tileBorderRight);
        BorderMap.SetTile(new Vector3Int(width, height, 0), tileIntersectionRightUp);
        BorderMap.SetTile(new Vector3Int(width, -1, 0), tileIntersectionRightDown);
        BorderMap.SetTile(new Vector3Int(width, -2, 0), tileBorderRight);
        BorderMap.SetTile(new Vector3Int(width, -3, 0), tileCornerRightDown);
        BorderMap.SetTile(new Vector3Int(0, height + 2, 0), tileClockCornerLeftUp);
        BorderMap.SetTile(new Vector3Int(width - 3, height + 2, 0), tileClockCornerLeftUp);
        BorderMap.SetTile(new Vector3Int(0, height + 1, 0), tileClockBorderLeft);
        BorderMap.SetTile(new Vector3Int(width - 3, height + 1, 0), tileClockBorderLeft);
        BorderMap.SetTile(new Vector3Int(0, height, 0), tileClockCornerLeftDown);
        BorderMap.SetTile(new Vector3Int(width - 3, height, 0), tileClockCornerLeftDown);
        BorderMap.SetTile(new Vector3Int(1, height + 2, 0), tileClockBorderUp);
        BorderMap.SetTile(new Vector3Int(width - 2, height + 2, 0), tileClockBorderUp);
        BorderMap.SetTile(new Vector3Int(1, height + 1, 0), tileClear);
        BorderMap.SetTile(new Vector3Int(width - 2, height + 1, 0), tileClear);
        BorderMap.SetTile(new Vector3Int(1, height, 0), tileClockBorderDown);
        BorderMap.SetTile(new Vector3Int(width - 2, height, 0), tileClockBorderDown);
        BorderMap.SetTile(new Vector3Int(2, height + 2, 0), tileClockCornerRightUp);
        BorderMap.SetTile(new Vector3Int(width - 1, height + 2, 0), tileClockCornerRightUp);
        BorderMap.SetTile(new Vector3Int(2, height + 1, 0), tileClockBorderRight);
        BorderMap.SetTile(new Vector3Int(width - 1, height + 1, 0), tileClockBorderRight);
        BorderMap.SetTile(new Vector3Int(2, height, 0), tileClockCornerRightDown);
        BorderMap.SetTile(new Vector3Int(width - 1, height, 0), tileClockCornerRightDown);
        for (int i = 3; i < width - 3; i++)
        {
            BorderMap.SetTile(new Vector3Int(i, height + 2, 0), tileBorderUp);
            BorderMap.SetTile(new Vector3Int(i, height + 1, 0), tileClear);
            BorderMap.SetTile(new Vector3Int(i, height, 0), tileBorderDown);
        }
        for (int i = 0; i < width; i++)
        {
            BorderMap.SetTile(new Vector3Int(i, -1, 0), tileBorderUp);
            BorderMap.SetTile(new Vector3Int(i, -2, 0), tileClear);
            BorderMap.SetTile(new Vector3Int(i, -3, 0), tileBorderDown);
        }
        for (int i = 0; i < height; i++)
        {
            BorderMap.SetTile(new Vector3Int(-1, i, 0), tileFieldBorderLeft);
            BorderMap.SetTile(new Vector3Int(width, i, 0), tileFieldBorderRight);
        }
    }
    public void FieldBorderMarkLeft(int height, bool marked)
    {
        if (marked)
            for (int i = 0; i < height; i++) BorderMap.SetTile(new Vector3Int(-1, i, 0), tileFieldBorderLeftMark);
        else
            for (int i = 0; i < height; i++) BorderMap.SetTile(new Vector3Int(-1, i, 0), tileFieldBorderLeft);
    }
    public void FieldBorderMarkRight(int width, int height, bool marked)
    {
        if (marked)
            for (int i = 0; i < height; i++) BorderMap.SetTile(new Vector3Int(width, i, 0), tileFieldBorderRightMark);
        else 
            for (int i = 0; i < height; i++) BorderMap.SetTile(new Vector3Int(width, i, 0), tileFieldBorderRight);
    }
    public void FieldBorderMarkDown(int width, bool marked)
    {
        if (marked)
            for (int i = 0; i < width; i++) BorderMap.SetTile(new Vector3Int(i, -1, 0), tileBorderUpMark);
        else
            for (int i = 0; i < width; i++) BorderMap.SetTile(new Vector3Int(i, -1, 0), tileBorderUp);
    }
    public void FieldBorderMarkUp(int width, int height, bool marked)
    {
        if (marked)
        { 
            BorderMap.SetTile(new Vector3Int(0, height, 0), tileClockCornerLeftDownMark);
            BorderMap.SetTile(new Vector3Int(width - 3, height, 0), tileClockCornerLeftDownMark);
            BorderMap.SetTile(new Vector3Int(1, height, 0), tileClockBorderDownMark);
            BorderMap.SetTile(new Vector3Int(width - 2, height, 0), tileClockBorderDownMark);
            BorderMap.SetTile(new Vector3Int(2, height, 0), tileClockCornerRightDownMark);
            BorderMap.SetTile(new Vector3Int(width - 1, height, 0), tileClockCornerRightDownMark);
            for (int i = 3; i < width - 3; i++) BorderMap.SetTile(new Vector3Int(i, height, 0), tileBorderDownMark);
        }
        else
        {
            BorderMap.SetTile(new Vector3Int(0, height, 0), tileClockCornerLeftDown);
            BorderMap.SetTile(new Vector3Int(width - 3, height, 0), tileClockCornerLeftDown);
            BorderMap.SetTile(new Vector3Int(1, height, 0), tileClockBorderDown);
            BorderMap.SetTile(new Vector3Int(width - 2, height, 0), tileClockBorderDown);
            BorderMap.SetTile(new Vector3Int(2, height, 0), tileClockCornerRightDown);
            BorderMap.SetTile(new Vector3Int(width - 1, height, 0), tileClockCornerRightDown);
            for (int i = 3; i < width - 3; i++) BorderMap.SetTile(new Vector3Int(i, height, 0), tileBorderDown);
        }
    }
}
