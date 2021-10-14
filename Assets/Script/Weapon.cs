using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private BoxCollider cd;
    private Animator am;

    //Player
    public GameObject player;
    private PlayerController playerScript;

    public int WeaponID;
    public string WeaponName;
    public string Target;
    public string AnimState;

    public GameObject ItemLog;
    void Start()
    {
        am = GetComponent<Animator>();
        cd = GetComponent<BoxCollider>();
        playerScript = player.GetComponent<PlayerController>();
    }

    public void ReturnAttack()
    {
        am.SetBool("Attacking", false);
        playerScript.CanMove = true;
        cd.enabled = false;
    }

    public void StartAttack()
    {
        playerScript.CanMove = false;
        cd.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Resource" && am.GetCurrentAnimatorStateInfo(0).IsName(AnimState))
        {
            Resource resource = other.gameObject.GetComponent<Resource>();
            if (resource.Target == WeaponName)
            {
                resource.ReduceResource(1);
                GameObject Clone = Instantiate(resource.itemObject);
                playerScript.AddItem(Clone,resource.itemID,resource.itemType,resource.itemDescription,resource.itemIcon);
                ItemLog.GetComponent<ItemLog>().AddLog(resource.itemDescription);
            }
          
            cd.enabled = false;
        }
    }
}
