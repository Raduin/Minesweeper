using UnityEngine;
using UnityEngine.Tilemaps;

public class Field : MonoBehaviour
{
    public Tilemap FieldMap { get; private set; }

    public Tile tileUnknown;
    public Tile tileEmpty;
    public Tile tileMine;
    public Tile tileExploded;
    public Tile tileFlag;
    public Tile tileNum1;
    public Tile tileNum2;
    public Tile tileNum3;
    public Tile tileNum4;
    public Tile tileNum5;
    public Tile tileNum6;
    public Tile tileNum7;
    public Tile tileNum8;
    public Tile tileMineWrong;
    public Tile tileQuestionMark;
    public Tile tileQuestionMarkDown;

    private void Awake()
    {
        FieldMap = GetComponent<Tilemap>();
    }

    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                DrawOneCell(state[x, y]);
            }
        }
    }

    public void DrawOneCell(Cell cell)
    {
        FieldMap.SetTile(cell.position, GetTile(cell));
    }
    public void DrawOneCellEmpty(Vector3Int position)
    {
        FieldMap.SetTile(position, tileEmpty);
    }
    public void DrawOneCellUnknown(Vector3Int position)
    {
        FieldMap.SetTile(position, tileUnknown);
    }
    public void DrawOneCellQuestionMark(Vector3Int position)
    {
        FieldMap.SetTile(position, tileQuestionMark);
    }
    public void DrawOneCellQuestionMarkDown(Vector3Int position)
    {
        FieldMap.SetTile(position, tileQuestionMarkDown);
    }

    private Tile GetTile(Cell cell)
    {
        if (cell.revealed) return GetRevealedTile(cell);
        else if (cell.flagged) return tileFlag;
        else if (cell.questionMark) return tileQuestionMark;
        else return tileUnknown;
    }

    private Tile GetRevealedTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty: return cell.mineWrong? tileMineWrong : tileEmpty;
            case Cell.Type.Mine:  return cell.exploded ? tileExploded : tileMine;
            case Cell.Type.Number: return cell.mineWrong ? tileMineWrong : GetNumberTile(cell);
            default: return null;
        }
    }

    private Tile GetNumberTile(Cell cell)
    {
        switch (cell.number)
        {
            case 1: return tileNum1;
            case 2: return tileNum2;
            case 3: return tileNum3;
            case 4: return tileNum4;
            case 5: return tileNum5;
            case 6: return tileNum6;
            case 7: return tileNum7;
            case 8: return tileNum8;
            default: return null;
        }
    }
}
