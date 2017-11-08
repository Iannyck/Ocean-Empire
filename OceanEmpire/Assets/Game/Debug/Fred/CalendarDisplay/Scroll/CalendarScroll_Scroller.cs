using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CalendarScroll_Scroller : MonoBehaviour, IDragHandler
{
    public ScrollRect scroller;
    public VerticalLayoutGroup containerLayoutGroup;

    [SerializeField, ReadOnly]
    private float itemSize = -1;
    [SerializeField, ReadOnly]
    float viewportSize = -1;
    [SerializeField, ReadOnly]
    int shownItemsAtATime = -1;
    [SerializeField, ReadOnly]
    float deplacementMaxDuScroll = -1;

    PointerEventData lastDragEvent;

    private void Start()
    {
        if (!HasFetchedData())
        {
            Debug.LogWarning("On doit fetch les donnees sur le Calendar Scroller d'abord.");
            gameObject.SetActive(false);
        }
        else
        {
            scroller.onValueChanged.AddListener(OnScrollValueChanged);
        }
    }

    void OnScrollValueChanged(Vector2 normalizedPosition)
    {
        if (normalizedPosition.y < 0)
        {
            Vector2 velocity = scroller.velocity;

            float a = (shownItemsAtATime * itemSize);
            if (containerLayoutGroup != null)
                a -= containerLayoutGroup.spacing;

            float decalage = a - viewportSize;
            float normalizedDecalage = decalage.Abs() / deplacementMaxDuScroll;

            //On kill le drag s'il y a lieu
            if (lastDragEvent.dragging)
                scroller.OnEndDrag(lastDragEvent);

            scroller.verticalNormalizedPosition = 1 - normalizedDecalage;

            //On reanime le drag d'entre les morts s'il y a lieu
            if (lastDragEvent.dragging)
                scroller.OnBeginDrag(lastDragEvent);

            //Re-apply velocity
            scroller.velocity = velocity;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        lastDragEvent = eventData;
    }

    public void NormalizedPositionToZero()
    {
        scroller.verticalNormalizedPosition = 0;
    }

    public bool HasFetchedData()
    {
        return itemSize != -1 && deplacementMaxDuScroll != -1 && viewportSize != -1 && shownItemsAtATime != -1;
    }

    public void FetchedData()
    {
        RectTransform viewport = scroller.viewport;
        RectTransform content = scroller.content;

        //La grosseur du viewport
        viewportSize = viewport.rect.size.y;

        //La grosseur total du content - le viewport
        deplacementMaxDuScroll = content.rect.size.y - viewportSize;

        //On calcul l'espace pris pas 1 enfant (1 jour du calendrier)
        RectTransform child0 = content.GetChild(0) as RectTransform;
        RectTransform child1 = content.GetChild(1) as RectTransform;
        itemSize = (child1.anchoredPosition.y - child0.anchoredPosition.y).Abs();

        //On calcul combien d'element est montrer a la fois
        shownItemsAtATime = Mathf.CeilToInt(viewportSize / itemSize);
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(CalendarScroll_Scroller))]
public class CalendarScroll_ScrollerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CalendarScroll_Scroller scroller = target as CalendarScroll_Scroller;


        Color guiColor = GUI.color;


        if (!scroller.HasFetchedData())
            GUI.color = Color.red;

        if (GUILayout.Button("Fetch layout data"))
        {
            scroller.FetchedData();
            EditorSceneManager.MarkSceneDirty(scroller.gameObject.scene);
        }



        GUI.color = guiColor;
    }
}
#endif
