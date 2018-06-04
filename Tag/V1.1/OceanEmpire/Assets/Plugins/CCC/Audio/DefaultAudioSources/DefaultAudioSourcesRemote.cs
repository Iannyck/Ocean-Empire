using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAudioSourcesRemote : MonoBehaviour
{
    public void PlaySFX_AudioClip(AudioClip clip)
    {
        DefaultAudioSources.PlaySFX(clip);
    }
    public void PlaySFX_AudioPlayable(AudioPlayable playable)
    {
        DefaultAudioSources.PlaySFX(playable);
    }
    public void PlayVoice_AudioClip(AudioClip clip)
    {
        DefaultAudioSources.PlayVoice(clip);
    }
    public void PlayVoice_AudioPlayable(AudioPlayable playable)
    {
        DefaultAudioSources.PlayVoice(playable);
    }
    public void PlayStaticSFX_AudioClip(AudioClip clip)
    {
        DefaultAudioSources.PlayStaticSFX(clip);
    }
    public void PlayStaticSFX_AudioPlayable(AudioPlayable playable)
    {
        DefaultAudioSources.PlayStaticSFX(playable);
    }
    public void PlayMusic_AudioClip(AudioClip clip)
    {
        DefaultAudioSources.PlayMusic(clip);
    }
    public void PlayMusic_AudioPlayable(AudioPlayable playable)
    {
        DefaultAudioSources.PlayMusic(playable);
    }
}