using UnityEngine;

public class BB_DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;

    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = new Vector3(newPos.x, newPos.y, 0);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        // TODO: Check if the support is dropped over a valid SupportSlot (using Physics2D.OverlapPoint).
        // If valid, disable isKinematic and optionally attach it to the bridge with a FixedJoint2D.
    }
}
