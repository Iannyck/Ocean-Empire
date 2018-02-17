using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonThrowerDescription : UpgradeDescription {
    [SerializeField, ReadOnly] private HarpoonThrower harpoonThrower;
    public HarpoonThrower GetHarpoonThrower() { return harpoonThrower; }

    [SerializeField]
    private float harpoonSpeed = 2;
    [SerializeField]
    private float cooldown = 5;
    [SerializeField]
    private float harpoonNumber = 0;

    public float GetHarpoonSpeed()
    {
        return harpoonSpeed;
    }
    public float GetCooldown()
    {
        return cooldown;
    }
    public float GetHarpoonNumber()
    {
        return harpoonNumber;
    }


    public override List<Statistic> GetStatistics()
    {
        List<Statistic> stats = new List<Statistic>();

        stats.Add(new Statistic("Vitesse des harpons", harpoonSpeed));
        stats.Add(new Statistic("temps de recharge", cooldown));
        stats.Add(new Statistic("Nombre de harpons", harpoonNumber));

        return stats;
    }
}
