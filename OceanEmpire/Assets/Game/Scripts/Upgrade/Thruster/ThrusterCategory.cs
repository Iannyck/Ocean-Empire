using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category/Thruster Category")]
public class ThrusterCategory : UpgradeCategory<ThrusterDescBuilder, ThrusterDescription>
{
    public override UpgradeDescription GenerateNextDescription(string nextUpgGenCode)
    {
        return null;
    }
}
