using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public class CaptureByTag : BaseCaptureQuest<CaptureByTagQC>
    {
        protected override bool IsValidFishCapture(Capturable capturable)
        {
            FishFlags expectedResult = context.flagsValue & context.flagsFilter;
            FishFlags result = capturable.info.description.flags & context.flagsFilter;

            return expectedResult == result;
        }
    }
}
