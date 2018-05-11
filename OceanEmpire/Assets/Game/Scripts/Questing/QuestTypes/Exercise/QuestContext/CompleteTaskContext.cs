using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public class CompleteTaskContext : QuestContext
    {
        public int completeCount;

        [Header("PAS ENCORE TESTÉ")]
        public bool inARow;
    }
}