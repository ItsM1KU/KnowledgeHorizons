using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FC_SlotHolder : MonoBehaviour, IDropHandler
{
    public bool isoccupied = false;
    public void OnDrop(PointerEventData eventData)
    {
        if (!isoccupied)
        {
            GameObject draggedObj = eventData.pointerDrag;

            if(draggedObj != null)
            {    
                draggedObj.GetComponent<RectTransform>().anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
                isoccupied = true;
            }
        }
        else
        {
            Debug.Log("Slot is already occupied");
        }
    }

}
