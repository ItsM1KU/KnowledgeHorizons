using System.Collections.Generic;
using UnityEngine;

public class CTI_IceTile : MonoBehaviour
{
    public int clickCount = 0;
    public int maxClicks = 5;

    // Neighboring tiles (can be assigned later by grid logic if needed)
    public List<CTI_IceTile> neighbors = new List<CTI_IceTile>();

    public void OnTileClicked()
    {
        clickCount++;
        Debug.Log($"Tile clicked {clickCount} times!");

        if (clickCount >= maxClicks)
        {
            Destroy(gameObject); // Destroy the ice tile
        }
    }

    // Utility: Get closest neighbor to a position (optional, useful for future pathfinding)
    public CTI_IceTile GetClosestNeighborTo(Vector3 targetPosition)
    {
        CTI_IceTile closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var neighbor in neighbors)
        {
            if (neighbor == null) continue;

            float distance = Vector3.Distance(neighbor.transform.position, targetPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = neighbor;
            }
        }

        return closest;
    }
}
