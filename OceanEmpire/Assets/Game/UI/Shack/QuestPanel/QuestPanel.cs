using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Questing;
using DG.Tweening;

public class QuestPanel : MonoBehaviour
{
    [Header("Entries Managment")]
    public QuestPanelEntry entryPrefab;
    public List<QuestPanelEntry> entries = new List<QuestPanelEntry>();
    public RectTransform entriesContainer;

    [Header("Animation")]
    public CanvasGroup canvasGroup;
    public Vector2 arrivalDelta;
    public float arrivalDelay;
    public Ease arrivalEase = Ease.OutSine;
    public float moveDuration = 1.5f;
    public float fadeInDuration = 1.5f;
    public bool showOnStart = true;

    private void Awake()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            UpdateContent();
            QuestManager.Instance.OnListChange += UpdateContent;
        });
    }

    private void Start()
    {
        if (showOnStart)
            Show();
        else
            HideInstant();
    }

    void Show()
    {
        var rectTr = GetComponent<RectTransform>();
        var destination = rectTr.anchoredPosition;
        var sq = DOTween.Sequence();

        canvasGroup.alpha = 0;
        rectTr.anchoredPosition += arrivalDelta;

        sq.AppendInterval(arrivalDelay);
        sq.Append(rectTr.DOAnchorPos(destination, moveDuration).SetEase(arrivalEase));
        sq.Join(canvasGroup.DOFade(1, fadeInDuration));
    }

    void HideInstant()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnListChange -= UpdateContent;
    }

    public void UpdateContent()
    {
        QuestManager questManager = QuestManager.Instance;
        if (questManager == null)
        {
            Debug.LogError("No quest manager");
            return;
        }

        List<Quest> questList = questManager.ongoingQuests;

        int i = 0;
        for (; i < questList.Count; i++)
        {
            var entry = GetEntry(i);
            entry.Fill(questList[i]);
            entry.gameObject.SetActive(true);
        }

        for (; i < entries.Count; i++)
        {
            entries[i].gameObject.SetActive(false);
        }
    }

    private QuestPanelEntry GetEntry(int i)
    {
        while (i >= entries.Count)
        {
            entries.Add(entryPrefab.DuplicateGO(entriesContainer));
        }
        return entries[i];
    }
}
