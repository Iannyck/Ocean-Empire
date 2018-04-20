using System;
using System.Collections.Generic;

namespace Questing
{
    [Serializable]
    public class CaptureByIdQC : CaptureQC
    {
        public List<FishId> fishIds;
    }
}
