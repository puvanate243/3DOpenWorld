using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Movement
    [SerializeField]
    private float MoveSpeed = 1f;
    [SerializeField]
    private float RotationSpeed = 1f;
    private float MoveX, MoveZ;
    public bool CanMove = true;

    //Inventory
    public Transform Forward_position;
    public GameObject SlotHolder;
    public GameObject ItemMenu;
    private int AllSlot = 28;
    private GameObject[] slot;
    public GameObject BagCanvas;
    private bool BagEnabled;

    //QuickItem
    public GameObject QuickSlotHolder;
    private int AllQuickSlotHolder = 9;
    private GameObject[] QuickSlot;
    private int QuickSlotSelected = 0;
    public GameObject Selected_Canvas;

    //Weapon
    public GameObject AllWeapon;
    private int Equipped = -1;

    //Attack
    public GameObject MainWeapon;
    Animator MainWeaponAnim;

    //GameManager
    GameManager GM;

    //ItemLog
    public GameObject ItemLog;

    //JoyStick
    public FixedJoystick JoyStick;

    void Start()
    {
        Setting();
    }

    private void Setting()
    {
        //GameManager
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Inventory
        GetAllSlotItem();
        

        //QuickSlot
        GetAllQuickSlot();
    }

    void Update()
    {
        Movement();
        Inventory();
        Attack();
       
    }

    //Movement
    private void Movement()
    {
        if (CanMove)
        {
            MoveX = Input.GetAxisRaw("Horizontal");
            MoveZ = Input.GetAxisRaw("Vertical");
            //MoveX = JoyStick.Horizontal;
            //MoveZ = JoyStick.Vertical;
            if (MoveX != 0 || MoveZ != 0)
            {
                Vector3 Direction = new Vector3(MoveX, 0, MoveZ) * MoveSpeed * Time.deltaTime;
                Quaternion Rotation = Quaternion.LookRotation(Direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, RotationSpeed * Time.deltaTime);
                transform.Translate(Direction, Space.World);
            }
        }
        
    }
    
    //Pick up item
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            GameObject Item = other.gameObject;
            Item item = other.gameObject.GetComponent<Item>();
            AddItem(Item, item.itemID, item.itemType, item.itemDescription, item.itemIcon);
            ItemLog.GetComponent<ItemLog>().AddLog(item.itemDescription);
          
        }
    }
    public void AddItem(GameObject itemObject ,int itemId,string itemType,string itemDescription, Sprite itemIcon)
    {
        int SlotSelect = CheckSameItem(itemId);
        if (SlotSelect < slot.Length)
        {
            Destroy(itemObject);
            slot[SlotSelect].GetComponent<Slot>().itemCount += 1;
        }
        else
        {

            for (int i = 0; i < AllSlot; i++)
            {
                if (slot[i].GetComponent<Slot>().empty)
                {
                    slot[i].GetComponent<Slot>().item = itemObject;
                    slot[i].GetComponent<Slot>().ItemIcon = itemIcon;
                    slot[i].GetComponent<Slot>().itemID = itemId;
                    slot[i].GetComponent<Slot>().itemType = itemType;
                    slot[i].GetComponent<Slot>().itemDescription = itemDescription;
                    itemObject.transform.parent = slot[i].transform.GetChild(0).Find("ItemObject");
                    itemObject.SetActive(false);
                    slot[i].GetComponent<Slot>().UpdateSlot();
                    slot[i].GetComponent<Slot>().empty = false;
                    slot[i].GetComponent<Slot>().itemCount += 1;
                    return;
                }
            }
        }
        
    }

    //Inventory
    private void Inventory()
    {
        Bag();
        QuickSlotController();
        SelectedQuickSlot(QuickSlotSelected);
       
    }
    private void Bag()
    {
        if (Input.GetKeyDown("q"))
        {
            BagEnabled = !BagEnabled;
        }
        if (BagEnabled == true)
        {
            BagCanvas.SetActive(true);
        }
        else
        {
            BagCanvas.SetActive(false);
            ItemMenu.SetActive(false);
        }

        if (Input.GetButtonDown("Cancel") && ItemMenu.activeSelf == false)
        {
            BagEnabled = false;
        }
    }
    public void DropItem()
    {
        int slot_id = ItemMenu.GetComponent<ItemMenu>().slot_id;
        if (slot[slot_id].GetComponent<Slot>().empty == false)
        {
            int item_id = slot[slot_id].transform.GetChild(0).Find("ItemObject").GetChild(0).gameObject.GetComponent<Item>().itemID;
            Rigidbody item = GM.item[item_id].GetComponent<Rigidbody>();
            Rigidbody clone = Instantiate(item, Forward_position.position, transform.rotation);
            clone.gameObject.name = item.gameObject.name;
            clone.gameObject.SetActive(true);
            clone.velocity = transform.TransformDirection(Vector3.forward * 5);
            slot[slot_id].GetComponent<Slot>().itemCount -= 1;
            if (slot[slot_id].GetComponent<Slot>().itemCount < 1)
            {
                Destroy(slot[slot_id].transform.GetChild(0).Find("ItemObject").GetChild(0).gameObject);
                slot[slot_id].GetComponent<Slot>().ResetSlot();
            }

            if (item.GetComponent<Item>().itemType == "Weapon")
            {
                int qslot = CheckQuickSlot(item.GetComponent<Item>().itemID);
                if (qslot < QuickSlot.Length)
                {
                    QuickSlot[qslot].GetComponent<Slot>().ResetSlot();
                }

                if (Equipped == item.GetComponent<Item>().itemID)
                {
                    SwitchWeapon(-1);
                }
            }
            BagEnabled = false;
        }

    }
    private void GetAllSlotItem()
    {
        if(BagCanvas.activeSelf == false)
        {
            BagCanvas.SetActive(true);
        }
        slot = new GameObject[AllSlot];
        for(int i = 0; i < AllSlot; i++)
        {
            slot[i] = SlotHolder.transform.GetChild(i).gameObject;
            if(slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
       
    }
    private int CheckSameItem(int id)
    {
        for (int i = 0; i < AllSlot; i++)
        {
            slot[i] = SlotHolder.transform.GetChild(i).gameObject;
            if (slot[i].GetComponent<Slot>().empty == false)
            {
                if(slot[i].GetComponent<Slot>().itemID == id)
                {
                    return i;
                }
            }
        }
        return 99;
    }
    public void SetEquipped()
    {
        int slot_id = ItemMenu.GetComponent<ItemMenu>().slot_id;
        if (slot[slot_id].GetComponent<Slot>().empty == false)
        {
            Item item = slot[slot_id].transform.GetChild(0).Find("ItemObject").GetChild(0).gameObject.GetComponent<Item>();
            if(item.itemType == "Weapon")
            {
                CopyToQuickSlot(slot_id);
                
            }
            else
            {
                return;
            }
        }
        BagEnabled = false;
    }

    //QuickSlot
    private void QuickSlotController()
    {
        SwitchQuickSlot();
    }
    private void GetAllQuickSlot()
    {
        QuickSlot = new GameObject[AllQuickSlotHolder];
        for (int i = 0; i < AllQuickSlotHolder; i++)
        {
            QuickSlot[i] = QuickSlotHolder.transform.GetChild(i).gameObject;
            if (QuickSlot[i].GetComponent<Slot>().item == null)
            {
                QuickSlot[i].GetComponent<Slot>().empty = true;
            }
        }
        QuickSlotSelected = 0;
    }
    public void CopyToQuickSlot(int slot_id)
    {

        Item item = slot[slot_id].transform.GetChild(0).Find("ItemObject").GetChild(0).gameObject.GetComponent<Item>();
        int check = CheckQuickSlot(item.itemID);
        if (check > QuickSlot.Length)
        {
            for (int i = 0; i < AllQuickSlotHolder; i++)
            {
                if (QuickSlot[i].GetComponent<Slot>().empty)
                {
                    slot[slot_id].GetComponent<Slot>().Equipped = true;
                    QuickSlot[i].GetComponent<Slot>().ItemIcon = item.itemIcon;
                    QuickSlot[i].GetComponent<Slot>().itemID = item.itemID;
                    QuickSlot[i].GetComponent<Slot>().itemType = item.itemType;
                    QuickSlot[i].GetComponent<Slot>().itemDescription = item.itemDescription;
                    QuickSlot[i].GetComponent<Slot>().empty = false;
                    QuickSlot[i].GetComponent<Slot>().itemCount += 1;
                    QuickSlot[i].GetComponent<Slot>().UpdateSlot();
                    if (i == QuickSlotSelected)
                    {
                        KeySwtichWeapon(i);
                    }
                    return;
                }
            }
        }
        else
        {
            return;
        }

    }
    private void SwitchQuickSlot()
    {
        if (CanMove)
        {
            if (Input.GetKeyDown("1"))
            {
                KeySwtichWeapon(0);
                QuickSlotSelected = 0;
                
            }
            if (Input.GetKeyDown("2"))
            {
                KeySwtichWeapon(1);
                QuickSlotSelected = 1;
              
            }
            if (Input.GetKeyDown("3"))
            {
                KeySwtichWeapon(2);
                QuickSlotSelected = 2;
               
            }
            if (Input.GetKeyDown("4"))
            {
                KeySwtichWeapon(3);
                QuickSlotSelected = 3;
               
            }
            if (Input.GetKeyDown("5"))
            {
                KeySwtichWeapon(4);
                QuickSlotSelected = 4;
               
            }
            if (Input.GetKeyDown("6"))
            {
                KeySwtichWeapon(5);
                QuickSlotSelected = 5;
              
            }
            if (Input.GetKeyDown("7"))
            {
                KeySwtichWeapon(6);
                QuickSlotSelected = 6;
              
            }
            if (Input.GetKeyDown("8"))
            {
                KeySwtichWeapon(7);
                QuickSlotSelected = 7;
              
            }
            if (Input.GetKeyDown("9"))
            {
                KeySwtichWeapon(8);
                QuickSlotSelected = 8;
                
            }
        }
    }
    public void ClickSwitchQuickSlot(int slot_id)
    {
        if (CanMove)
        {
            KeySwtichWeapon(slot_id);
            QuickSlotSelected = slot_id;
        }
    }
    private void KeySwtichWeapon(int slot_id)
    {
        if (QuickSlot[slot_id].GetComponent<Slot>().empty == false)
        {
            int weaponId = QuickSlot[slot_id].GetComponent<Slot>().itemID;
            SwitchWeapon(weaponId);
        }
        else
        {
            SwitchWeapon(-1);
        }
    }
    private void SelectedQuickSlot(int slot_id)
    {
        for (int i = 0; i < AllQuickSlotHolder; i++)
        {
            QuickSlot[i] = QuickSlotHolder.transform.GetChild(i).gameObject;
            QuickSlot[i].GetComponent<Slot>().Selected = false;
        }

        QuickSlot[slot_id].GetComponent<Slot>().Selected = true;
        Selected_Canvas.transform.position = QuickSlot[slot_id].transform.position;
    }
    private int CheckQuickSlot(int id)
    {
        for (int i = 0; i < AllQuickSlotHolder; i++)
        {
            if (QuickSlot[i].GetComponent<Slot>().empty == false)
            {
                if (QuickSlot[i].GetComponent<Slot>().itemID == id)
                {
                    return i;
                }
            }
        }
        return 99;
    }

    //Weapon
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Resource")
        {
            Quaternion Rotation = Quaternion.LookRotation(other.transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, RotationSpeed * Time.deltaTime);
        }
    }
    private void Attack()
    {
        if (Equipped != -1)
        {
            MainWeaponAnim = MainWeapon.transform.GetChild(Equipped).GetComponent<Animator>();
            if (Input.GetKeyDown("f"))
            {
                MainWeaponAnim.SetBool("Attacking", true);
            }
        }
    }
    private void SwitchWeapon(int id)
    {
        Equipped = id;
        if (Equipped != -1)
        {
            DisableAllWeapon();
            MainWeapon.transform.GetChild(Equipped).gameObject.SetActive(true);
        }
        else
        {
            DisableAllWeapon();
        }
    }
    private void DisableAllWeapon()
    {
        for(int i =0;i < AllWeapon.transform.childCount; i++)
        {
            AllWeapon.transform.GetChild(i).gameObject.SetActive(false);
        }
       
    }

    //JoyStick
    public void btn_bag()
    {
        BagEnabled = !BagEnabled;
    }

    public void btn_attack()
    {
        if (Equipped != -1)
        {
            MainWeaponAnim = MainWeapon.transform.GetChild(Equipped).GetComponent<Animator>();
            
            MainWeaponAnim.SetBool("Attacking", true);
            
        }
    }







}
