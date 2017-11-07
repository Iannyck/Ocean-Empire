using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class CalendarScroll_Scroller : MonoBehaviour
{
    public ScrollRect scroller;

    private void Start()
    {
        scroller.onValueChanged.AddListener(OnScrollValueChanged);
    }

    void OnScrollValueChanged(Vector2 normalizedPosition)
    {
        print(normalizedPosition);
        if(normalizedPosition.y < -1)
        {
            scroller.verticalNormalizedPosition = 0;
            //scroller.velocity
            Debug.LogWarning("sup");
        }
    }

    public void NormalizedPositionToZero()
    {
        scroller.verticalNormalizedPosition = 0;
    }
}
