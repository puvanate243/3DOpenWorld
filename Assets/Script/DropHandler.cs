using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler 
{
    bool empty;

    GameObject item;
    int itemID;
    string itemType;
    string itemDescription;
    Sprite ItemIcon;

    int itemCount;

    public void OnDrop(PointerEventData eventData)
    {
        if (DragHandler.ItemDrag != null)
        {
            GetItemDetail();
            transform.GetChild(0).transform.position = DragHandler.ItemDrag.GetComponent<DragHandler>().StartPosition;
            transform.GetChild(0).transform.SetParent(DragHandler.ItemDrag.GetComponent<DragHandler>().StartParent);
            DragHandler.ItemDrag.transform.SetParent(transform);
            DragHandler.ItemDrag.transform.position = transform.position;
        }

    }

    private void GetItemDetail()
    {
        Slot from = DragHandler.ItemDrag.transform.parent.gameObject.GetComponent<Slot>();
        Slot to = transform.gameObject.GetComponent<Slot>();

        empty = from.empty;
        item = from.item;
        itemID = from.itemID;
        itemType = from.itemType;
        itemDescription = from.itemDescription;
        ItemIcon = from.ItemIcon;
        itemCount = from.itemCount;

        from.empty = to.empty;
        from.item = to.item;
        from.itemID = to.itemID;
        from.itemType = to.itemType;
        from.itemDescription = to.itemDescription;
        from.ItemIcon = to.ItemIcon;
        from.itemCount = to.itemCount;

        to.empty = empty;
        to.item = item;
        to.itemID = itemID;
        to.itemType = itemType;
        to.itemDescription = itemDescription;
        to.ItemIcon = ItemIcon;
        to.itemCount = itemCount;


    }




}
