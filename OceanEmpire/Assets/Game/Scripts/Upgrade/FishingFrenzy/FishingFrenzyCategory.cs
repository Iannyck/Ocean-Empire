using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category/Fishing Frenzy Category")]
public class FishingFrenzyCategory : UpgradeCategory<FishingFrenzyDescBuilder, FishingFrenzyDescription>
{
    protected override string OwnedUpgradeKey { get { return "ff1"; } }
    protected override string NextUpgGenCodeKey { get { return "ff2"; } }
    protected override string OwnedUpgGenKey { get { return "ff3"; } }
    protected override string AvailableSaveKey { get { return "ffAvailable"; } }
    protected override bool AvailableByDefault { get { return false; } }

    public override FishingFrenzyDescription GenerateNextDescription(string nextUpgGenCode)
    {
        throw new NotImplementedException();
    }
    public override void MakeNextGenCode(int level) { }
}
