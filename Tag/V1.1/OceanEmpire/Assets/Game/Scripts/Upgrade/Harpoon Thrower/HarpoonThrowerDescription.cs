using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HarpoonThrowerDescription : UpgradeDescription
{
    public int bonusCoins;
    [SerializeField] float harpoonSpeed = 2;
    [SerializeField]  float cooldown = 5;
    [SerializeField] int harpoonNumber = 0;

    public float GetHarpoonSpeed()
    {
        return harpoonSpeed;
    }
    public float GetCooldown()
    {
        return cooldown;
    }
    public int GetHarpoonNumber()
    {
        return harpoonNumber;
    }
    public TriColoredSprite GetPullSprite()
    {
        return spriteKit.Get(0);
    }
    public TriColoredSprite GetCanonSprite()
    {
        return spriteKit.Get(1);
    }
    public TriColoredSprite GetHarpoonSprite()
    {
        return spriteKit.Get(2);
    }




    public override List<Statistic> GetStatistics()
    {
        List<Statistic> stats = new List<Statistic>
        {
            new Statistic("Vitesse", harpoonSpeed),
            new Statistic("Recharge", cooldown, "s"),
            new Statistic("Bonus", bonusCoins, "$", true)
        };

        return stats;
    }

    //public override string GetStats()
    //{
    //    return "Vitesse: <stat>" + harpoonSpeed
    //        + "</stat>\nRecharge: <stat>" + cooldown + "s</stat>";
    //}
}


