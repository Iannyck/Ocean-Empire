using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GazTankDescription : UpgradeDescription {
    [SerializeField, ReadOnly] private GazTank gazTank;
    public GazTank GetGazTank() { return gazTank; }
}