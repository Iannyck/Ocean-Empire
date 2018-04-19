using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using System;
using System.Text;

namespace Questing
{
    public class QuestManager : MonoPersistent
    {
        public DataSaver dataSaver;
        [Suffix("seconds")]
        public float saveDirtyQuestsEvery;

        public List<Quest> ongoingQuests = new List<Quest>();
        public static QuestManager Instance { get; private set; }
        public event Action OnListChange;

        private float checkDirtyTimer;
        private const string ONGOINGQUEST_KEY = "ongoingQuests";

        public override void Init(Action onComplete)
        {
            Instance = this;
            dataSaver.OnReassignData += FetchData;
            if (!dataSaver.HasEverLoaded)
                dataSaver.LateLoad(onComplete);
            else
                onComplete();
        }

        void Update()
        {
            ProcessQuests();
            ProcessQuestDirt();
        }

        void ProcessQuests()
        {
            Quest quest;
            for (int i = 0; i < ongoingQuests.Count; i++)
            {
                quest = ongoingQuests[i];

                if (quest.state == QuestState.Abandoned)
                {
                    ongoingQuests.RemoveAt(i);
                    i--;
                    LateSave();
                    Debug.Log("Quest abandoned" + quest.Context.description);
                    continue;
                }

                if (quest.state == QuestState.Completed)
                {
                    continue;
                }

                if (quest.state == QuestState.NotStarted)
                {
                    Debug.Log("Quest launched: " + quest.Context.description);
                    quest.Launch();
                }

                quest.UpdateState();
            }
        }

        public void AddQuest(Quest quest, bool andLateSave = true)
        {
            ongoingQuests.Add(quest);
            if (andLateSave)
                LateSave();

            if (OnListChange != null)
                OnListChange();
        }

        public bool RemoveQuest(Quest quest, bool andLateSave = true)
        {
            if (ongoingQuests.Remove(quest))
            {
                if (andLateSave)
                    LateSave();

                if (OnListChange != null)
                    OnListChange();
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Dirty Quests Managment
        void ProcessQuestDirt()
        {
            // Check if we need to save dirty quests

            var highestDirtyState = GetHighestDirtyState();

            if (highestDirtyState == DirtyState.UrgentDirty)
            {
                LateSave();
            }
            else
            {
                if (checkDirtyTimer >= 0)
                {
                    checkDirtyTimer -= Time.deltaTime;
                    if (checkDirtyTimer < 0)
                    {
                        checkDirtyTimer = saveDirtyQuestsEvery;
                        if (highestDirtyState == DirtyState.Dirty)
                            LateSave();
                    }
                }
            }
        }

        DirtyState GetHighestDirtyState()
        {
            DirtyState record = DirtyState.Clean;
            foreach (var quest in ongoingQuests)
            {
                if (quest.DirtyState > record)
                    record = quest.DirtyState;
            }
            return record;
        }

        void CleanQuests()
        {
            foreach (var quest in ongoingQuests)
            {
                quest.DirtyState = DirtyState.Clean;
            }
        }
        #endregion

        #region Load/Save
        public void Save()
        {
            ShipData();
            dataSaver.Save();
            CleanQuests();
        }

        public void LateSave()
        {
            print("save");
            ShipData();
            dataSaver.LateSave();
            CleanQuests();
        }

        private void ShipData()
        {
            dataSaver.SetObjectClone(ONGOINGQUEST_KEY, ongoingQuests);
        }

        private void FetchData()
        {
            var savedList = dataSaver.GetObjectClone(ONGOINGQUEST_KEY);
            if (savedList != null && savedList is List<Quest>)
            {
                ongoingQuests = (List<Quest>)savedList;
            }
            else
            {
                ongoingQuests = new List<Quest>();
            }

            if (OnListChange != null)
                OnListChange();
        }
        #endregion
    }
}