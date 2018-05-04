using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public class BuyThisQC : QuestContext
    {
        [AssetName]
        public string shopCategory;
        public int upgradeLevelToReach = 1;
    }
}