using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ocean Empire/Item/Gaz Tank")]
public class GazTank : Upgrade
{
    public float GazDuration = 20;
    [SerializeField, ReadOnly]
    private float GazTimeRemaining = 0;


    public void UpdateTimer()
    {
        GazTimeRemaining = Mathf.Max(GazTimeRemaining - Time.deltaTime, 0);
    }

    public void SetGaz()
    {
        GazTimeRemaining = GazDuration;
    }

    public float GetGazRatio()
    {
        return GazTimeRemaining / GazDuration;
    }
}
