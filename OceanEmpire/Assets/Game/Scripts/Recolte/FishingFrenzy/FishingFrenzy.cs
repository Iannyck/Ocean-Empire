using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using System;

public class FishingFrenzy : MonoPersistent
{
    public enum EffectState { InCooldown, Available, CurrentlyActive }

    [SerializeField] bool updateStateEveryFrame = true;
    [SerializeField, Suffix("minutes")] float cooldown;
    [SerializeField, Suffix("minutes")] float duration;
    [SerializeField] DataSaver dataSaver;

    private const string SAVEKEY_NEXTAVAILABLETIME = "nxtAvblTime_UTC";
    private const string SAVEKEY_LASTACTIVATION = "lstActiv_UTC";
    private DateTime nextAvailableTime_UTC;
    private DateTime lastActivation_UTC;

    public TimeSpan Cooldown { get { return new TimeSpan(0, 0, Mathf.RoundToInt(cooldown * 60f)); } }
    public TimeSpan Duration { get { return new TimeSpan(0, 0, Mathf.RoundToInt(duration * 60f)); } }
    public EffectState State { get; private set; }

    public static FishingFrenzy Instance { get; private set; }

    void Awake()
    {
        dataSaver.OnReassignData += FetchData;
        Instance = this;
    }

    void Update()
    {
        if (updateStateEveryFrame)
            UpdateState();
    }

    public override void Init(Action onComplete)
    {
        if (dataSaver.HasEverLoaded)
        {
            FetchData();
            onComplete();
        }
        else
        {
            dataSaver.LateLoad(onComplete);
        }
    }

    public void Activate()
    {
        if (State != EffectState.Available)
        {
            Debug.LogError("Cannot activate FishingFrenzy because it is not currrently available.");
            return;
        }
        lastActivation_UTC = DateTime.UtcNow;
        nextAvailableTime_UTC = lastActivation_UTC + Cooldown;

        UpdateState();
        SaveData();
    }

    public void UpdateState()
    {
        var now_UTC = DateTime.UtcNow;
        if (lastActivation_UTC + Duration > now_UTC)
        {
            State = EffectState.CurrentlyActive;
        }
        else if (nextAvailableTime_UTC < now_UTC)
        {
            State = EffectState.Available;
        }
        else
        {
            State = EffectState.InCooldown;
        }
    }

    public TimeSpan GetRemainingCooldownDuration()
    {
        return nextAvailableTime_UTC - DateTime.UtcNow;
    }
    public TimeSpan GetRemainingActiveDuration()
    {
        return (lastActivation_UTC + Duration) - DateTime.UtcNow;
    }

    public void Cheat_SkipCooldown()
    {
        if (State != EffectState.InCooldown)
        {
            Debug.LogError("(Fishing Frenzy) Cannot skip cooldown because it is not currrently in cooldown.");
            return;
        }
        nextAvailableTime_UTC = DateTime.UtcNow - new TimeSpan(0, 0, 1);
        UpdateState();
        SaveData();
    }

    #region Save/Load
    void FetchData()
    {
        nextAvailableTime_UTC = (DateTime)dataSaver.GetObjectClone(SAVEKEY_NEXTAVAILABLETIME, DateTime.UtcNow);
        lastActivation_UTC = (DateTime)dataSaver.GetObjectClone(SAVEKEY_LASTACTIVATION, new DateTime());
    }

    void SaveData()
    {
        dataSaver.SetObjectClone(SAVEKEY_NEXTAVAILABLETIME, nextAvailableTime_UTC);
        dataSaver.SetObjectClone(SAVEKEY_LASTACTIVATION, lastActivation_UTC);
        dataSaver.LateSave();
    }
    #endregion
}
