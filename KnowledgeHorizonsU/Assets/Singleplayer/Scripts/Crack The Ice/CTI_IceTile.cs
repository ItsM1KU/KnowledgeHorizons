using System.Collections.Generic;
using UnityEngine;

public class CTI_IceTile : MonoBehaviour
{
    public bool hasSlime = false;
    public int clickCount = 0;
    public int maxClicks = 5;

    // 🧩 List of neighboring tiles (populate this during grid generation)
    public List<CTI_IceTile> neighbors = new List<CTI_IceTile>();

    public void OnTileClicked()
    {
        if (!hasSlime) return;

        clickCount++;
        Debug.Log($"Tile clicked {clickCount} times!");

        if (clickCount >= maxClicks)
        {
            DestroySlime();
            Destroy(gameObject); // destroys the ice tile
        }
    }

    public void AddSlime()
    {
        hasSlime = true;
        clickCount = 0;
    }

    public void RemoveSlime()
    {
        hasSlime = false;
        clickCount = 0;
    }

    void DestroySlime()
    {
        // You can trigger slime death animation here
        Debug.Log("Slime destroyed!");
    }

    // ✅ NEW: Get closest neighbor tile toward a given position (used by slimes)
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
