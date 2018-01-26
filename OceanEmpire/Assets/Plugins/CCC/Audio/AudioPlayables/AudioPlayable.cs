using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioPlayable : ScriptableObject
{
    public float cooldown = -1;

    [System.NonSerialized]
    protected float lastPlayedTime = -1;

    public virtual bool IsInCooldown
    {
        get
        {
            return cooldown > 0 && Time.time < lastPlayedTime + cooldown && Application.isPlaying;
        }
    }

    public void PlayOn(AudioSource audioSource, float volumeMultiplier = 1)
    {
        if (IsInCooldown)
            return;

        Internal_PlayOn(audioSource, volumeMultiplier);
        lastPlayedTime = Time.time;
    }
    public void PlayOnAndIgnoreCooldown(AudioSource audioSource, float volumeMultiplier = 1)
    {
        Internal_PlayOn(audioSource, volumeMultiplier);
    }

    public void PlayLoopedOn(AudioSource audioSource, float volumeMultiplier = 1)
    {
        Interal_PlayLoopedOn(audioSource, volumeMultiplier);
    }

    protected abstract void Internal_PlayOn(AudioSource audioSource, float volumeMultiplier = 1);
    protected abstract void Interal_PlayLoopedOn(AudioSource audioSource, float volumeMultiplier = 1);
}