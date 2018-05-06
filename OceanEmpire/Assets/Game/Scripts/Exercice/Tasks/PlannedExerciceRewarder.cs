using CCC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlannedExerciceRewarder : MonoPersistent
{
    public static PlannedExerciceRewarder instance = null;

    public bool keepAnalysing = true;

    [SerializeField] private float cooldown = 5f;

    [SerializeField] private SceneInfo failureWindow;
    [SerializeField] private SceneInfo completionWindow;

    [SerializeField] private Sprite ticketIcon;

    [SerializeField] private AnalyserGroup analyserGroup;

    DateTime lastTimeChecked = new DateTime();

    private CurrencyType rewardCurrency = CurrencyType.Ticket;

    public override void Init(Action onComplete)
    {
        if (instance == null)
            instance = this;
        onComplete();
    }

    void Start()
    {
        if (keepAnalysing)
            this.DelayedCall(Analyse, cooldown);
    }

    void Analyse()
    {
        if (keepAnalysing)
        {
            //List<BonifiedTime> plannedExercice = CheckPlannedExercice();

            //MarketValue totalRewardValue = 0;

            //ExerciseReport lastNoExerciceReport = new ExerciseReport();

            ////Debug.Log("Analyse des exercices passés");

            //if (plannedExercice != null && plannedExercice.Count > 0)
            //{
            //    //Debug.Log("Exercices passés trouvé !");
            //    for (int i = 0; i < plannedExercice.Count; i++)
            //    {
            //        MarketValue rewardValue = 0;

            //        float activityVolume = GetActivityVolumeIn(plannedExercice[i]);
            //        rewardValue.floatValue = activityVolume;

            //        if (VerifyCompletionOf(plannedExercice[i], activityVolume))
            //            totalRewardValue += rewardValue;

            //        //plannedExercice[i].plannedExercice.ended = true;

            //        ExerciseReport report = new ExerciseReport(plannedExercice[i].timeSlot, plannedExercice[i].plannedExercice, activityVolume);

            //        // Code degueu
            //        if(VerifyCompletionOf(plannedExercice[i], activityVolume))
            //            report.state = ExerciseReport.State.Completed;
            //        else
            //            report.state = ExerciseReport.State.Failed;

            //        if (activityVolume > 0)
            //            PlayerProfile.instance.LogReport(report);
            //        else
            //            lastNoExerciceReport = report;
            //    }

            //    if (totalRewardValue.floatValue > 0)
            //        GiveRewards(totalRewardValue.floatValue);
            //    else
            //        GiveAdvice(lastNoExerciceReport);
            //}
        }

        this.DelayedCall(Analyse, cooldown);
    }

    List<BonifiedTime> CheckPlannedExercice()
    {
        List<BonifiedTime> plannedExercice = null;

        TimeSlot timeSlot = new TimeSlot(lastTimeChecked, DateTime.Now);
        plannedExercice = Calendar.instance.GetAllBonifiedTimesInTimeSlot(timeSlot);

        lastTimeChecked = DateTime.Now;

        return plannedExercice;
    }

    float GetActivityVolumeIn(BonifiedTime bonifiedTime)
    {
        float volume = 0;

        if (bonifiedTime.timeSlot.duration.TotalSeconds != 0)
        {
            AnalyserGroupReport groupReport = analyserGroup.GetExerciseVolume(bonifiedTime.timeSlot);

            foreach (AnalyserReport individualReport in groupReport.individualReports)
            {
                volume += individualReport.volume.volume;
            }
        }

        return volume;
    }

    bool VerifyCompletionOf(ScheduledBonus schedule, float activityVolume)
    {
        return activityVolume >= schedule.task.minDuration;
    }

    void GiveAdvice(ExerciseReport report)
    {
        Scenes.Load(failureWindow,delegate(Scene scene) {
            scene.FindRootObject<FailureAsk>().Init(report);
        });
    }

    void GiveRewards(float rewardAmount)
    {
        // Give reward
        CurrencyAmount reward = Market.GetCurrencyAmountFromValue(rewardCurrency, rewardAmount);

        if (reward.amount > 0)
            PlayerCurrency.AddCurrencyAmount(reward);

        List<CompletionWindow.Rewards> rewards = new List<CompletionWindow.Rewards>();
        CompletionWindow.Rewards newReward = new CompletionWindow.Rewards
        {
            amount = Mathf.RoundToInt(rewardAmount),
            icon = ticketIcon
        };
        rewards.Add(newReward);

        Scenes.Load(completionWindow,delegate(Scene scene) {
            scene.FindRootObject<CompletionWindow>().ShowCompletionRewards(rewards);
        });
    }
}
