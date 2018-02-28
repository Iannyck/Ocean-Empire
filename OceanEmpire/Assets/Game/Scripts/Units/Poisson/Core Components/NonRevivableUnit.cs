using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonRevivableUnit : BaseKillableUnit
{
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
