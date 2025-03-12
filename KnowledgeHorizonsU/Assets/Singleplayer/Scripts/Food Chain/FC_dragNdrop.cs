using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class FC_dragNdrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform objRect;
    [SerializeField] Canvas _canvas;
    [SerializeField] FC_AnimalInfo animalInfo;
    private CanvasGroup canvasgrp;

    [SerializeField] GameObject animalNameText;

    private Transform orgTransform;
    private void Awake()
    {   
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        objRect = GetComponent<RectTransform>();
        canvasgrp = GetComponent<CanvasGroup>();
        orgTransform = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDragging");
        canvasgrp.alpha = 0.5f;
        canvasgrp.blocksRaycasts = false;

        /*if(transform.parent.TryGetComponent<FC_SlotHolder>(out FC_SlotHolder slot))
        {
            slot.isoccupied = false;
        }

        orgTransform = transform.parent;
        //transform.SetParent(_canvas.transform);
        */

        if(transform.parent.TryGetComponent<FC_SlotHolder>(out FC_SlotHolder slot))
        {
            slot.isoccupied = false;
            transform.SetParent(_canvas.transform);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDragging");
        
        objRect.anchoredPosition += eventData.delta;
        clamptoCanvas();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDragging");
        canvasgrp.alpha = 1f;
        canvasgrp.blocksRaycasts = true;
        /*
        GameObject droptarget = eventData.pointerEnter;
        if (droptarget != null && droptarget.TryGetComponent<FC_SlotHolder>(out FC_SlotHolder slot)) 
        {
            if (!slot.isoccupied)
            { 
                //transform.SetParent(droptarget.transform);
                objRect.anchoredPosition = droptarget.GetComponent<RectTransform>().anchoredPosition;
                slot.isoccupied = true;
            }
            else
            {
                Debug.Log("Slot is already occupied");
                //transform.SetParent(orgTransform);
                //objRect.anchoredPosition = Vector2.zero;
            }
        }
        else
        {
            //transform.SetParent(orgTransform);
            //objRect.anchoredPosition = Vector2.zero;
        }*/
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void clamptoCanvas()
    {
        if(transform.parent == _canvas.transform)
        {
            return;
        }
        Vector2 newpos = objRect.localPosition;

        Vector3 minPos = _canvas.GetComponent<RectTransform>().rect.min - objRect.rect.min;
        Vector3 maxPos = _canvas.GetComponent<RectTransform>().rect.max - objRect.rect.max;

        newpos.x = Mathf.Clamp(newpos.x, minPos.x, maxPos.x);
        newpos.y = Mathf.Clamp(newpos.y, minPos.y, maxPos.y);

        objRect.localPosition = newpos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animalNameText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animalNameText.SetActive(false);
    }
}
