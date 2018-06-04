using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GazTankDescription : UpgradeDescription
{
    [SerializeField]
    private float diveDuration = 2;

    public float GetDiveDuration()
    {
        return diveDuration;
    }

    public override List<Statistic> GetStatistics()
    {
        List<Statistic> stats = new List<Statistic>
        {
            new Statistic("Durée de plongée", diveDuration, "s")
        };

        return stats;
    }

    //public override string GetStats()
    //{
    //    return "Durée de plongé: <stat>" + diveDuration + "s</stat>";
    //}
}