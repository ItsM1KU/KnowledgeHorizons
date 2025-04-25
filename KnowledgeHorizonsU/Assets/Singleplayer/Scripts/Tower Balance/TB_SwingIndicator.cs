using UnityEngine;

public class TB_SwingIndicator : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform movingBlock;
    public Transform pivotPoint;

    void Start()
    {
        // Initialize line renderer settings
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.textureMode = LineTextureMode.Tile;

        // If you have a rope material, load it like this:
        // lineRenderer.material = Resources.Load<Material>("RopeMaterial");
    }

    void Update()
    {
        if (movingBlock != null && pivotPoint != null)
        {
            lineRenderer.SetPosition(0, pivotPoint.position);
            lineRenderer.SetPosition(1, movingBlock.position);
        }
    }
}