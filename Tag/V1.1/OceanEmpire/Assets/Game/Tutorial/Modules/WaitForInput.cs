using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForInput : MonoBehaviour
{
    private struct Order
    {
        public bool useTouch;
        public KeyCode[] keyCodes;
        public TouchPhase touchType;
        public Action callback;
    }

    private List<Order> orders = new List<Order>();

    public void OnKeyDown(Action callback, params KeyCode[] keyCodes)
    {
        orders.Add(new Order() { keyCodes = keyCodes, callback = callback, useTouch = false });
    }
    public void OnAnyKeyDown(Action callback)
    {
        orders.Add(new Order() { keyCodes = null, callback = callback, useTouch = false });
    }
    public void OnTouch(Action callback, TouchPhase phase)
    {
        orders.Add(new Order() { keyCodes = null, callback = callback, useTouch = true, touchType = phase });
    }

    void Update()
    {
        int count = orders.Count;
        for (int i = 0; i < count; i++)
        {
            if (orders[i].useTouch)
            {
                if(Input.touchCount > 0) // ANDROID
                {
                    if (Input.GetTouch(1).phase == orders[i].touchType)
                    {
                        orders[i].callback();
                        orders.RemoveAt(i);
                        i--;
                        count--;
                    }
                } else if(Input.GetMouseButtonDown(0)) // PC
                {
                    orders[i].callback();
                    orders.RemoveAt(i);
                    i--;
                    count--;
                }
            } else if(orders[i].keyCodes == null)
            {
                if (Input.anyKeyDown)
                {
                    orders[i].callback();
                    orders.RemoveAt(i);
                    i--;
                    count--;
                }
            }
            else
            {
                for (int u = 0; u < orders[i].keyCodes.Length; u++)
                {
                    if (Input.GetKeyDown(orders[i].keyCodes[u]))
                    {
                        orders[i].callback();
                        orders.RemoveAt(i);
                        i--;
                        count--;
                        break;
                    }
                }
            }
        }
    }
}
