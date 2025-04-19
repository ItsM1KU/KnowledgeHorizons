using UnityEngine;
using System.Collections.Generic;

public class CTI_SlimeSpawner : MonoBehaviour
{
    [System.Serializable]
    public class DirectionalSpawn
    {
        public string direction;
        public GameObject slimePrefab;
        public CTI_IceTile spawnTile;
    }

    public Transform player;
    public List<DirectionalSpawn> spawns;
    public CTI_RectangularGrid gridGenerator; // Reference to the grid generator

    void Start()
    {
        // Make sure to get the reference to the grid generator
        if (gridGenerator == null)
        {
            gridGenerator = FindObjectOfType<CTI_RectangularGrid>();
        }

        // Spawn slimes based on the corner tiles from the grid generator
        foreach (CTI_IceTile spawnTile in gridGenerator.cornerTiles)
        {
            // Determine the direction based on the position of the corner tile
            DirectionalSpawn spawn = new DirectionalSpawn();

            if (spawnTile.transform.position.x < 0 && spawnTile.transform.position.y > 0) // Top-left
            {
                spawn.direction = "TopLeft";
            }
            else if (spawnTile.transform.position.x > 0 && spawnTile.transform.position.y > 0) // Top-right
            {
                spawn.direction = "TopRight";
            }
            else if (spawnTile.transform.position.x < 0 && spawnTile.transform.position.y < 0) // Bottom-left
            {
                spawn.direction = "BottomLeft";
            }
            else if (spawnTile.transform.position.x > 0 && spawnTile.transform.position.y < 0) // Bottom-right
            {
                spawn.direction = "BottomRight";
            }

            spawn.spawnTile = spawnTile;
            spawn.slimePrefab = GetSlimePrefabForDirection(spawn.direction);
            SpawnSlime(spawn);
        }
    }

    void SpawnSlime(DirectionalSpawn spawn)
    {
        GameObject slimeGO = Instantiate(spawn.slimePrefab);
        CTI_SlimeMovement slime = slimeGO.GetComponent<CTI_SlimeMovement>();
        slime.player = player;
        slime.SetCurrentTile(spawn.spawnTile);
    }

    GameObject GetSlimePrefabForDirection(string direction)
    {
        // Return a slime prefab based on the direction
        foreach (DirectionalSpawn spawn in spawns)
        {
            if (spawn.direction == direction)
            {
                return spawn.slimePrefab;
            }
        }
        return null;
    }
}
