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

        [BitMask]
        public CaptureTechnique allowedTechniques = CaptureTechnique.Harpoon | CaptureTechnique.Melee;
    }
}