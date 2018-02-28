using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HarpoonThrowerDescription : UpgradeDescription
{
    [SerializeField]
    private float harpoonSpeed = 2;
    [SerializeField]
    private float cooldown = 5;
    [SerializeField]
    private int harpoonNumber = 0;

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
            new Statistic("Vitesse des harpons", harpoonSpeed),
            new Statistic("Temps de recharge", cooldown),
            new Statistic("Nombre de harpons", harpoonNumber)
        };

        return stats;
    }
}


