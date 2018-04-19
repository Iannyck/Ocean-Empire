using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public class CaptureById : BaseCaptureQuest<CaptureByIdQC>
    {
        protected override bool IsValidFishCapture(Capturable capturable)
        {
            return context.fishIds.Contains(capturable.info.description.id);
        }
    }
}
