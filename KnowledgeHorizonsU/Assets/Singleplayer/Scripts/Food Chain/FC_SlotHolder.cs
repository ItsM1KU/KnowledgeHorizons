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
                draggedObj.transform.SetParent(transform);
                draggedObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                Debug.Log(draggedObj.GetComponent<FC_AnimalInfo>().AnimalName);
                isoccupied = true;
                Debug.Log(isoccupied);
            }
        }
        else
        {
            Debug.Log("Slot is already occupied");
        }
    }

}
