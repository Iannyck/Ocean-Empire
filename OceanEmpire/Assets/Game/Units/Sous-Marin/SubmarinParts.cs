using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinParts : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Thruster thruster;

    public HarpoonThrower harpoonThrower;

    public Thruster GetThruster() { return thruster; }
    public HarpoonThrower GetHarpoonThrower() { return harpoonThrower; }

    void Start()
    {     
        ThrusterDescription TD = ItemsList.GetEquipThruster();
        HarpoonThrowerDescription HPD = ItemsList.GetEquipHarpoonThrower();

        if (TD != null)
            thruster = TD.GetItem<Thruster>();
        if (HPD != null)
            harpoonThrower = HPD.GetItem<HarpoonThrower>();
    }

}
