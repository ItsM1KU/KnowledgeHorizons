using UnityEngine;
using System.Collections.Generic;

public class CTI_RectangularGrid : MonoBehaviour
{
    public GameObject iceBlockPrefab;     // Scale = 1.5
    public GameObject centerTilePrefab;   // Scale = 3
    public Camera mainCamera;

    public float tileSize = 1.5f;         // Surrounding tile size in world units
    public float centerTileSize = 3f;     // Center tile size in world units

    public List<CTI_IceTile> allTiles = new List<CTI_IceTile>(); // To store all the tiles
    public List<CTI_IceTile> cornerTiles = new List<CTI_IceTile>(); // To store corner tiles

    void Start()
    {
        GenerateSymmetricalGrid();
    }

    void GenerateSymmetricalGrid()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        float orthoHeight = mainCamera.orthographicSize;
        float aspect = 16f / 9f;
        float orthoWidth = orthoHeight * aspect;

        // Calculate how many tiles can fit on each side
        int halfCols = Mathf.FloorToInt(orthoWidth / tileSize);
        int halfRows = Mathf.FloorToInt(orthoHeight / tileSize);

        // Place center tile at origin
        Instantiate(centerTilePrefab, Vector3.zero, Quaternion.identity, transform);

        for (int row = -halfRows; row <= halfRows; row++)
        {
            for (int col = -halfCols; col <= halfCols; col++)
            {
                Vector2 pos = new Vector2(col * tileSize, row * tileSize);

                // Leave a square area at center for the larger center tile
                if (Mathf.Abs(pos.x) < centerTileSize && Mathf.Abs(pos.y) < centerTileSize)
                    continue;

                // Instantiate and add the ice block to the list
                GameObject iceBlockGO = Instantiate(iceBlockPrefab, pos, Quaternion.identity, transform);
                CTI_IceTile iceTile = iceBlockGO.GetComponent<CTI_IceTile>();
                allTiles.Add(iceTile);

                // Identify and store corner tiles
                if ((row == -halfRows || row == halfRows) && (col == -halfCols || col == halfCols))
                {
                    cornerTiles.Add(iceTile);
                }
            }
        }
    }
}
