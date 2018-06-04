using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThrusterDescription : UpgradeDescription
{
    [SerializeField, Suffix("x 2")]
    private float speed = 1; // "speed"
    [SerializeField, Suffix("x 10")]
    private float acceleration = 1; // "Accélération"
    [SerializeField]
    private float deceleration = 1; //"décélération"

    public float GetSpeed()
    {
        return speed;
    }
    public float GetAcceleration()
    {
        return acceleration;
    }
    public float GetDeceleration()
    {
        return deceleration;
    }
    public override List<Statistic> GetStatistics()
    {
        List<Statistic> stats = new List<Statistic>
        {
            new Statistic("Vitesse", speed * 2),
            new Statistic("Accélération", acceleration * 10)/*,
            new Statistic("décélération", deceleration)*/
        };

        return stats;
    }

    //public override string GetStats()
    //{
    //    return "Vitesse: <stat>" + speed
    //        + "</stat>\nAccélération: <stat>" + acceleration + "</stat>";
    //}


    public ThrusterDescription(string nm, int lv, string desc, int coin, int tick, Sprite Icon, float Speed, float Acce, float Dece)
        : base(nm, lv, desc, coin, tick, Icon)
    {
        speed = Speed;
        acceleration = Acce;
        deceleration = Dece;
    }
}
