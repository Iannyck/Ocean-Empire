using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerPop : MonoBehaviour {

    public void LowerPopopulationRate()
    {
        FishPopulation.LowerRate(0.25f);
    }
}
