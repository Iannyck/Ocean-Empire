using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseFish : MonoBehaviour, IKillable
{
    public delegate void FishEvent(BaseFish fish);

    public FishDescription description;
    public event FishEvent captureEvent;
    public event FishEvent deathEvent;

    private int currentPalier = -1;
    private FishSpawner spawner;
    //private GameCamera cam;

    public bool HasBeenCaptured
    {
        get { return hasBeenCaptured; }
    }
    private bool hasBeenCaptured = false;
    private bool hasBeenKilled = false;

    public void Capture()
    {
        if (hasBeenCaptured)
            return;
        hasBeenCaptured = true;
        
        OnCapture();
    }

    protected virtual void OnCapture()
    {
        if (captureEvent != null)
            captureEvent(this);

        Kill();
    }

    public void Kill()
    {
        if (hasBeenKilled)
            return;
        hasBeenKilled = true;

        if (deathEvent != null)
            deathEvent(this);

        gameObject.SetActive(false);
        ClearEvents();
    }

    protected virtual void ClearEvents()
    {
        deathEvent = null;
        captureEvent = null;
    }

    public void RemiseEnLiberté()
    {
        if (spawner!= null && currentPalier >= 0)
            UnsuscribePalier();

        currentPalier = -1;

        hasBeenKilled = false;
        hasBeenCaptured = false;

        BaseFishDriver driver = GetComponent<BaseFishDriver>();
        if (driver != null)
            driver.ClearMind();
    }

    public bool IsDead()
    {
        return hasBeenCaptured;
    }

    public void Start()
    {
        Game.OnGameStart += Init;
    }

    public void Init()
    {
        spawner = Game.FishSpawner;
        //cam = Game.GameCamera;
    }

    public void Update()
    {
        CheckPalier();
    }





    public void CheckPalier()
    {
        float fishPosition = transform.position.y;
        int closestPalier = spawner.GetClosestPalier(fishPosition);
        //float closestPalierPosition = spawner.GetPalierPosition(closestPalier);

        if (currentPalier == -1)
        {
            SetPalier(closestPalier);
            return;
        }
        if (currentPalier == closestPalier)
            return;

        if ((spawner.GetPalierPosition(currentPalier) - fishPosition).Abs() > spawner.palierHeigth)
        {
            SetPalier(closestPalier);
        }
    }

    public void SetPalier(int newPalierIte)
    {
        if(currentPalier == -1)
        {
            currentPalier = newPalierIte;
            SuscribePalier();
            return;
        }

        if (spawner.GetPalier(newPalierIte).isActive == false)
        {

            Kill();
            return;
        }

        UnsuscribePalier();
        currentPalier = newPalierIte;
        SuscribePalier();
    }

    public void SuscribePalier()
    {
        FishPalier fiP = spawner.GetPalier(currentPalier);
        fiP.SuscribeFish(this);
        fiP.palierDespawnEvent += Kill;
    }

    public void UnsuscribePalier()
    {
        FishPalier fiP = spawner.GetPalier(currentPalier);
        fiP.UnsuscribeFish(this);
        fiP.palierDespawnEvent -= Kill;
    }



}
