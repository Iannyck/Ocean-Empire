using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetItems : MonoBehaviour {

    public ThrusterCategory TC;
    public HarpoonThrowerCategory HTC;
    public GazTankCategory GTC;

    public void ResetData()
    {
        TC.ResetData();
        HTC.ResetData();
        GTC.ResetData();
    }
}
