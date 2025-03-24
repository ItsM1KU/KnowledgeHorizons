using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = transform.position;
    }

    // Called when dragging starts
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Make it slightly transparent
        canvasGroup.blocksRaycasts = false; // Allow raycast detection to go through
    }

    // Called while dragging
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; // Move the UI element with the mouse
    }

    // Called when dragging ends
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // Reset transparency
        canvasGroup.blocksRaycasts = true; // Enable raycasts
    }

    // Reset position if not dropped in the beaker
    public void ResetPosition()
    {
        transform.position = originalPosition;
    }
}
