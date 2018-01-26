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
        GazTimeRemaining = (GazTimeRemaining - Time.deltaTime).Raised(0);

        if (GazTimeRemaining <= 0 && !Game.Instance.gameOver)
            Game.Instance.EndGame();
    }

    public void SetGaz()
    {
        GazTimeRemaining = GazDuration;
    }

    public float GetGazRatio()
    {
        return 1 - ( (GazDuration - GazTimeRemaining) / GazDuration );
    }
}
