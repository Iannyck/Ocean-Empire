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
    private const string SAVEKEY_REMAINING_MARKETVALUE = "CR_remainingMrktVal";

    [SerializeField] private AnalyserGroup analyserGroup;
    [SerializeField] private DataSaver dataSaver;

    [SerializeField, Suffix("seconds")] private int updateEvery = 5;

    [SerializeField] private CurrencyType rewardCurrency = CurrencyType.Ticket;
    [SerializeField] ScriptableActionQueue shackAnimationQueue;
    [SerializeField] SceneInfo rewardScene;

    private DateTime lastUpdate;
    private DateTime nextUpdate;
    private MarketValue remainingMarketValue;

    private float nextCheckTimer = 0;

    public static ContinuousRewarder Instance { get; private set; }

    public override void Init(Action onComplete)
    {
        Instance = this;
        onComplete();

        nextUpdate = DateTimeNow;

        object lastUpdateOBJ = dataSaver.GetObjectClone(SAVEKEY_LASTUPDATE);
        if (lastUpdateOBJ == null)
            lastUpdate = DateTimeNow;
        else
            lastUpdate = (DateTime)lastUpdateOBJ;
        remainingMarketValue = new MarketValue(dataSaver.GetFloat(SAVEKEY_REMAINING_MARKETVALUE));
    }

    DateTime DateTimeNow
    {
        get { return Calendar.Now; }
    }

    private void Update()
    {
        if (!Scenes.IsActive("Shack"))
            return;

        nextCheckTimer -= Time.unscaledDeltaTime;

        if (nextCheckTimer < 0)
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
        if (timeslotToAnalyse.duration.TotalSeconds > 1)
        {
            lastUpdate = now;

            AnalyseAndRewardTimeSlot(timeslotToAnalyse);

            dataSaver.SetObjectClone(SAVEKEY_LASTUPDATE, lastUpdate);
            dataSaver.SetFloat(SAVEKEY_REMAINING_MARKETVALUE, remainingMarketValue.floatValue);
            dataSaver.LateSave();
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

        // Le change qui restait de la dernière fois
        rewardValue += remainingMarketValue;

        // Give reward
        CurrencyAmount reward = Market.GetCurrencyAmountFromValue(rewardCurrency, rewardValue, out remainingMarketValue);

        // TEMPORAIRE
        Logger.Log(Logger.Category.ContinuousReward, "remains: " + remainingMarketValue.floatValue);

        if (reward.amount > 0)
        {
            PlayerCurrency.AddCurrencyAmount(reward);

            Logger.Log(Logger.Category.ContinuousReward, "analysed(" + timeslotToAnalyse.ToEfficientString()
                + ") reward(" + reward.amount + ") remains(" + remainingMarketValue.floatValue + ")");

            // On fait pop un message de "bravo!! 10 ticket pour avoir marché"
            // si ça fait plus de 2 min qu'on a analysé
            if (timeslotToAnalyse.duration > new TimeSpan(0, 2, 0))
            {
                Action onAnimComplete = null;
                shackAnimationQueue.ActionQueue.AddAction(() =>
                {
                    Shack_Canvas shack_Canvas = null;
                    if (Scenes.IsActive("Shack") && (shack_Canvas = Scenes.GetActive("Shack").FindRootObject<Shack_Canvas>()))
                        shack_Canvas.HideAll();
                    Scenes.Load(rewardScene, (s) =>
                    {
                        s.FindRootObject<ContinuousRewardWindow>().FillContent(reward.amount, () =>
                        {
                            if (shack_Canvas)
                                shack_Canvas.ShowAll();
                            onAnimComplete();
                        });
                    });
                }, 0, out onAnimComplete);
            }
        }
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