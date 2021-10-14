using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Vector3 StartPosition;
    public static GameObject ItemDrag;
    public Transform StartParent;
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (!transform.parent.GetComponent<Slot>().empty)
        {
            ItemDrag = gameObject;
            StartPosition = transform.position;
            StartParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!transform.parent.GetComponent<Slot>().empty)
        {
            transform.position = Input.mousePosition;

            transform.GetComponent<Canvas>().sortingOrder = 20;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!transform.parent.GetComponent<Slot>().empty)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.GetComponent<Canvas>().sortingOrder = 10;
            if (transform.parent == StartParent)
            {
                transform.position = StartPosition;
                
            }
        }
        ItemDrag = null;

    }
}
