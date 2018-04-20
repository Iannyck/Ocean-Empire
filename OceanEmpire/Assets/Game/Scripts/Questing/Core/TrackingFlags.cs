using System;

[Flags, Serializable]
public enum TrackingFlags
{
    Recolte = 1 << 0,
    Shop = 1 << 1,
    // Etc = 1 << 2,
    // Etc = 1 << 3
}
