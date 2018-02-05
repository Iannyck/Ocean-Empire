using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Description/Thruster Description")]
public class ThrusterDescription : UpgradeDescription {
    [SerializeField, ReadOnly] private Thruster thruster;
    public Thruster GetThruster() { return thruster;}
}
