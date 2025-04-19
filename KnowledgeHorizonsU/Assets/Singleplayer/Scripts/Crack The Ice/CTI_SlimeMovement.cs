using UnityEngine;
using System.Collections;

public class CTI_SlimeMovement : MonoBehaviour
{
    public Transform player;
    private CTI_IceTile currentTile;
    public float moveInterval = 5f;

    void Start()
    {
        StartCoroutine(MoveTowardPlayer());
    }

    public void SetCurrentTile(CTI_IceTile tile)
    {
        currentTile = tile;
        currentTile.AddSlime();
        transform.position = tile.transform.position;
    }

    IEnumerator MoveTowardPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);

            // Find next tile and move
            CTI_IceTile nextTile = FindNextTileTowardPlayer();

            if (nextTile != null)
            {
                currentTile.RemoveSlime(); // Clear slime state from old tile
                currentTile = nextTile;
                currentTile.AddSlime(); // Mark new tile as having slime
                transform.position = nextTile.transform.position;
            }
        }
    }

    CTI_IceTile FindNextTileTowardPlayer()
    {
        if (currentTile == null) return null;
        return currentTile.GetClosestNeighborTo(player.position);
    }
}
