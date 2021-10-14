using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Item Database")]
    public GameObject[] item;

    public static int ConvertInt(string str)
    {
        int i;
        if (int.TryParse(str, out i))
        {
            return i;
        }
        else
        {
            return 0;
        }
    }
}
