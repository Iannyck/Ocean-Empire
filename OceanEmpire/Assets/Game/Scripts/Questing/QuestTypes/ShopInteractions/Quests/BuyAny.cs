using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Questing
{
    [Serializable]
    public class BuyAny : Quest<BuyAnyQC>
    {
        public int progress;

        [NonSerialized] bool listening = false;
        [NonSerialized] Shop2UI shop;

        public override string GetDisplayedProgressText()
        {
            return progress + " / " + context.upgradeCount;
        }

        public override float GetProgress01()
        {
            return ((float)progress) / context.upgradeCount;
        }

        public override void Launch()
        {
            state = QuestState.Ongoing;
            VerifyGoal();
        }

        public override QuestState UpdateState()
        {
            if(state == QuestState.Ongoing)
            {
                VerifyListening();
            }
            return state;
        }

        private void Shop_OnAnyUpgrade(UpgradeCategory obj)
        {
            progress++;
            VerifyGoal();
            DirtyState = DirtyState.UrgentDirty;
        }

        void VerifyGoal()
        {
            if (progress >= context.upgradeCount)
            {
                Complete();
            }
        }

        bool VerifyListening()
        {
            if (shop == null)
                listening = false;

            if (listening)
            {
                return true;
            }
            else
            {
                if (FetchShop())
                {
                    shop.OnBuyAnyUpgrade += Shop_OnAnyUpgrade;
                    listening = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        bool FetchShop()
        {
            if (shop != null)
                return true;

            if (!Scenes.IsActive("Shop2"))
                return false;

            shop = Scenes.GetActive("Shop2").FindRootObject<Shop2UI>();
            return shop != null;
        }
    }
}