using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public class BuyThis : Quest<BuyThisQC>
    {
        [NonSerialized] UpgradeCategory upgradeCategory1;
        [NonSerialized] UpgradeCategory upgradeCategory2;
        [NonSerialized] UpgradeCategory upgradeCategory3;
        [NonSerialized] UpgradeCategory upgradeCategory4;

        public override string GetDisplayedProgressText()
        {
            FetchShopCategories();
            return GetLevel() + " / " + context.upgradeLevelToReach;
        }

        public override float GetProgress01()
        {
            FetchShopCategories();
            return Mathf.Clamp01(GetLevel() / (float)context.upgradeLevelToReach);
        }

        public override void Launch()
        {
            state = QuestState.Ongoing;
            FetchShopCategories();
        }

        public override QuestState UpdateState()
        {
            if (state == QuestState.Ongoing)
            {
                FetchShopCategories();
                if (GetLevel() >= context.upgradeLevelToReach)
                {
                    // Complete !
                    Complete();
                }
            }
            return state;
        }

        int GetLevel()
        {
            int cumul = 0;
            if (upgradeCategory1)
                cumul += upgradeCategory1.GetCurrentLevel();
            if (upgradeCategory2)
                cumul += upgradeCategory2.GetCurrentLevel();
            if (upgradeCategory3)
                cumul += upgradeCategory3.GetCurrentLevel();
            if (upgradeCategory4)
                cumul += upgradeCategory4.GetCurrentLevel();
            return cumul;
        }

        void FetchShopCategories()
        {
            if (upgradeCategory1 == null)
                upgradeCategory1 = FetchShopCategory(context.shopCategory1);
            if (upgradeCategory2 == null)
                upgradeCategory2 = FetchShopCategory(context.shopCategory2);
            if (upgradeCategory3 == null)
                upgradeCategory3 = FetchShopCategory(context.shopCategory3);
            if (upgradeCategory4 == null)
                upgradeCategory4 = FetchShopCategory(context.shopCategory4);
        }

        static UpgradeCategory FetchShopCategory(string name)
        {
            if (name == "")
                return null;

            UnityEngine.Object shopCategory = PersistentLoader.instance.persistentObjects.Find((o) => o.name == name);
            if (!shopCategory)
            {
                Debug.LogError("Failed to fetch the \"" + name + "\" upgrade category");
                return null;
            }
            if (shopCategory is UpgradeCategory)
            {
                return (UpgradeCategory)shopCategory;
            }
            else
            {
                Debug.LogError("The object \"" + shopCategory.name + "\" is not of type UpgradeCategory as expected");
                return null;
            }
        }
    }
}