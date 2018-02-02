using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using System;

/// <summary>
/// Classe qui s'occupe de continuellement reward le joueur pour son exercice.
/// Elle vérifie périodiquement les dernières mise à jours de capture d'exercice et reward le joueur en conséquence.
/// </summary>
public class ContinuousRewarder : MonoPersistent
{
    [SerializeField] private AnalyserGroup analyserGroup;

    [SerializeField, Suffix("seconds")] private float updateEvery = 5;

    [SerializeField] private float lastUpdate;

    public override void Init(Action onComplete)
    {
        onComplete();
    }

    private void Update()
    {
        var currentTime = Time.realtimeSinceStartup;
        if (currentTime > lastUpdate + updateEvery)
        {
            UpdateReward();
        }
    }

    public void ForceUpdate() { UpdateReward(); }
    private void UpdateReward()
    {
        var currentTime = Time.realtimeSinceStartup;
        var elapsedTime = currentTime - lastUpdate;
        lastUpdate = Time.realtimeSinceStartup;

        var timeslotToAnalyse = new TimeSlot(DateTime.Now.AddSeconds(-elapsedTime), DateTime.Now);
        AnalyserGroupReport groupReport = analyserGroup.GetExerciseVolume(timeslotToAnalyse);

        // Calculate reward value
        MarketValue rewardValue = 0;
        foreach (AnalyserReport individualReport in groupReport.individualReports)
        {
            rewardValue += Market.GetExerciseValue(individualReport.volume);
        }

        // Give reward
        CurrencyAmount reward = Market.GetCurrencyAmountFromValue(CurrencyType.Ticket, rewardValue);
        if (reward.amount > 0)
            PlayerCurrency.AddCurrencyAmount(reward);
    }
}