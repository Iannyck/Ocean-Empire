using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Utility.Lootbox
{
    // Data structure containing info about the reward we gives to player
    public abstract class LootBoxReward : ScriptableObject
    {
        public virtual void GiveRewardToPlayer()
        {
            Debug.Log("Giving Reward to player : add your code here");
        }
        public virtual bool PlayerAlreadyHasIt()
        {
            Debug.Log("Checking if player already has reward : add your code here");
            return false;
        }
    }
}
