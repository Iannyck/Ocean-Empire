using System.Collections.Generic;
using UnityEngine;
using System;

namespace CCC.Utility.Lootbox
{
    public class LootBoxRef : ScriptableObject
    {
        [Header("Rewards")]
        public Dictionary<LootBoxReward, float> possibleItems = new Dictionary<LootBoxReward, float>();

        public int itemsAmount = 1; // Amount of rewards given

        public void PickRewards(bool onlyUnique, Action<List<LootBoxReward>> callback)
        {
            // Contruction de la lottery
            Lottery<LootBoxReward> lot = new Lottery<LootBoxReward>();

            foreach (var reward in possibleItems)
            {
                // Conditions to adding an item in rewards here :
                // example : this items in the ref is not yet available by the player so it needs to not appear in lootbox reward

                // If player already has item and we want only unique reward
                if (reward.Key.PlayerAlreadyHasIt() && onlyUnique)
                    continue;

                lot.Add(reward.Key,reward.Value);
            }

            List<LootBoxReward> pickedItems = new List<LootBoxReward>();

            //On pick
            for (int i = 0; i < itemsAmount; i++)
            {
                if (lot.Count <= 0)
                    break; // No rewards

                int pickedIndex = -1;
                pickedItems.Add(lot.Pick(out pickedIndex));
                lot.RemoveAt(pickedIndex);
            }

            callback(pickedItems);
        }
    }
}
