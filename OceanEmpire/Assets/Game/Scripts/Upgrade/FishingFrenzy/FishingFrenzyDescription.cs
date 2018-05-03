using System.Collections.Generic;

[System.Serializable]
public class FishingFrenzyDescription : UpgradeDescription
{
    public float fishMultiplier = 2;

    /// <summary>
    /// In minutes
    /// </summary>
    [Suffix("minutes")]
    public float cooldown;

    public override List<Statistic> GetStatistics()
    {
        List<Statistic> stats = new List<Statistic>
        {
            new Statistic("Poissons bonus", fishMultiplier)
        };

        return stats;
    }
}