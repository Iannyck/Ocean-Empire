using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category/Harpoon Thrower Category")]
public class HarpoonThrowerCategory : UpgradeCategory<HarpoonThrowerDescBuilder, HarpoonThrowerDescription>
{
    protected override string OwnedUpgradeKey { get { return "ht1"; } }
    protected override string NextUpgGenCodeKey { get { return "ht2"; } }
    protected override string OwnedUpgGenKey { get { return "ht3"; } }
    protected override string AvailableSaveKey { get { return "htAvailable"; } }


    public override HarpoonThrowerDescription GenerateNextDescription(string nextUpgGenCode)
    {
        return null;
    }
    public override void MakeNextGenCode(int level) { }
}
