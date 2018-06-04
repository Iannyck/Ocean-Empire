using System;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Utility.Lootbox
{
    /// <summary>
    /// Backend of a lootbox system. You open and get rewards without animation
    /// </summary>
    public class LootBox
    {
        public List<LootBoxReward> rewards;

        /// <summary>
        /// Open a lootbox with the specified parameters
        /// </summary>
        /// <param name="reference">The lootbox will be identical to this reference</param>
        /// <param name="onComplete">Callback to get the rewards once the lootbox has been open</param>
        /// <param name="onlyUnique">Would you like to open only unique rewards (can't get already own or doubles)"/>)</param>
        public static void OpenLootbox(LootBoxRef reference, Action<LootBox> onComplete, bool onlyUnique = false)
        {
            // Create Lootbox (rewards picked at creation)
            LootBox newLootbox = new LootBox(reference, onlyUnique);
            // Return the lootbox
            onComplete.Invoke(newLootbox);
        }

        private LootBox(LootBoxRef reference, bool onlyUnique)
        {
            // Pick rewards
            reference.PickRewards(onlyUnique, GiveRewards);
        }

        private void GiveRewards(List<LootBoxReward> report)
        {
            // Put the rewards in lootbox
            rewards = report;
        }
    }
}