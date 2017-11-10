using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinParts : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Thruster thruster;

    public Thruster GetThruster() { return thruster; }
    void Start()
    {
        thruster = ItemsList.GetEquipThruster();
    }

}
