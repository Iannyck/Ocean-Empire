using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicElectrify : Electrify
{
    public float electricInterval = 2.5f;
    public float electricDuration = 2.5f;


    protected override void Start () {
        base.Start();

        this.DelayedCall(ElectricRestart, electricDuration);
    }

    void ElectricRestart()
    {
        StopElectricEffect();
        this.DelayedCall(delegate() {
            ElectricEffect();
            this.DelayedCall(ElectricRestart, electricDuration);
        }, electricInterval);
    }
}
