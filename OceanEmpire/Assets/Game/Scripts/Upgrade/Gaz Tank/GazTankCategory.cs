using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category/Gaz Tank Category")]
public class GazTankCategory : UpgradeCategory<GazTankDescBuilder, GazTankDescription>
{
    protected override string OwnedUpgradeKey { get { return "gt1"; } }
    protected override string NextUpgGenCodeKey { get { return "gt2"; } }
    protected override string OwnedUpgGenKey { get { return "gt3"; } }
    protected override string AvailableSaveKey { get { return "gtAvailable"; } }

    public override GazTankDescription GenerateNextDescription(string nextUpgGenCode)
    {
        return null;
    }

    public override void MakeNextGenCode(int level) { }
}
