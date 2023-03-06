using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundField : MonoBehaviour
{
    public Tilemap BackgroundFieldMap { get; private set; }

    public Tile backgroundField;


    private void Awake()
    {
        BackgroundFieldMap = GetComponent<Tilemap>();
    }
    // Background sprite must have resolution 3840x2160, Pixel Per Unit 64 
    // One cell of the tilemap contains the image 60 cells long
    // This method make a horizontal line 25 images long
    // Such perversion needs to keep background when Runtime Window might be absolutely minimized by vertical
    public void DrawBackgroundField(Vector3Int position)
    {
        for (int i = -720; i <= 720; i += 60)
        {
            position.x = i;
            BackgroundFieldMap.SetTile(position, backgroundField);
        }
    }
}
