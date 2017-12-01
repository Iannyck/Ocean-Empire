using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragThreashold : PublicSingleton<DragThreashold>
{
    public EventSystem eventSystem;

    [Header("Pourcentage d'ecran")]
    public float inMenuDragSize = 0.01f;
    public float inGameDragSize = 0.001f;

    public enum DragType { InGame = 0, InMenu = 1 }

    protected override void Awake()
    {
        base.Awake();
        SetDragType(DragType.InMenu);
    }

    public void SetDragType(DragType type)
    {
        int pixels = 0;
        float size = 0;
        switch (type)
        {
            case DragType.InGame:
                size = inGameDragSize;
                break;
            case DragType.InMenu:
                size = inMenuDragSize;
                break;
        }
        pixels = (size * Screen.width).RoundedToInt();
        eventSystem.pixelDragThreshold = pixels;
    }
}