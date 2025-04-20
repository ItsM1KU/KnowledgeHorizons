using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CTI_RectangularGrid : MonoBehaviour
{
    public GameObject iceBlockPrefab;     // Scale = 1.5
    public GameObject centerTilePrefab;   // Scale = 3
    public Camera mainCamera;

    public float tileSize = 1.5f;         // Surrounding tile size in world units
    public float centerTileSize = 3f;     // Center tile size in world units

    private List<CTI_IceTile> allTiles = new List<CTI_IceTile>();        // All ice tiles
    private List<CTI_IceTile> cornerTiles = new List<CTI_IceTile>();     // Edge or corner tiles used for spawning
    public List<CTI_IceTile> edgeTiles = new List<CTI_IceTile>();

    private Dictionary<Vector2, CTI_IceTile> tileMap = new Dictionary<Vector2, CTI_IceTile>();

    public List<CTI_IceTile> GetEdgeTiles()
    {
        return cornerTiles;
    }

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

        int halfCols = Mathf.FloorToInt(orthoWidth / tileSize);
        int halfRows = Mathf.FloorToInt(orthoHeight / tileSize);

        Instantiate(centerTilePrefab, Vector3.zero, Quaternion.identity, transform);

        for (int row = -halfRows; row <= halfRows; row++)
        {
            for (int col = -halfCols; col <= halfCols; col++)
            {
                Vector2 pos = new Vector2(col * tileSize, row * tileSize);

                if (Mathf.Abs(pos.x) < centerTileSize && Mathf.Abs(pos.y) < centerTileSize)
                    continue;

                GameObject iceBlockGO = Instantiate(iceBlockPrefab, pos, Quaternion.identity, transform);
                CTI_IceTile iceTile = iceBlockGO.GetComponent<CTI_IceTile>();
                allTiles.Add(iceTile);
                tileMap[pos] = iceTile;
            }
        }

        foreach (var kvp in tileMap)
        {
            Vector2 pos = kvp.Key;
            CTI_IceTile tile = kvp.Value;

            float x = pos.x;
            float y = pos.y;

            bool isLeft = Mathf.Approximately(x, -halfCols * tileSize);
            bool isRight = Mathf.Approximately(x, halfCols * tileSize);
            bool isTop = Mathf.Approximately(y, halfRows * tileSize);
            bool isBottom = Mathf.Approximately(y, -halfRows * tileSize);

            if (isLeft || isRight || isTop || isBottom)
            {
                cornerTiles.Add(tile);
                edgeTiles.Add(tile);    
                Debug.Log("Edge tile added at: " + pos);
            }
        }

        // Pick the 4 furthest tiles from center to spawn slimes
        Vector3 center = Vector3.zero;
        cornerTiles = cornerTiles.OrderByDescending(t => Vector3.Distance(t.transform.position, center)).Take(4).ToList();
    }
}
