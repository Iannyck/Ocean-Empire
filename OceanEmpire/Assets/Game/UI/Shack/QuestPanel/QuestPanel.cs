using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Questing;

public class QuestPanel : MonoBehaviour
{
    public QuestPanelEntry entryPrefab;
    public List<QuestPanelEntry> entries = new List<QuestPanelEntry>();
    public RectTransform entriesContainer;

    private void Awake()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            UpdateContent();
            QuestManager.Instance.OnListChange += UpdateContent;
        });
    }

    private void OnDestroy()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnListChange -= UpdateContent;
    }

    public void UpdateContent()
    {
        QuestManager questManager = QuestManager.Instance;
        if(questManager == null)
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
