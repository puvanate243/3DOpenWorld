using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLog : MonoBehaviour
{
    GameObject[] txt;
    private int txtMax = 9;

    private void Start()
    {
        GetAllText();
    }

    private void GetAllText()
    {
        txt = new GameObject[txtMax];
        for(int i = 0; i < txtMax;i++)
        {
            txt[i] = transform.GetChild(i).gameObject;
            txt[i].SetActive(false);
        }
    }

   public void AddLog(string name)
   {
        MoveLog();
        txt[0].SetActive(true);
        txt[0].GetComponent<Text>().text = name + "       x1"; 
   }
    private void MoveLog()
    {
        for(int i = 8; i > 0; i--)
        {
            if (txt[i - 1].activeSelf == true)
            {
                txt[i].GetComponent<Text>().text = txt[i - 1].GetComponent<Text>().text;
                txt[i].SetActive(true);
            }
        }

    }

}
