using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonThrowerDescription : UpgradeDescription {
    [SerializeField, ReadOnly] private HarpoonThrower harpoonThrower;
    public HarpoonThrower GetHarpoonThrower() { return harpoonThrower; }
}
