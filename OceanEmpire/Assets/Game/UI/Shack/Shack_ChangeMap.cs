using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Questing;

public class Shack_ChangeMap : MonoBehaviour
{
    public QuestPanel questPanel;

    public bool IsTransitioning { get; private set; }

    public void TransitionToNextMap()
    {
        if (IsTransitioning)
            return;
        IsTransitioning = true;

        CoroutineLauncher.Instance.DelayedCall(() =>
        {
            questPanel.HideInstant();
            QuestManager.Instance.RemoveAllQuests();
            MapManager.Instance.SetMap_Next(true);

            CoroutineLauncher.Instance.DelayedCall(() =>
            {
                questPanel.Show();
            }, 1);
        }, 0.5f);
    }
}
