using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonThrower
{
    public int ThrownAmount { private set; get; }
    public HarpoonThrowerDescription Description { private set; get; }

    public HarpoonThrower(HarpoonThrowerDescription description)
    {
        Description = description;
    }
}

