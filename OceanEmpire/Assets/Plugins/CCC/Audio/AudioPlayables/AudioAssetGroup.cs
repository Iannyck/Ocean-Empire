using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "CCC/Audio/Audio Asset Group", fileName = "AAG_Something")]
public class AudioAssetGroup : AudioPlayable
{
    public AudioPlayable[] clips;

    [NonSerialized]
    private int lastPickedIndex = -1;

    protected override void Internal_PlayOn(AudioSource audioSource, float volumeMultiplier = 1)
    {
        if (!CheckRessources())
            return;

        PickAsset().PlayOn(audioSource, volumeMultiplier);
    }

    protected override void Interal_PlayLoopedOn(AudioSource audioSource, float volumeMultiplier = 1)
    {
        if (!CheckRessources())
            return;

        PickAsset().PlayLoopedOn(audioSource, volumeMultiplier);
    }

    private bool CheckRessources()
    {
        return clips != null && clips.Length != 0;
    }

    private AudioPlayable PickAsset()
    {
        if (lastPickedIndex >= clips.Length)
            lastPickedIndex = 0;

        if (clips.Length == 1)
            return clips[0];


        int from;
        int to;
        if (lastPickedIndex == -1)
        {
            //On a jamais pick
            from = 0;
            to = clips.Length;
        }
        else
        {
            //On a deja pick
            from = lastPickedIndex + 1;
            to = lastPickedIndex + clips.Length;
        }

        int pickedIndex = UnityEngine.Random.Range(from, to) % clips.Length;
        lastPickedIndex = pickedIndex;
        return clips[pickedIndex];
    }
}
