using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinParts : MonoBehaviour
{
    public Thruster Thruster { private set; get; }
    public HarpoonThrower HarpoonThrower { private set; get; }
    public GazTank GazTank { private set; get; }

    [Header("Default upgrades"), SerializeField] private bool useDefaults = false;
    [SerializeField] private ThrusterDescBuilder prebuiltThruster;
    [SerializeField] private HarpoonThrowerDescBuilder prebuiltHarpoon;
    [SerializeField] private GazTankDescBuilder prebuiltGazTank;

    [SerializeField] private ThrusterCategory thrusterCategory;
    [SerializeField] private HarpoonThrowerCategory harpoonThrowerCategory;
    [SerializeField] private GazTankCategory gazTankCategory;

    void Start()
    {
        GazTankDescription GTD = null;
        ThrusterDescription TD = null;
        HarpoonThrowerDescription HTD = null;
        if (useDefaults)
        {
            TD = prebuiltThruster.BuildUpgradeDescription();
            HTD = prebuiltHarpoon.BuildUpgradeDescription();
            GTD = prebuiltGazTank.BuildUpgradeDescription();
        }
        else
        {
            TD = thrusterCategory.GetCurrentDescription();
            HTD = harpoonThrowerCategory.GetCurrentDescription();
            GTD = gazTankCategory.GetCurrentDescription();
        }

        GazTank = new GazTank(GTD);
        Thruster = new Thruster(TD);
        HarpoonThrower = new HarpoonThrower(HTD);

        SlingshotControl slingshotControl = GetComponent<SlingshotControl>();

        if (slingshotControl != null)
        {
            HarpoonThrowerDescription desc = HarpoonThrower.Description;
            slingshotControl.Initiate(
                desc.GetCanonSprite(),
                desc.GetPullSprite(),
                desc.GetHarpoonSprite(),
                desc.GetHarpoonSpeed());
        }

    }

    private void Update()
    {
        if (GazTank != null)
            GazTank.UpdateTimer();
    }
}
