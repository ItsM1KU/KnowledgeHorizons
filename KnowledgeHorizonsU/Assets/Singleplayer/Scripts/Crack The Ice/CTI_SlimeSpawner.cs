using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTI_SlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;
    public float spawnInterval = 5f;
    public Transform centerTileTransform;

    public Sprite upSprite, downSprite, leftSprite, rightSprite;

    private List<CTI_IceTile> edgeTiles;

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        // Wait until the end of the first frame to let the grid generate
        yield return new WaitForSeconds(0.1f);

        CTI_RectangularGrid grid = FindObjectOfType<CTI_RectangularGrid>();
        if (grid != null)
        {
            edgeTiles = grid.edgeTiles;
        }

        if (edgeTiles == null || edgeTiles.Count == 0)
        {
            Debug.LogWarning("No edge tiles found after delay!");
            yield break;
        }

        if (slimePrefab == null || centerTileTransform == null)
        {
            Debug.LogError("Slime prefab or center tile not assigned!");
            yield break;
        }

        StartCoroutine(SpawnSlimesRoutine());
    }

    IEnumerator SpawnSlimesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            CTI_IceTile randomTile = edgeTiles[Random.Range(0, edgeTiles.Count)];
            Debug.Log("Spawning slime at: " + randomTile.transform.position);

            GameObject slimeGO = Instantiate(slimePrefab, randomTile.transform.position + Vector3.up * 0.1f, Quaternion.identity);
            CTI_Slime slime = slimeGO.GetComponent<CTI_Slime>();

            if (slime != null)
            {
                slime.Initialize(
                    randomTile,
                    centerTileTransform,
                    upSprite,
                    downSprite,
                    leftSprite,
                    rightSprite
                );
            }
            else
            {
                Debug.LogError("CTI_Slime script not found on prefab!");
            }
        }
    }
}
