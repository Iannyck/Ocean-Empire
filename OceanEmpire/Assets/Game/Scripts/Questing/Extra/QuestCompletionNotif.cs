using UnityEngine;

namespace Questing
{
    public class QuestCompletionNotif : MonoBehaviour
    {
        private void OnEnable()
        {
            PersistentLoader.LoadIfNotLoaded(() =>
            {
                QuestManager.Instance.OnQuestComplete += OnQuestComplete;
            });
        }

        private void OnDisable()
        {
            if (QuestManager.Instance != null)
                QuestManager.Instance.OnQuestComplete -= OnQuestComplete;
        }

        private void OnQuestComplete(Quest obj)
        {
            MessagePopup.DisplayMessage("Objectif complété!");
        }
    }
}