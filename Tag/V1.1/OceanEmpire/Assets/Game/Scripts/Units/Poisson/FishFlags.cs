using System;

[Flags, Serializable]
public enum FishFlags
{
    MeleeCapturable = 1 << 0,
    Harpoonable = 1 << 1,
    Electric = 1 << 2,
    Rare = 1 << 3,
    DeepDweller = 1 << 4,
    Animal = 1 << 5,
    Capturable = 1 << 6,
}