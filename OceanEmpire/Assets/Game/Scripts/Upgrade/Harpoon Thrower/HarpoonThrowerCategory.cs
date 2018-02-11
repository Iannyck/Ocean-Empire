using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category/Harpoon Thrower Category")]
public class HarpoonThrowerCategory : UpgradeCategory<HarpoonThrowerDescBuilder, HarpoonThrowerDescription>
{

    public override UpgradeDescription GenerateNextDescription(string nextUpgGenCode)
    {
        return null;
    }
    public override void MakeNextGenCode(int level) { }
}
