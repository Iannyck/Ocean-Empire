using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinParts : MonoBehaviour
{
    public Thruster Thruster { private set; get; }
    public HarpoonThrower HarpoonThrower { private set; get; }
    public GazTank GazTank { private set; get; }

    [Header("Categories"), SerializeField] private ThrusterCategory thrusterCategory;
    [SerializeField] private HarpoonThrowerCategory harpoonThrowerCategory;
    [SerializeField] private GazTankCategory gazTankCategory;

    [Header("Default upgrades"), SerializeField] private bool useDefaults = false;
    [SerializeField] private ThrusterDescBuilder prebuiltThruster;
    [SerializeField] private HarpoonThrowerDescBuilder prebuiltHarpoon;
    [SerializeField] private GazTankDescBuilder prebuiltGazTank;

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

        ApplyFuelTank();
        ApplyHarpoonThrower();
        ApplyThruster();
    }

    void ApplyThruster()
    {
        var movement = GetComponent<SubmarineMovement>();
        if (movement != null)
        {
            movement.maximumSpeed = Thruster.Description.GetSpeed();
            movement.accelerationRate = Thruster.Description.GetAcceleration();
        }
    }

    void ApplyFuelTank()
    {
    }

    void ApplyHarpoonThrower()
    {
        var slingshotControl = GetComponent<SlingshotControl>();

        if (slingshotControl != null)
        {
            HarpoonThrowerDescription desc = HarpoonThrower.Description;
            slingshotControl.SetHarpoonVisuals(desc.GetCanonSprite(), desc.GetPullSprite(), desc.GetHarpoonSprite());
            slingshotControl.SetHarpoonSpeed(desc.GetHarpoonSpeed());
            slingshotControl.HarpoonCooldown = desc.GetCooldown();
            slingshotControl.HarpoonCount = 1;
        }
    }

    private void Update()
    {
        if (GazTank != null)
            GazTank.UpdateTimer();
    }
}
