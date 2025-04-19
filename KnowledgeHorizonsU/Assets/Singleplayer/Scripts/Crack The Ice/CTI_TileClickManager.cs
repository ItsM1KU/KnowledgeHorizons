using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CTI_TileClickManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                CTI_IceTile tile = hit.collider.GetComponent<CTI_IceTile>();
                if (tile != null && tile.hasSlime)
                {
                    tile.OnTileClicked();
                }
            }
        }
    }
}
