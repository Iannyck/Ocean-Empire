using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionWindow : WindowAnimation {

    public class Rewards
    {
        public int amount;
        public Sprite icon;
    }

    public int differentRewardMax = 4;
    public RewardDisplay rewardPrefab;
    public Transform countainer;
    public Button exitButton;

	public void ShowCompletionRewards(List<Rewards> rewards)
    {
        exitButton.onClick.AddListener(delegate ()
        {
            Close();
        });
        if (rewards.Count >= differentRewardMax)
            return;
        for (int i = 0; i < rewards.Count; i++)
        {
            Instantiate(rewardPrefab, countainer).GetComponent<RewardDisplay>().UpdateRewardInfo(rewards[i].amount, rewards[i].icon);
        }
        Open();
    }
}
