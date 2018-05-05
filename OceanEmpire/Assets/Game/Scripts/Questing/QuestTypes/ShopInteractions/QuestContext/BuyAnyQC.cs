using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [System.Serializable]
    public class BuyAnyQC : QuestContext
    {
        public int upgradeCount;
        public bool useFilter;

        // NB: Yes, it would be way cleaner to use an array, but in this case, I want to use the AssetName attributeDrawer
        [AssetName]
        public string filterOne;
        [AssetName]
        public string filterTwo;
        [AssetName]
        public string filterThree;
    }
}