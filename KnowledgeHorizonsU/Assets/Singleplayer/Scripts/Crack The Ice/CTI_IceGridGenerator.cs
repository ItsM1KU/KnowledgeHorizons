using UnityEngine;

public class CTI_IceGridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;          // Assign your tile prefab here (with SpriteRenderer)
    public GameObject centerTilePrefab;    // Assign your special center tile prefab here

    public int gridWidth = 11;
    public int gridHeight = 11;

    // Tile base size in units (256 px / 100 PPU)
    private float baseTileSize = 2.56f;

    void Start()
    {
        SetupCameraAndGrid();
    }

    void SetupCameraAndGrid()
    {
        // Calculate aspect ratio (assuming 1920x1080)
        float aspectRatio = 1920f / 1080f;

        // Calculate orthographic size needed to fit grid horizontally exactly
        float horizontalOrthoSize = (gridWidth * baseTileSize) / (2f * aspectRatio);

        // Calculate scale needed for tiles to also fit vertically
        float verticalSizeNeeded = (gridHeight * baseTileSize);
        float cameraVerticalView = horizontalOrthoSize * 2f;
        float tileScale = cameraVerticalView / verticalSizeNeeded;

        // Clamp tileScale to max 1 (don't upscale tiles)
        tileScale = Mathf.Min(tileScale, 1f);

        // Apply camera orthographic size
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = horizontalOrthoSize;

        // Center camera at (0,0,-10)
        Camera.main.transform.position = new Vector3(0f, 0f, -10f);

        // Clear existing tiles (optional if you want to reset)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Calculate offset to center grid at (0,0)
        float totalGridWidth = gridWidth * baseTileSize * tileScale;
        float totalGridHeight = gridHeight * baseTileSize * tileScale;

        Vector2 bottomLeft = new Vector2(-totalGridWidth / 2f + (baseTileSize * tileScale) / 2f,
                                         -totalGridHeight / 2f + (baseTileSize * tileScale) / 2f);

        // Instantiate tiles
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2 tilePos = new Vector2(bottomLeft.x + x * baseTileSize * tileScale,
                                              bottomLeft.y + y * baseTileSize * tileScale);

                GameObject toSpawn;

                // Use centerTilePrefab for center tile, else normal tilePrefab
                if (x == gridWidth / 2 && y == gridHeight / 2 && centerTilePrefab != null)
                    toSpawn = centerTilePrefab;
                else
                    toSpawn = tilePrefab;

                GameObject tile = Instantiate(toSpawn, tilePos, Quaternion.identity, transform);

                // Scale tile accordingly
                tile.transform.localScale = new Vector3(tileScale, tileScale, 1f);
            }
        }
    }
}
