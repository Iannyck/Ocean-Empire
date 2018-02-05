using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Description/Harpoon Thrower Description")]
public class HarpoonThrowerDescription : UpgradeDescription {
    [SerializeField, ReadOnly] private HarpoonThrower harpoonThrower;
    public HarpoonThrower GetHarpoonThrower() { return harpoonThrower; }
}
