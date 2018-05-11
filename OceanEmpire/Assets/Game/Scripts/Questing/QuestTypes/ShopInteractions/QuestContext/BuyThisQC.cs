using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public class BuyThisQC : QuestContext
    {
        [AssetName] public string shopCategory1;
        [AssetName] public string shopCategory2;
        [AssetName] public string shopCategory3;
        [AssetName] public string shopCategory4;
        public int upgradeLevelToReach = 1;
    }
}