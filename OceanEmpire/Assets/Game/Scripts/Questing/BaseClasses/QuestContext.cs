using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    /// <summary>
    /// Toute les information statique de la quest (qui ne changera pas avec le temps)
    /// </summary>
    [Serializable]
    public class QuestContext
    {
        [Header("Quest")]
        public string description;
        public virtual QuestReward Reward { get { return null; } }

        //[BitMask, ReadOnly]
        //public TrackingFlags trackingFlags;
    }
}