using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Questing;
using DG.Tweening;
using System;

public class QuestPanel : MonoBehaviour
{
    [Header("References")]
    public string lastMap;
    public ScriptableActionQueue shackAnimQueue;
    public Shack_CameraController shackCamera;

    [Header("Entries Managment")]
    public QuestPanelEntry entryPrefab;
    public List<QuestPanelEntry> entries = new List<QuestPanelEntry>();
    public RectTransform entriesContainer;

    [Header("Arrival Animation")]
    public CanvasGroup canvasGroup;
    public Vector2 arrivalDelta;
    public float arrivalDelay;
    public Ease arrivalEase = Ease.OutSine;
    public float moveDuration = 1.5f;
    public float fadeInDuration = 1.5f;
    public bool showOnStart = true;

    [Header("Offer Next Map Animation")]
    public Vector2 onm_targetSizeDelta;
    public float onm_sizeDuration;
    public Ease onm_sizeEase;
    public RectTransform onm_button;
    public float onm_buttonScaleDuration;
    public Ease onm_buttonScaleEase;

    private Tween ongoingShowAnimation;
    private Vector2 normalAnchoredPosition;
    private Vector2 normalSize;
    private bool isShown = false;
    private bool isOfferingNextMap = false;
    //private bool isOfferingNextMapComplete = false;

    private void Awake()
    {
        var rectTr = GetComponent<RectTransform>();
        normalAnchoredPosition = rectTr.anchoredPosition;
        normalSize = rectTr.sizeDelta;

        PersistentLoader.LoadIfNotLoaded(() =>
        {
            UpdateContent();
            QuestManager.Instance.OnListChange += UpdateContent;
        });
    }

    private void Start()
    {
        onm_button.gameObject.SetActive(false);

        if (showOnStart)
        {
            if (!isShown)
                Show();
        }
        else
            HideInstant();
    }

    public void Show()
    {
        this.DOKill();

        gameObject.SetActive(true);

        isShown = true;
        var rectTr = GetComponent<RectTransform>();
        var sq = DOTween.Sequence();

        canvasGroup.alpha = 0;
        rectTr.sizeDelta = normalSize;
        rectTr.anchoredPosition = normalAnchoredPosition + arrivalDelta;

        sq.AppendInterval(arrivalDelay);
        sq.Append(rectTr.DOAnchorPos(normalAnchoredPosition, moveDuration).SetEase(arrivalEase));
        sq.Join(canvasGroup.DOFade(1, fadeInDuration));
        sq.SetId(this);
        sq.onComplete = CheckIfShouldOfferNextMap;
        ongoingShowAnimation = sq;
    }

    public void OfferNextMap()
    {
        if (this == null || onm_button == null || isOfferingNextMap)
            return;
        isOfferingNextMap = true;
        TweenCallback onCompletion = null;

        // On met l'animation dans une file d'attente
        shackAnimQueue.ActionQueue.AddAction(() =>
        {
            this.DOKill();

            TweenCallback onAllEntriesGone = () =>
            {
                if (this == null || onm_button == null)
                    return;

                var sq = DOTween.Sequence();
                onm_button.localScale = Vector3.one * 0.5f;

                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
                // Resize
                sq.Append(GetComponent<RectTransform>().DOSizeDelta(onm_targetSizeDelta, onm_sizeDuration).SetEase(onm_sizeEase));

                // Enable button
                sq.AppendCallback(() => onm_button.gameObject.SetActive(true));

                // Scale up button
                var scaleUp = onm_button.DOScale(1, onm_buttonScaleDuration).SetEase(onm_buttonScaleEase);
                scaleUp.easeOvershootOrAmplitude = 0;
                sq.Append(scaleUp);

                sq.SetId(this);
                sq.onComplete = () =>
                {
                    //isOfferingNextMapComplete = true;
                    onCompletion();
                };
            };

            if (entries.Count > 0)
            {
                entries[0].FlashAwayAnimation(onAllEntriesGone);
                for (int i = 1; i < entries.Count; i++)
                {
                    entries[i].FlashAwayAnimation();
                }
            }
            else
            {
                onAllEntriesGone();
            }
        }, 0, out onCompletion);
    }

    public void HideInstant()
    {
        this.DOKill();
        isShown = false;
        gameObject.SetActive(false);
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

        CheckIfShouldOfferNextMap();
    }

    void CheckIfShouldOfferNextMap()
    {
        var shouldOfferNextMap = IsEveryOngoingQuestCompleted()
            && MapManager.Instance.MapData.Name != lastMap
            && isShown
            && (!ongoingShowAnimation.IsActive() || ongoingShowAnimation.IsComplete())
            && (shackCamera == null || shackCamera.CurrentSection == Shack_CameraController.Section.Hub);
        var shouldOMNButtonBeVisibleRightNow = shouldOfferNextMap && isOfferingNextMap;

        if (shouldOfferNextMap && !shouldOMNButtonBeVisibleRightNow)
        {
            OfferNextMap();
        }
        else
        {
            // pour couvrir les moments où on update ET qu'on est déjà entrain de montrer le bouton
            onm_button.gameObject.SetActive(shouldOMNButtonBeVisibleRightNow);
        }
    }

    private void OnDestroy()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnListChange -= UpdateContent;
    }

    private bool IsEveryOngoingQuestCompleted()
    {
        foreach (var quest in QuestManager.Instance.ongoingQuests)
        {
            if (quest.state != QuestState.Completed)
                return false;
        }
        return true;
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
