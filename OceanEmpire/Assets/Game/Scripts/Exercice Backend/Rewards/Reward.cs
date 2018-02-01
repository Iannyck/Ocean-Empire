using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Reward
{
    public abstract bool Apply();
    public abstract RewardType GetRewardType();
    public abstract MarketValue GetMarketValue();
}
