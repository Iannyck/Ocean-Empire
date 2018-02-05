using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category/Gaz Tank Category")]
public class GazTankCategory : UpgradeCategory<GazTankDescBuilder, GazTankDescription>
{

    public override UpgradeDescription GenerateNextDescription(string nextUpgGenCode)
    {
        return null;
    }
}
