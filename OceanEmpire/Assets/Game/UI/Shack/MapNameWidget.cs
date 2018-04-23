using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNameWidget : MonoBehaviour
{
    [Header("References")]
    public Text textComp;
    public MapManager mapManager;

    [Header("Animation Settings")]
    public float fadeInDuration = 0.5f;
    public float pauseDuration = 0.75f;
    public float fadeOutDuration = 0.75f;
    public float scaleStart = 1.1f;
    public float scaleEnd = 0.93f;

    private Tween currentAnim;

    void Start()
    {
        FillContent(mapManager.MapData);
        ShowAndHide();
        mapManager.OnMapSet += OnMapSet;
    }

    private void OnMapSet(int arg1, MapData mapData)
    {
        FillContent(mapData);
        ShowAndHide();
    }

    void FillContent(MapData mapData)
    {
        textComp.text = mapData != null ? mapData.Name : "";
    }

    void ShowAndHide()
    {
        if (currentAnim != null)
        {
            currentAnim.Kill();
        }

        var tr = textComp.rectTransform;
        tr.localScale = Vector3.one * scaleStart;
        textComp.color = textComp.color.ChangedAlpha(0);
        textComp.gameObject.SetActive(true);

        Sequence sq = DOTween.Sequence();
        sq.Join(textComp.DOFade(1, fadeInDuration));
        sq.AppendInterval(pauseDuration);
        sq.Append(textComp.DOFade(0, fadeOutDuration));
        sq.Insert(0, tr.DOScale(scaleEnd, fadeInDuration + pauseDuration + fadeOutDuration));
        sq.onComplete = () =>
         {
             if (textComp != null)
                 textComp.gameObject.SetActive(false);
         };
    }
}
