using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenu : MonoBehaviour
{
    GameObject GFX;

    public int slot_id;

    private void Start()
    {
        GFX = transform.GetChild(0).gameObject;
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RectTransform rectTransform = GFX.GetComponent<RectTransform>();
            Canvas canvas = GetComponent<Canvas>();
            Camera camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main;
            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, camera))
            {
                gameObject.SetActive(false);
            }
         
        }

    }

    public void DisableMenu()
    {
        gameObject.SetActive(false);
    }


}
