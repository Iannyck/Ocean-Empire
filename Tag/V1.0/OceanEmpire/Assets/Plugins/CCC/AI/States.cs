using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class States
{
    // Variables Generales
    protected string nom;

    // Interaction Entity
    protected Personnage personnage;
    public Personnage target;

    public States(Personnage personnage)
    {
        this.personnage = personnage;
    }

    public string getNom()
    {
        return nom;
    }

    public Personnage getTarget()
    {
        return target;
    }

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();

    public void MoveTo(Vector3 pos)
    {

    }

    public void Stop()
    {

    }

    public void LookAt(Transform target)
    {

    }

    public void StopLooking()
    {

    }
}
