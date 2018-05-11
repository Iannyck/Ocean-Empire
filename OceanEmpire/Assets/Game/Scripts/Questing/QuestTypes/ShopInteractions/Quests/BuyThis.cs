using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [System.Serializable]
    public class BuyThis : Quest<BuyThisQC>
    {
        [System.NonSerialized]
        private UpgradeCategory upgradeCategory;

        public override string GetDisplayedProgressText()
        {
            FetchShopCategory();
            return upgradeCategory.GetCurrentLevel() + " / " + context.upgradeLevelToReach;
        }

        public override float GetProgress01()
        {
            FetchShopCategory();
            return Mathf.Clamp01(upgradeCategory.GetCurrentLevel() / (float)context.upgradeLevelToReach);
        }

        public override void Launch()
        {
            state = QuestState.Ongoing;
            FetchShopCategory();
        }

        void FetchShopCategory()
        {
            Object shopCategory = PersistentLoader.instance.persistentObjects.Find((o) => o.name == context.shopCategory);
            if (!shopCategory)
            {
                Debug.LogError("Failed to fetch the \"" + context.shopCategory + "\" upgrade category");
            }
            if (shopCategory is UpgradeCategory)
            {
                upgradeCategory = (UpgradeCategory)shopCategory;
            }
            else
            {
                Debug.LogError("The object \"" + shopCategory.name + "\" is not of type UpgradeCategory as expected");
            }
        }

        public override QuestState UpdateState()
        {
            if(state == QuestState.Ongoing)
            {
                if(upgradeCategory != null)
                {
                    if (upgradeCategory.GetCurrentLevel() >= context.upgradeLevelToReach)
                    {
                        // Complete !
                        Complete();
                    }
                }
                else
                {
                    FetchShopCategory();
                }
            }
            return state;
        }
    }
}