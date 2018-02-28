using UnityEngine;

public class GazTank
{
    public float GazTimeRemaining { private set; get; }
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
        GazTimeRemaining = Description.GetDiveDuration();
    }

    public float GetGazRatio()
    {
        return GazTimeRemaining / Description.GetDiveDuration();
    }
}
