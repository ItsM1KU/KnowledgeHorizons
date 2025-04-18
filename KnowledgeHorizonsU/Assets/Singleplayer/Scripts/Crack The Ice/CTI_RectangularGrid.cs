using UnityEngine;

public class CTI_RectangularGrid : MonoBehaviour
{
    public GameObject iceBlockPrefab;     // Scale 1.5
    public GameObject centerTilePrefab;   // Scale 3
    public int rows = 9;
    public int cols = 9;

    void Start()
    {
        GenerateGridWithGap();
    }

    void GenerateGridWithGap()
    {
        float tileSize = 1.5f;      // Surrounding tile size (based on scale)
        float centerTileSize = 3f;  // Size of center tile in world units

        Vector2 center = Vector2.zero;

        // Place the large center tile at the origin
        Instantiate(centerTilePrefab, center, Quaternion.identity, transform);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float x = (col - cols / 2) * tileSize;
                float y = (row - rows / 2) * tileSize;
                Vector2 pos = new Vector2(x, y);

                // Skip a full square region the size of the center tile
                if (Mathf.Abs(pos.x) < centerTileSize && Mathf.Abs(pos.y) < centerTileSize)
                    continue;

                Instantiate(iceBlockPrefab, pos, Quaternion.identity, transform);
            }
        }
    }
}
