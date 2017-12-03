using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Audio/Audio Asset Group")]
public class AudioAssetGroup : AudioPlayable
{
    public AudioAsset[] clips;

    [NonSerialized]
    private int lastPickedIndex = -1;

    public override void PlayOn(AudioSource audioSource)
    {
        if (clips == null || clips.Length == 0)
            return;

        int index = PickClip();
        clips[index].PlayOn(audioSource);
        lastPickedIndex = index;
    }

    private int PickClip()
    {
        if (lastPickedIndex >= clips.Length)
            lastPickedIndex = 0;

        if (clips.Length == 1)
            return 1;


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

        return UnityEngine.Random.Range(from, to) % clips.Length;
    }
}
