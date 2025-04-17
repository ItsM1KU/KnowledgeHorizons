using UnityEngine;

public class CTI_SquareGridGenerator : MonoBehaviour
{
    public GameObject iceBlockPrefab;
    public int gridSize = 7; // Size of the grid (7x7)
    public float tileSize = 1f;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = -gridSize / 2; x <= gridSize / 2; x++)
        {
            for (int y = -gridSize / 2; y <= gridSize / 2; y++)
            {
                Vector2 position = new Vector2(x * tileSize, y * tileSize);
                GameObject tile = Instantiate(iceBlockPrefab, position, Quaternion.identity, transform);
                tile.name = $"Tile_{x}_{y}";
            }
        }
    }
}
