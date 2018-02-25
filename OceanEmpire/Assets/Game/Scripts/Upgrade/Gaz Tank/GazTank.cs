using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ocean Empire/Item/Gaz Tank")]
public class GazTank : Upgrade
{
    public float GazDuration = 20;
    [SerializeField, ReadOnly]
    private float GazTimeRemaining = 0;

    public GazTank(float _GazDuration)
    {
        this.GazDuration = _GazDuration;
        FillGaz();
    }

    public void UpdateTimer()
    {
        GazTimeRemaining = Mathf.Max(GazTimeRemaining - Time.deltaTime, 0);
    }

    public void FillGaz()
    {
        GazTimeRemaining = GazDuration;
    }

    public float GetGazRatio()
    {
        return GazTimeRemaining / GazDuration;
    }
}
