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
    private const string SAVEKEY_LASTUPDATE = "CR_lastUpdate";
    [SerializeField] private AnalyserGroup analyserGroup;
    [SerializeField] private DataSaver dataSaver;

    [SerializeField, Suffix("seconds")] private int updateEvery = 5;

    [SerializeField] private CurrencyType rewardCurrency = CurrencyType.Ticket;

    private DateTime lastUpdate;
    private DateTime nextUpdate;

    private float nextCheckTimer = 0;

    public override void Init(Action onComplete)
    {
        onComplete();

        nextUpdate = DateTimeNow;

        object lastUpdateOBJ = dataSaver.GetObjectClone(SAVEKEY_LASTUPDATE);
        if (lastUpdateOBJ == null)
            lastUpdate = DateTimeNow;
        else
            lastUpdate = (DateTime)lastUpdateOBJ;
    }

    DateTime DateTimeNow
    {
        get { return DateTime.Now; }
    }

    private void Update()
    {
        nextCheckTimer -= Time.unscaledDeltaTime;

        if(nextCheckTimer < 0)
        {
            CheckIfNeedToUpdate();
            nextCheckTimer = updateEvery / 10;
        }
    }

    public void CheckIfNeedToUpdate()
    {
        if (DateTimeNow > nextUpdate)
            UpdateReward();
    }

    public void ForceUpdate() { UpdateReward(); }

    private void UpdateReward()
    {
        var now = DateTimeNow;
        nextUpdate = now + new TimeSpan(0, 0, updateEvery);

        var timeslotToAnalyse = new TimeSlot(lastUpdate, now);
        if (timeslotToAnalyse.duration.Seconds > 1)
        {
            lastUpdate = now;

            dataSaver.SetObjectClone(SAVEKEY_LASTUPDATE, lastUpdate);

            AnalyseAndRewardTimeSlot(timeslotToAnalyse);
        }
    }

    void AnalyseAndRewardTimeSlot(TimeSlot timeslotToAnalyse)
    {
        var bts = Calendar.instance.GetAllBonifiedTimesInTimeSlot(timeslotToAnalyse);

        DateTime previous = timeslotToAnalyse.start;

        MarketValue rewardValue = MarketValue.Zero;

        for (int i = 0; i < bts.Count; i++)
        {
            var ts = new TimeSlot(previous, bts[i].timeSlot.start);

            //On prend la reward entre le bonified time et le precendant
            rewardValue += GetMarketValueOfTimeSlot(ts, null);

            //On prend la reward du bonified time
            rewardValue += GetMarketValueOfTimeSlot(bts[i].timeSlot, bts[i].bonus);

            previous = bts[i].timeSlot.end;
        }

        //On prend la reward entre le bonified time et le precendant
        var finalTs = new TimeSlot(previous, timeslotToAnalyse.end);
        rewardValue += GetMarketValueOfTimeSlot(finalTs, null);


        // Give reward
        CurrencyAmount reward = Market.GetCurrencyAmountFromValue(rewardCurrency, rewardValue);

        if (reward.amount > 0)
            PlayerCurrency.AddCurrencyAmount(reward);
    }

    MarketValue GetMarketValueOfTimeSlot(TimeSlot timeSlot, Bonus bonus)
    {
        if (timeSlot.duration.TotalSeconds == 0)
            return MarketValue.Zero;

        AnalyserGroupReport groupReport = analyserGroup.GetExerciseVolume(timeSlot);

        // Calculate reward value
        MarketValue rewardValue = 0;
        foreach (AnalyserReport individualReport in groupReport.individualReports)
        {
            rewardValue += Market.GetExerciseValue(individualReport.volume);
        }

        if (bonus != null)
            rewardValue *= bonus.ticketMultiplier;

        return rewardValue;
    }
}