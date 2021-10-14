using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //Slot Detail
    public int slot_id;
    public string slot_type;
    public bool empty;
    

    //Item Detail
    public GameObject item;
    public int itemID;
    public string itemType;
    public string itemDescription;
    public Sprite ItemIcon;
    public int itemCount = 0;
    private Transform SlotIcon;


    //For Quick slot
    public bool Selected;

    //For Slot
    public bool Equipped = false;

    private void Start()
    {
        string id = transform.name.Replace("(", "").Replace(")","").Replace(" ","").Replace("Slot","").Replace("Quick","");
        slot_id = GameManager.ConvertInt(id);
        
    }

    public void UpdateSlot()
    {
        SlotIcon = transform.GetChild(0).transform;
        SlotIcon.GetComponent<Image>().sprite = ItemIcon;
        if (slot_type == "Slot")
        {
            if (Equipped)
            {
                Debug.Log("Equipped");
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void ResetSlot()
    {
        empty = true;
        transform.GetComponent<Image>().sprite = null;
        itemCount = 0;
        ItemIcon = null;
        Equipped = false;
        UpdateSlot();
    }
}
