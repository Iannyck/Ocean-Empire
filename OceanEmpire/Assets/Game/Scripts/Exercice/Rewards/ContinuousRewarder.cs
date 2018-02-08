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
            List<BT> result = GetAllBTInTimeSlot(new TimeSlot(start, end));
            for (int i = 0; i < result.Count; i++)
            {
                print(result[i].bonus + " : " + result[i].ts);
            }
            if (result.Count == 0)
                print("nothin'");
        }
    }

    public void ForceUpdate() { UpdateReward(); }

    private struct BT
    {
        public BonifiedTime bonus;
        public TimeSlot ts;
    }

    private static List<BT> GetAllBTInTimeSlot(TimeSlot analysedTime)
    {
        List<BT> list = new List<BT>();

        var past = Calendar.instance.GetPastBonifiedTimes();
        var future = Calendar.instance.GetPresentAndFutureBonifiedTimes();

        for (int i = past.Count - 1; i >= 0; i--)
        {
            TimeSlot overlap;
            int entryIsInThePast = analysedTime.IsOverlappingWith(past[i].timeslot, out overlap);

            // Stop ! On est allé trop loin
            if (entryIsInThePast == 1)
            {
                break;
            }

            // OVERLAP !
            if (entryIsInThePast == 0)
                list.Add(new BT() { bonus = past[i], ts = overlap });
        }
        list.Reverse();

        for (int i = 0; i < future.Count; i++)
        {
            TimeSlot overlap;
            int entryIsInThePast = analysedTime.IsOverlappingWith(future[i].timeslot, out overlap);

            // Stop ! On est allé trop loin
            if (entryIsInThePast == -1)
            {
                break;
            }

            // OVERLAP !
            if (entryIsInThePast == 0)
                list.Add(new BT() { bonus = future[i], ts = overlap });
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