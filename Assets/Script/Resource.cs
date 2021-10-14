using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private int Amount = 3;

    public GameObject itemObject;
    //Detail
    public int itemID;
    public string itemType;
    public string itemDescription;
    public Sprite itemIcon;

    public string Target;

    public void ReduceResource(int i)
    {
        if (Amount > 0)
        {
            Amount -= i;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
