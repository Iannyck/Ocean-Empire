using System;

[Flags]
public enum CaptureTechnique
{
    Melee = 1 << 0,
    Harpoon = 1 << 1
}