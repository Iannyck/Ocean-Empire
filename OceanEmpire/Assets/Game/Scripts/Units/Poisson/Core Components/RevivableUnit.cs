using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivableUnit : BaseKillableUnit
{
    public void Revive()
    {
        gameObject.SetActive(true);
    }

    void OnEnable()
    {
        isDead = false;
    }

    protected override void OnDeath()
    {
        gameObject.SetActive(false);
    }
}
