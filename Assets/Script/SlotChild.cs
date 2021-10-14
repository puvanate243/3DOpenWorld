using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotChild : MonoBehaviour , IPointerClickHandler
{
    Slot slot;
    PlayerController player;
    public GameObject menu;

       
    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left && !slot.empty && slot.slot_type == "Slot")
        {
            menu.transform.position = transform.position;
            menu.GetComponent<ItemMenu>().slot_id = slot.slot_id;
            menu.SetActive(true);
        }

        if (eventData.button == PointerEventData.InputButton.Left && slot.slot_type == "Quick")
        { 
            player.ClickSwitchQuickSlot(slot.slot_id);
        }

    }

    void Start()
    {
        menu.SetActive(false);
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        slot = transform.parent.GetComponent<Slot>();
        if (slot.empty == false)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<Text>().text = slot.itemCount.ToString();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (menu.activeSelf == true)
            {
                menu.SetActive(false);
            }
        }

    }

}
