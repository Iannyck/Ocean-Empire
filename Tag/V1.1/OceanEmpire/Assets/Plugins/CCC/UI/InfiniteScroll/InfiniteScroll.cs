using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public abstract class InfiniteScroll : MonoBehaviour, IDragHandler
{
    public ScrollRect scrollRect;

    [System.Serializable]
    public class RewindEvent : UnityEvent<int> { }

    [SerializeField, ReadOnly]
    protected Vector2 itemSize = Vector2.zero;
    [SerializeField, ReadOnly]
    protected Vector2 viewportSize = Vector2.zero;

    [SerializeField, ReadOnly]
    protected int shownItemsAtATimeX = -1;
    [SerializeField, ReadOnly]
    protected int shownItemsAtATimeY = -1;

    [SerializeField, ReadOnly]
    protected Vector2 deplacementMaxDuScroll = Vector2.zero;

    [SerializeField, ReadOnly]
    protected Vector2 itemSpacing = Vector2.zero;

    PointerEventData lastDragEvent;

    protected virtual void Start()
    {
        if (!IsDataOk())
        {
            Debug.LogWarning("On doit fetch les donnees sur le Calendar Scroller d'abord.");
            gameObject.SetActive(false);
        }
        else
        {
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
        }
    }

    private void OnScrollValueChanged(Vector2 normalizedPosition)
    {
        if (MustRewind(normalizedPosition))
        {
            //Get velocity
            Vector2 velocity = scrollRect.velocity;

            //On kill le drag s'il y a lieu
            if (lastDragEvent != null && lastDragEvent.dragging)
            {
                scrollRect.OnEndDrag(lastDragEvent);
            }

            if (MustRewindVertical(normalizedPosition.y))
                RewindVertical(normalizedPosition.y);

            if (MustRewindHorizontal(normalizedPosition.x))
                RewindHorizontal(normalizedPosition.x);

            //On reanime le drag d'entre les morts s'il y a lieu
            if (lastDragEvent != null && lastDragEvent.dragging)
            {
                scrollRect.OnBeginDrag(lastDragEvent);
            }

            //Re-apply velocity
            scrollRect.velocity = velocity;
        }
    }

    private bool MustRewind(Vector2 np)
    {
        return MustRewindHorizontal(np.x) || MustRewindVertical(np.y);
    }

    private bool MustRewindHorizontal(float npAxis)
    {
        return scrollRect.horizontal && MustRewindAxis(npAxis);
    }

    private bool MustRewindVertical(float npAxis)
    {
        return scrollRect.vertical && MustRewindAxis(npAxis);
    }

    private bool MustRewindAxis(float npAxis)
    {
        return npAxis > 1 || npAxis < 0;
    }

    private void RewindVertical(float normalizedPosition)
    {
        float itemDelta;
        float deplacement = CalculateRewindDelta(itemSpacing.y, itemSize.y, viewportSize.y, deplacementMaxDuScroll.y, shownItemsAtATimeY, out itemDelta);

        int mult = normalizedPosition > 1 ? -1 : 1;

        scrollRect.verticalNormalizedPosition += deplacement * mult;
        OnVerticalRewind((itemDelta * mult).RoundedToInt());
    }

    protected virtual void OnVerticalRewind(int value) { }

    private void RewindHorizontal(float normalizedPosition)
    {
        float itemDelta;
        float deplacement = CalculateRewindDelta(itemSpacing.x, itemSize.x, viewportSize.x, deplacementMaxDuScroll.x, shownItemsAtATimeX, out itemDelta);

        int mult = normalizedPosition > 1 ? -1 : 1;

        scrollRect.horizontalNormalizedPosition += deplacement * mult;
        OnHorizontalRewind((itemDelta * mult).RoundedToInt());
    }
    protected virtual void OnHorizontalRewind(int value) { }

    private float CalculateRewindDelta(float _itemSpacing, float _itemSize, float _viewportSize, float _deplacementMaxDuScroll, int shownItemsAtATime, out float itemDelta)
    {
        float roundedViewportSize = (shownItemsAtATime * (_itemSize + _itemSpacing)) - _itemSpacing;
        float decalage = roundedViewportSize - _viewportSize;
        if (decalage < 0)
            Debug.LogWarning("Mauvais rewind");
        float normalizedDecalage = decalage / _deplacementMaxDuScroll;
        float deplacement = 1 - normalizedDecalage;

        itemDelta = (deplacement * _deplacementMaxDuScroll) / -_itemSize;

        return deplacement;
    }

    public virtual bool IsDataOk()
    {
        if (scrollRect == null)
        {
            return false;
        }
        if (scrollRect.movementType != ScrollRect.MovementType.Unrestricted)
        {
            return false;
        }
        if (scrollRect.horizontalScrollbar != null || scrollRect.verticalScrollbar != null)
        {
            return false;
        }

        return itemSize != Vector2.zero
            && viewportSize != Vector2.zero
            && shownItemsAtATimeX != -1
            && shownItemsAtATimeY != -1
            && deplacementMaxDuScroll != Vector2.zero;
    }

    public virtual void FetchData()
    {
        if (scrollRect == null)
        {
            Debug.LogError("On doit avoir un scroll rect de linker");
            return;
        }
        if (scrollRect.movementType != ScrollRect.MovementType.Unrestricted)
        {
            Debug.LogError("Le type de mouvement du scrollRect doit etre a Unrestricted");
            return;
        }
        if (scrollRect.horizontalScrollbar != null || scrollRect.verticalScrollbar != null)
        {
            Debug.LogError("On ne doit pas avoir de scroll bar");
            return;
        }

        RectTransform viewport = scrollRect.viewport;
        RectTransform content = scrollRect.content;

        //La grosseur du viewport
        viewportSize = viewport.rect.size;

        //La grosseur total du content - le viewport
        deplacementMaxDuScroll = content.rect.size - viewportSize;

        //Espace pris par un enfant
        itemSize = GetItemSize();

        itemSpacing = GetItemSpacing();

        //On calcul combien d'elements sont montrer a la fois
        shownItemsAtATimeX = Mathf.CeilToInt((viewportSize.x + itemSpacing.x) / (itemSize.x + itemSpacing.x));
        shownItemsAtATimeY = Mathf.CeilToInt((viewportSize.y + itemSpacing.y) / (itemSize.y + itemSpacing.y));
    }

    public void OnDrag(PointerEventData eventData)
    {
        lastDragEvent = eventData;
    }

    protected abstract Vector2 GetItemSize();
    protected abstract Vector2 GetItemSpacing();
}