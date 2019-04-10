using UnityEngine;

public class GazTank
{
    public float GazTimeRemaining {  set; get; }
    public float MaxGas { private set; get; }
    public GazTankDescription Description { private set; get; }

    public GazTank(GazTankDescription description)
    {
        Description = description;
        FillGaz();
    }

    public void UpdateTimer()
    {
        GazTimeRemaining = Mathf.Max(GazTimeRemaining - Time.deltaTime, 0);
    }

    public void FillGaz()
    {
        FishingFrenzyDescription desc;
        if (Game.Instance.IsInFishingFrenzy
            && (desc = FishingFrenzy.Instance.shopCategory.GetCurrentDescription()) != null)
            MaxGas = desc.duration;
        else
            MaxGas = Description.GetDiveDuration();

        GazTimeRemaining = MaxGas;
    }

    public float GetGazRatio()
    {
        return GazTimeRemaining / MaxGas;
    }

    public void degat(float penality)
    {
        GazTimeRemaining -= penality;
    }
}
