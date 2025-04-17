using UnityEngine;

public class CTI_IceBlockGenerator : MonoBehaviour
{
    public GameObject iceBlockPrefab; // Prefab with size 0.25
    public int gridRadius = 3;        // How far out from center to generate blocks
    public float tileSize = 0.25f;    // Size of each tile
    public Vector2 centerSize = new Vector2(0.5f, 0.5f); // Center tile size

    void Start()
    {
        GenerateIceGrid();
    }

    void GenerateIceGrid()
    {
        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                // Skip center area (occupied by larger tile)
                if (Mathf.Abs(x * tileSize) < centerSize.x && Mathf.Abs(y * tileSize) < centerSize.y)
                    continue;

                Vector3 position = new Vector3(x * tileSize, y * tileSize, 0);
                Instantiate(iceBlockPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
