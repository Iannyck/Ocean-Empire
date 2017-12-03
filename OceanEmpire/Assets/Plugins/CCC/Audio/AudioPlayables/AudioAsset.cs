using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Asset")]
public class AudioAsset : AudioPlayable
{
    public AudioClip clip;
    public float volume;


    public override void PlayOn(AudioSource audioSource)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
