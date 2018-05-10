using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    public class FirstQuestsGiver : MonoBehaviour
    {
        [SerializeField] PrebuiltMapData firstMap;
        [SerializeField] DataSaver saver;

        private const string FIRST_QUESTS_GIVEN = "firstQuestsGiven";

        void Start()
        {
            PersistentLoader.LoadIfNotLoaded(() =>
            {
                if (!saver.HasEverLoaded)
                    saver.Load(GiveFirstQuests);
                else
                    GiveFirstQuests();
            });
        }

        void GiveFirstQuests()
        {
            if (saver.GetBool(FIRST_QUESTS_GIVEN))
                return;

            var now = DateTime.Now;
            foreach (var quest in firstMap.GetRelatedQuestBuilders())
            {
                if (quest != null)
                    QuestManager.Instance.AddQuest(quest.BuildQuest(now), false);
            }
            QuestManager.Instance.LateSave();
            saver.SetBool(FIRST_QUESTS_GIVEN, true);
            saver.LateSave();
        }
    }
}
