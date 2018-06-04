using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEnterListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public bool isIn = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isIn = false;
    }
}
