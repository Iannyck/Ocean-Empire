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
        thruster = ItemsList.GetEquipThruster().GetItem<Thruster>();
        harpoonThrower = ItemsList.GetEquipHarpoonThrower().GetItem<HarpoonThrower>();
    }

}
