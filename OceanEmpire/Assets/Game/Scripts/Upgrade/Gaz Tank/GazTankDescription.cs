using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GazTankDescription : UpgradeDescription {
    [SerializeField, ReadOnly] private GazTank gazTank;
    public GazTank GetGazTank() { return gazTank; }

    [SerializeField]
    private float diveDuration = 2;

    public float GetDiveDuration()
    {
        return diveDuration;
    }

    public override List<Statistic> GetStatistics()
    {
        List<Statistic> stats = new List<Statistic>();

        stats.Add(new Statistic("Durée de plongée", diveDuration));

        return stats;
    }
}