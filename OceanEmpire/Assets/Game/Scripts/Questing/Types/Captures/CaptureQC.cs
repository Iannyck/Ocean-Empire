using System;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public abstract class CaptureQC : QuestContext
    {
        [Header("Capture")]
        public int captureGoal;
    }
}