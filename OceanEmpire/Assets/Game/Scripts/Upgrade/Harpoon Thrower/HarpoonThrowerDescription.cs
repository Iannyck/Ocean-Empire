using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HarpoonThrowerDescription : UpgradeDescription {
    [SerializeField, ReadOnly] private HarpoonThrower harpoonThrower;
    public HarpoonThrower GetHarpoonThrower() { return harpoonThrower; }

    [SerializeField]
    private float harpoonSpeed = 2;
    [SerializeField]
    private float cooldown = 5;
    [SerializeField]
    private int harpoonNumber = 0;

    public float GetHarpoonSpeed()  {
        return harpoonSpeed;
    }
    public float GetCooldown()   {
        return cooldown;
    }
    public int GetHarpoonNumber()   {
        return harpoonNumber;
    }
    public Sprite GetPullSprite(){
        return spriteKit.Get(0).sprite;
    }
    public Sprite GetCanonSprite()    {
        return spriteKit.Get(1).sprite;
    }
    public Sprite GetHarpoonSprite()    {
        return spriteKit.Get(2).sprite;
    }




    public override List<Statistic> GetStatistics()
    {
        List<Statistic> stats = new List<Statistic>();

        stats.Add(new Statistic("Vitesse des harpons", harpoonSpeed));
        stats.Add(new Statistic("Temps de recharge", cooldown));
        stats.Add(new Statistic("Nombre de harpons", harpoonNumber));

        return stats;
    }
}


