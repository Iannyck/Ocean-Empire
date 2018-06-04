using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonBlocker : MonoBehaviour
{
    public virtual void HarpoonHit(out bool repelHarpoon)
    {
        repelHarpoon = true;
        //Do nothing
    }
}
