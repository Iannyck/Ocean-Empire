using System.Collections.Generic;

[System.Serializable]
public class FishingFrenzyDescription : UpgradeDescription
{
    public float fishMultiplier = 2;
    public float displayedfishMultiplier = 2;

    /// <summary>
    /// In minutes
    /// </summary>
    [Suffix("minutes")]
    public float cooldown;

    /// <summary>
    /// In seconds
    /// </summary>
    [Suffix("seconds")]
    public float duration;

    public override List<Statistic> GetStatistics()
    {
        List<Statistic> stats = new List<Statistic>
        {
            new Statistic("Extra-Poisson", displayedfishMultiplier, "x"),
            new Statistic("Durée", duration, "s"),
            new Statistic("Recharge", cooldown * 60, "s")
        };

        return stats;
    }
}