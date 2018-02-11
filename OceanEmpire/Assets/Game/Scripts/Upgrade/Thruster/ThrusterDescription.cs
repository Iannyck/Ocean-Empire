using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThrusterDescription : UpgradeDescription {
    /*
    [SerializeField, ReadOnly] private Thruster thruster;
    public Thruster GetThruster() { return thruster;}
    */

    public float subMarineSpeed = 1;
    public float subMarineAcceleration = 1;

    public float GetSpeed()
    {
        return subMarineSpeed;
    }
    public float GetAcceleration()
    {
        return subMarineAcceleration;
    }


    public ThrusterDescription(string nm, int lv, string desc, int coin, int tick, Sprite Icon, float Speed, float Acce)
        : base(nm, lv, desc, coin, tick, Icon)
    {
        subMarineSpeed = Speed;
        subMarineAcceleration = Acce;
    }
}
