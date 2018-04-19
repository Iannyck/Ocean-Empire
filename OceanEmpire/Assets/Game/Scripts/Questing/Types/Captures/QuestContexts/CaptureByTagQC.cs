using System;

namespace Questing
{
    [Serializable]
    public class CaptureByTagQC : CaptureQC
    {
        [BitMask]
        public FishFlags flagsFilter;
        [BitMask]
        public FishFlags flagsValue;
    }
}
