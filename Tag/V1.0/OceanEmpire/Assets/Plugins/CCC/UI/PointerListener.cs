using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PointerListener : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    public UnityEvent onClick = new UnityEvent();
    public UnityEvent onEnter = new UnityEvent();
    public UnityEvent onExit = new UnityEvent();
    public UnityEvent onUp = new UnityEvent();
    public UnityEvent onDown = new UnityEvent();

    public bool isDragging = false;
    public bool isIn = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        onDown.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
        onEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isIn = false;
        onExit.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        onUp.Invoke();
    }
}
