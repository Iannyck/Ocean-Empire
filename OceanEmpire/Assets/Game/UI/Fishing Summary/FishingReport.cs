using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingReport
{
    public Dictionary<FishDescription, int> CapturedFish = new Dictionary<FishDescription, int>();

    public void AddToReport(BaseFish fish)
    {
        if (!CapturedFish.ContainsKey(fish.description))
            CapturedFish.Add(fish.description, 1);

        CapturedFish[fish.description] += 1;
    }

    public void KeepTrack(BaseFish fish)
    {
        fish.captureEvent += AddToReport;
    }

    public List<FishDescription> GetSortedFishes()
    {
        List<FishDescription> tempD = new List<FishDescription>();

        int FishType = CapturedFish.Count;
        for (int i = 0; i < FishType; i++)
        {
            int maxValue = -1;
            FishDescription maxFish = null;

            int FishRemaining = CapturedFish.Count;
            foreach (KeyValuePair<FishDescription, int> entry in CapturedFish)
            {
                if (entry.Key.baseMonetaryValue > maxValue)
                    maxFish = entry.Key;
            }
            CapturedFish.Remove(maxFish);
            tempD.Add(maxFish);
        }
        return tempD;
    }
}