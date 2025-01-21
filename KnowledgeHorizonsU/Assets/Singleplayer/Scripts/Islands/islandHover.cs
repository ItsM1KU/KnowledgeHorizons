using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class islandHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hoverText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(true); 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(false);
        }
    }
}
