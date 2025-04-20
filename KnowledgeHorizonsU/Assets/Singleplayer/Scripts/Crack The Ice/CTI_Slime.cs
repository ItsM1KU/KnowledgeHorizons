using UnityEngine;

public class CTI_Slime : MonoBehaviour
{
    public float moveInterval = 5f;
    public float jumpSpeed = 3f;
    public Sprite upSprite, downSprite, leftSprite, rightSprite;

    private CTI_IceTile currentTile;
    private Transform centerTarget;
    private SpriteRenderer spriteRenderer;

    public void Initialize(CTI_IceTile startTile, Transform center, Sprite up, Sprite down, Sprite left, Sprite right)
    {
        currentTile = startTile;
        centerTarget = center;
        upSprite = up;
        downSprite = down;
        leftSprite = left;
        rightSprite = right;
        spriteRenderer = GetComponent<SpriteRenderer>();

        MoveToTile(currentTile.transform.position); // Place on top of start tile
        InvokeRepeating(nameof(MoveTowardsCenter), moveInterval, moveInterval);
    }

    void MoveTowardsCenter()
    {
        if (currentTile == null || centerTarget == null) return;

        CTI_IceTile nextTile = currentTile.GetClosestNeighborTo(centerTarget.position);

        if (nextTile != null)
        {
            Vector3 dir = (nextTile.transform.position - currentTile.transform.position).normalized;
            UpdateDirectionSprite(dir);
            currentTile = nextTile;
            MoveToTile(currentTile.transform.position);
        }
    }

    void MoveToTile(Vector3 position)
    {
        // Optional: You can tween or animate this
        transform.position = position + Vector3.up * 0.1f;
    }

    void UpdateDirectionSprite(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            spriteRenderer.sprite = (direction.x > 0) ? rightSprite : leftSprite;
        }
        else
        {
            spriteRenderer.sprite = (direction.y > 0) ? upSprite : downSprite;
        }
    }
}
