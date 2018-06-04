using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundPlayer_PointerListener : SoundPlayer,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerUpHandler
{
    [Header("Pointer Events")]
    public bool pointerClick = false;
    public bool pointerEnter = false;
    public bool pointerExit = false;
    public bool pointerDown = false;
    public bool pointerUp = false;



    public void OnPointerClick(PointerEventData eventData)
    {
        if (pointerClick)
            Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pointerDown)
            Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pointerEnter)
            Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (pointerExit)
            Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerUp)
            Play();
    }
}
