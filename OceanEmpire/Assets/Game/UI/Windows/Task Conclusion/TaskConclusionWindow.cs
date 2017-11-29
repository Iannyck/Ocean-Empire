using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TaskConclusionWindow : CCC.UI.WindowAnimation
{
    public const string SCENENAME = "Task Conclusion Window";

    [Header("Coin Reward")]
    public GameObject coinReward;
    public Text coinText;
    public string coinPrefix = "x ";

    [Header("Ticket Reward")]
    public GameObject ticketReward;
    public Text ticketText;
    public string ticketPrefix = "x ";

    [Header("Ocean Refill Reward")]
    public GameObject oceanRefillReward;

    private ScheduledTask task;
    private TimedTaskReport report;
    private Action concludeCallback;

    private bool rewardGiven = false;

    public static void ConcludeTask(ScheduledTask task, TimedTaskReport incompleteReport, Action concludeCallback)
    {
        Action<Scene> action = (scene) =>
        {
            scene.FindRootObject<TaskConclusionWindow>().InitWindow(task, incompleteReport, concludeCallback);
        };

        MasterManager.Sync(() =>
        {
            if (Scenes.Exists(SCENENAME))
                action(Scenes.GetActive(SCENENAME));
            else
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, action);
        });
    }

    protected override void Awake()
    {
        base.Awake();

        HideAllRewards();
    }

    private void InitWindow(ScheduledTask task, TimedTaskReport incompleteReport, Action concludeCallback)
    {
        this.report = incompleteReport;
        this.task = task;
        this.concludeCallback = concludeCallback;

        DisplayReward();
    }

    private void HideAllRewards()
    {
        ticketReward.SetActive(false);
        coinReward.SetActive(false);
        oceanRefillReward.SetActive(false);
    }

    private void DisplayReward()
    {
        Reward reward = task.task.GetReward();
        switch (reward.GetRewardType())
        {
            case RewardType.Coins:
                coinReward.SetActive(true);
                coinText.text = coinPrefix + (reward as Reward_Coins).amount;
                break;
            case RewardType.Tickets:
                ticketReward.SetActive(true);
                ticketText.text = ticketPrefix + (reward as Reward_Tickets).amount;
                break;
            case RewardType.OceanRefill:
                oceanRefillReward.SetActive(true);
                break;
            default:
                Debug.LogError("Unsupported reward type (Task Conclusion Window)");
                break;
        }
    }

    public override void Close(TweenCallback onComplete)
    {
        GiveRewardIfNotDone();
        base.Close(onComplete);
    }

    protected override void OnCloseComplete()
    {
        GiveRewardIfNotDone();
        base.OnCloseComplete();
    }

    private void GiveRewardIfNotDone()
    {
        if (rewardGiven)
            return;

        Reward reward = task.task.GetReward();
        bool applyResult = reward.Apply();
        concludeCallback();

        if (!applyResult)
        {
            MessagePopup.DisplayMessage("Une erreur est survenu. Donner la r\u00E9compense a \u00E9chou\u00E9");
        }

        rewardGiven = true;
    }
}
