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

    [SerializeField] private CurrencyType rewardCurrency = CurrencyType.Ticket;
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

        if (Input.GetKeyDown(KeyCode.T))
        {
            var start = new DateTime(2018, 2, 10, 1, 30, 0);
            var end = new DateTime(2018, 2, 10, 5, 30, 0);
            List<BonifiedTime> result = GetAllBTInTimeSlot(new TimeSlot(start, end));
            for (int i = 0; i < result.Count; i++)
            {
                print(result[i].bonus + " : " + result[i].timeSlot);
            }
            if (result.Count == 0)
                print("nothin'");
        }
    }

    public void ForceUpdate() { UpdateReward(); }

    private struct BT
    {
        public ScheduledBonus bonus;
        public TimeSlot ts;
    }

    private static List<BonifiedTime> GetAllBTInTimeSlot(TimeSlot analysedTime)
    {
        List<BonifiedTime> list = new List<BonifiedTime>();

        var past = Calendar.instance.GetPastBonifiedTimes();
        var future = Calendar.instance.GetPresentAndFutureBonifiedTimes();

        for (int i = past.Count - 1; i >= 0; i--)
        {
            var bonifiedTime = past[i].GetBonifiedTime();
            int compareResult;
            var result = BonifiedTime.Cross(bonifiedTime, analysedTime, out compareResult);

            if (result != null)
                list.Add(result);

            // bonifiedTime -> analysedTime    Stop ! On est allé trop loin dans le passé
            if (compareResult == -1)
            {
                break;
            }
        }
        list.Reverse();

        for (int i = 0; i < future.Count; i++)
        {
            var bonifiedTime = future[i].GetBonifiedTime();
            int compareResult;
            var result = BonifiedTime.Cross(bonifiedTime, analysedTime, out compareResult);

            if (result != null)
                list.Add(result);

            // analysedTime -> bonifiedTime    Stop ! On est allé trop loin dans le passé
            if (compareResult == 1)
            {
                break;
            }
        }

        return list;
    }

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
        CurrencyAmount reward = Market.GetCurrencyAmountFromValue(rewardCurrency, rewardValue);
        if (reward.amount > 0)
            PlayerCurrency.AddCurrencyAmount(reward);
    }
}