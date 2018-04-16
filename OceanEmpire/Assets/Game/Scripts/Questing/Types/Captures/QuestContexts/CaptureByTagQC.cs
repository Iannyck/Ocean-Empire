using System;

namespace Questing
{
    [Serializable]
    public class CaptureByTagQC : CaptureQC
    {
        public FishFlags[] observedFishFlags;
        [BitMask]
        public FishFlags fishFlagsValue;
    }
}
