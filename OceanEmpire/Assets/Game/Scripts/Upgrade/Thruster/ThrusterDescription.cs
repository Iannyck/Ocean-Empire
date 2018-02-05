using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterDescription : UpgradeDescription {
    [SerializeField, ReadOnly] private Thruster thruster;
    public Thruster GetThruster() { return thruster;}
}
