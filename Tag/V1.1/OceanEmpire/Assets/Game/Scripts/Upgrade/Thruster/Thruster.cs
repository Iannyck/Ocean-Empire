using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Thruster
{
    public ThrusterDescription Description { private set; get; }

    public Thruster(ThrusterDescription description)
    {
        Description = description;
    }
}
