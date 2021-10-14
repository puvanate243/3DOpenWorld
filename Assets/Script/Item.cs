using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //Detail
    public int itemID;
    public string itemType;
    public string itemDescription;
    public Sprite itemIcon;
  
    public Vector3 itemScale;

    private void Start()
    {
        itemScale = transform.localScale;
    }

 

}
