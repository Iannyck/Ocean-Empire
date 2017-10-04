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

    private int currentPalier;
    private FishSpawner spawner;
    private GameCamera cam;

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

        "Receive".Log();
    }

    protected virtual void ClearEvents()
    {
        deathEvent = null;
        captureEvent = null;
    }

    public void RemiseEnLiberté()
    {
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
        cam = Game.GameCamera;

        currentPalier = -1;
        ChangePalier(spawner.GetClosestPalier(transform.position.y));
    }

    public void Update()
    {
        CheckPalier();
    }

    public void CheckPalier()
    {
        float position = transform.position.y;
        int closestPalier = spawner.GetClosestPalier(position);
        float closestPalierPosition = spawner.GetPalierPosition(closestPalier);

        //Doesn't Move
        if (currentPalier == closestPalier)
            return;

        //Move Up
        if (currentPalier < closestPalier)                                  
        {
            //don't change if in palier SpawnRange (prevent auto-despawn)
            if (position < closestPalierPosition)       
                ChangePalier(closestPalier);
            return;
        }

        //Move Down;
        if (currentPalier > closestPalier)
        {
            if (position > closestPalierPosition)
                ChangePalier(closestPalier);
            return;
        }
    }

    public void ChangePalier(int newPalierIte)
    {
        if (currentPalier != -1)
            RemovePalier();
        AddPalier(newPalierIte);
    }

    private void RemovePalier()
    {
        FishPalier oldPalier = spawner.GetPalier(currentPalier);
        oldPalier.UnSuscribeFish(this);
        oldPalier.palierDespawnEvent -= Kill;
    }

    private void AddPalier(int newPalierIte)
    {
        FishPalier newPalier = spawner.GetPalier(newPalierIte);

        if (newPalier.isActive == false)
        { 
            Kill();
            return;
        }
        else
        {
            newPalier.SuscribeFish(this);
            newPalier.palierDespawnEvent += Kill;
            return;
        }
    }

}
