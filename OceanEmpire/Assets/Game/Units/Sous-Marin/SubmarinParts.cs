using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinParts : MonoBehaviour
{
    private ThrusterDescription thruster;
    private HarpoonThrowerDescription harpoonThrower;
    private GazTank gazTank;

    public ThrusterCategory thrusterCategory;
    public HarpoonThrowerCategory harpoonThrowerCategory;
    public GazTankCategory gazTankCategory;

    public ThrusterDescription GetThruster() { return thruster; }
    public HarpoonThrowerDescription GetHarpoonThrower() { return harpoonThrower; }
    public GazTank GetGazTank() { return gazTank; }

    void Start()
    {
        thruster = thrusterCategory.GetCurrentDescription() as ThrusterDescription;

        harpoonThrower = harpoonThrowerCategory.GetCurrentDescription() as HarpoonThrowerDescription;
        GetComponent<SlingshotControl>().Initiate(
            harpoonThrower.GetCanonSprite(),
            harpoonThrower.GetPullSprite(),
            harpoonThrower.GetHarpoonSprite(),
            harpoonThrower.GetHarpoonSpeed());

        GazTankDescription GTD = gazTankCategory.GetCurrentDescription() as GazTankDescription;
        gazTank = new GazTank(GTD.GetDiveDuration());
    }

    private void Update()
    {
        if (gazTank != null)
            gazTank.UpdateTimer();
    }
}
