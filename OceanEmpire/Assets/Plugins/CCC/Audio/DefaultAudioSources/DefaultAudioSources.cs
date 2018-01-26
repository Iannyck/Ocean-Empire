using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DG.Tweening;
using UnityEngine.Audio;
using FullInspector;
using System;
using System.Collections.Generic;
using CCC.Persistence;

public class DefaultAudioSources : MonoPersistent
{
    public AudioSource SFXSource;
    public AudioSource staticSFXSource;
    public AudioSource musicSource_1;
    public AudioSource musicSource_0;
    public int currentMusicSource = 1;
    public AudioSource voiceSource;
    private List<Coroutine> musicTransitionCalls = new List<Coroutine>();
    private List<Tween> musicTransitionTweens = new List<Tween>();

    private const Ease AUDIOFADE_EASE = Ease.OutSine;

    private static DefaultAudioSources instance;

    public override void Init(Action onComplete)
    {
        instance = this;
        onComplete();
    }

    /// <summary>
    /// Plays the audioplayable. Leave source to 'null' to play on the standard 2D SFX audiosource.
    /// </summary>
    static public void PlayStaticSFX(AudioPlayable playable, float delay = 0, float volumeMultiplier = 1, AudioSource source = null)
    {
        if (CheckResources_Instance())
            PlayNonMusic(playable, delay, volumeMultiplier, source, instance.staticSFXSource);
    }
    /// <summary>
    /// Plays the audioclip. Leave source to 'null' to play on the standard 2D SFX audiosource.
    /// </summary>
    static public void PlayStaticSFX(AudioClip clip, float delay = 0, float volume = 1, AudioSource source = null)
    {
        if (CheckResources_Instance())
            PlayNonMusic(clip, delay, volume, source, instance.staticSFXSource);
    }

    /// <summary>
    /// Plays the audioplayable. Leave source to 'null' to play on the standard 2D Voice audiosource.
    /// </summary>
    static public void PlayVoice(AudioPlayable playable, float delay = 0, float volumeMultiplier = 1, AudioSource source = null)
    {
        if (CheckResources_Instance())
            PlayNonMusic(playable, delay, volumeMultiplier, source, instance.voiceSource);
    }
    /// <summary>
    /// Plays the audioclip. Leave source to 'null' to play on the standard 2D Voice audiosource.
    /// </summary>
    static public void PlayVoice(AudioClip clip, float delay = 0, float volume = 1, AudioSource source = null)
    {
        if (CheckResources_Instance())
            PlayNonMusic(clip, delay, volume, source, instance.voiceSource);
    }

    /// <summary>
    /// Plays the audioplayable. Leave source to 'null' to play on the standard 2D SFX audiosource.
    /// </summary>
    static public void PlaySFX(AudioPlayable playable, float delay = 0, float volumeMultiplier = 1, AudioSource source = null)
    {
        if (CheckResources_Instance())
            PlayNonMusic(playable, delay, volumeMultiplier, source, instance.SFXSource);
    }
    /// <summary>
    /// Plays the audioclip. Leave source to 'null' to play on the standard 2D SFX audiosource.
    /// </summary>
    static public void PlaySFX(AudioClip clip, float delay = 0, float volume = 1, AudioSource source = null)
    {
        if (CheckResources_Instance())
            PlayNonMusic(clip, delay, volume, source, instance.SFXSource);
    }

    private static void PlayNonMusic(AudioPlayable playable, float delay, float volumeMultiplier, AudioSource source, AudioSource defaultSource)
    {
        if (playable == null)
            return;

        AudioSource theSource = source;
        if (theSource == null) theSource = defaultSource;

        if (delay > 0)
        {
            instance.StartCoroutine(PlayNonMusicIn(playable, delay, volumeMultiplier, theSource));
            return;
        }
        else
        {
            playable.PlayOn(theSource, volumeMultiplier);
        }
    }
    private static void PlayNonMusic(AudioClip clip, float delay, float volume, AudioSource source, AudioSource defaultSource)
    {
        if (clip == null)
            return;

        AudioSource theSource = source;
        if (theSource == null) theSource = defaultSource;

        if (delay > 0)
        {
            instance.StartCoroutine(PlayNonMusicIn(clip, delay, volume, theSource));
            return;
        }
        else
        {
            theSource.PlayOneShot(clip, volume);
        }
    }

    private static IEnumerator PlayNonMusicIn(AudioPlayable playable, float delay, float volumeMultiplier, AudioSource source)
    {
        yield return new WaitForSecondsRealtime(delay);
        playable.PlayOn(source, volumeMultiplier);
    }
    private static IEnumerator PlayNonMusicIn(AudioClip clip, float delay, float volume, AudioSource source)
    {
        yield return new WaitForSecondsRealtime(delay);
        source.PlayOneShot(clip, volume);
    }

    #region Music
    /// <returns>The standard source volume</returns>
    static private float Internal_PlayMusic(AudioPlayable playable, bool looping, float volumeMultiplier = 1)
    {
        if (!CheckResources_Instance() || !CheckResources_MusicSource())
            return 0;

        AudioSource source = GetAndIncrementMusicSource();
        source.volume = 1;

        if (looping)
            playable.PlayLoopedOn(source, 1);
        else
            playable.PlayOn(source);

        float stdVolume = source.volume;
        source.volume *= volumeMultiplier;
        return stdVolume;
    }
    static private void Internal_PlayMusic(AudioClip clip, bool looping, float volume)
    {
        if (!CheckResources_Instance() || !CheckResources_MusicSource())
            return;

        AudioSource source = GetAndIncrementMusicSource();

        source.volume = volume;
        source.clip = clip;
        source.loop = looping;
        source.Play();
    }

    static private void Internal_PlayMusicFaded(AudioPlayable playable, float fadeInDuration, bool looping = true, float startingVolume = 0)
    {
        if (!CheckResources_Instance() || !CheckResources_MusicSource())
            return;

        float stdVolume = Internal_PlayMusic(playable, looping, startingVolume);
        instance.musicTransitionTweens.Add(
            GetMusicSource().DOFade(stdVolume, fadeInDuration).SetEase(AUDIOFADE_EASE));
    }
    static private void Internal_PlayMusicFaded(AudioClip clip, float fadeInDuration, bool looping = true, float volume = 1, float startingVolume = 0)
    {
        if (!CheckResources_Instance() || !CheckResources_MusicSource())
            return;

        Internal_PlayMusic(clip, looping, startingVolume);
        instance.musicTransitionTweens.Add(
            GetMusicSource().DOFade(volume, fadeInDuration).SetEase(AUDIOFADE_EASE));
    }

    static private AudioSource GetAndIncrementMusicSource()
    {
        instance.currentMusicSource++;
        instance.currentMusicSource %= 2;
        return GetMusicSource();
    }
    static private AudioSource GetMusicSource()
    {
        return instance.currentMusicSource == 0 ? instance.musicSource_0 : instance.musicSource_1;
    }
    static private AudioSource GetOtherMusicSource()
    {
        return instance.currentMusicSource == 0 ? instance.musicSource_1 : instance.musicSource_0;
    }

    static private Coroutine DelayedCall(float delay, Action to)
    {
        return instance.DelayedCall(to, delay, true);
    }
    static private void CancelRoutine(Coroutine routine)
    {
        instance.StopCoroutine(routine);
    }
    static private void CancelMusicTransitionCalls()
    {
        List<Coroutine> routine = instance.musicTransitionCalls;
        for (int i = 0; i < routine.Count; i++)
        {
            CancelRoutine(routine[i]);
        }
        routine.Clear();

        List<Tween> tweens = instance.musicTransitionTweens;
        for (int i = 0; i < tweens.Count; i++)
        {
            tweens[i].Kill();
        }
        tweens.Clear();
    }
    static private void StopSource(AudioSource source)
    {
        source.Stop();
    }
    static private void StopSourceFaded(AudioSource source, float fadeDuration, Action onComplete)
    {
        if (fadeDuration > 0 && IsPlayingMusic())
        {
            instance.musicTransitionTweens.Add(
                source.DOFade(0, fadeDuration).OnComplete(delegate ()
                {
                    StopSource(source);
                    if (onComplete != null)
                        onComplete();
                }).SetEase(AUDIOFADE_EASE));
        }
        else
        {
            StopSource(source);
            if (onComplete != null)
                onComplete();
        }
    }


    static public void PlayMusic(AudioPlayable playable, bool looping = true)
    {
        StopMusic();
        Internal_PlayMusic(playable, looping);
    }
    static public void PlayMusic(AudioClip clip, bool looping = true, float volume = 1)
    {
        StopMusic();
        Internal_PlayMusic(clip, looping, volume);
    }

    static private void Internal_TransitionToMusic(Action playMusicFaded, float fadingDuration = 1.5f, float overlap = 0.5f)
    {
        if (!CheckResources_Instance() || !CheckResources_MusicSource())
            return;

        CancelMusicTransitionCalls();

        if (IsPlayingMusic())
        {
            AudioSource firstSource = GetMusicSource();
            float end1stMusicDelay = overlap < 0.5f ? 0 : (overlap - 0.5f) * 2 * fadingDuration;
            float start2ndMusicDelay = overlap > 0.5f ? 0 : (0.5f - overlap) * 2 * fadingDuration;

            instance.musicTransitionCalls.Add(
                DelayedCall(end1stMusicDelay,
                    () => StopSourceFaded(firstSource, fadingDuration, null)));
            instance.musicTransitionCalls.Add(
                DelayedCall(start2ndMusicDelay, playMusicFaded));
        }
        else
        {
            playMusicFaded();
        }
    }

    /// <summary>
    /// Transitionne vers une nouvelle musique
    /// </summary>
    /// <param name="clip">Le clip a faire jouer</param>
    /// <param name="looping">Est-ce qu'on loop</param>
    /// <param name="volume">Le volume de la 2e musique</param>
    /// <param name="fadingDuration">La duree de fading par musique. ATTENTION, ceci n'est pas necessairement == duree total de la transition</param>
    /// <param name="overlap">L'overlapping des deux musiques en transition (en %). 0 = la 1ere stoppe, puis la 2e commence.   0.5 = les 2 tansitionne en meme temps   1 = la deuxieme commence, puis la 1ere stoppe</param>
    /// <param name="startingVolume">Volume initiale de la 2e musique</param>
    static public void TransitionToMusic(AudioPlayable playable, bool looping = true, float fadingDuration = 1.5f, float overlap = 0.5f, float startingVolume = 0)
    {
        Action action = () => Internal_PlayMusicFaded(playable, fadingDuration, looping, startingVolume);
        Internal_TransitionToMusic(action, fadingDuration, overlap);
    }
    /// <summary>
    /// Transitionne vers une nouvelle musique
    /// </summary>
    /// <param name="clip">Le clip a faire jouer</param>
    /// <param name="looping">Est-ce qu'on loop</param>
    /// <param name="volume">Le volume de la 2e musique</param>
    /// <param name="fadingDuration">La duree de fading par musique. ATTENTION, ceci n'est pas necessairement == duree total de la transition</param>
    /// <param name="overlap">L'overlapping des deux musiques en transition (en %). 0 = la 1ere stoppe, puis la 2e commence.   0.5 = les 2 tansitionne en meme temps   1 = la deuxieme commence, puis la 1ere stoppe</param>
    /// <param name="startingVolume">Volume initiale de la 2e musique</param>
    static public void TransitionToMusic(AudioClip clip, bool looping = true, float volume = 1, float fadingDuration = 1.5f, float overlap = 0.5f, float startingVolume = 0)
    {
        Action action = () => Internal_PlayMusicFaded(clip, fadingDuration, looping, volume, startingVolume);
        Internal_TransitionToMusic(action, fadingDuration, overlap);
    }

    /// <summary>
    /// Stop la musique en cours.
    /// </summary>
    static public void StopMusic()
    {
        if (!CheckResources_Instance() || !CheckResources_MusicSource())
            return;

        CancelMusicTransitionCalls();

        StopSource(GetMusicSource());
    }

    /// <summary>
    /// Stop la musique en cours avec un fadeout.
    /// </summary>
    static public void StopMusicFaded(float fadeDuration = 1.5f, Action onComplete = null)
    {
        if (!CheckResources_Instance() || !CheckResources_MusicSource())
            return;

        CancelMusicTransitionCalls();

        StopSourceFaded(GetMusicSource(), fadeDuration, onComplete);
    }

    static public bool IsPlayingMusic()
    {
        if (!CheckResources_Instance() || !CheckResources_MusicSource())
            return false;

        return GetMusicSource().isPlaying;
    }
    #endregion

    #region Check Resources
    private static bool CheckResources_MusicSource()
    {
        if (instance.musicSource_0 == null || instance.musicSource_1 == null)
        {
            Debug.LogError("Il manque 1 ou 2 AudioSource de musique sur l'instance de SoundManager");
            return false;
        }

        return true;
    }
    private static bool CheckResources_VoiceSource()
    {
        if (instance.voiceSource == null)
        {
            Debug.LogError("Aucune 'Voice' AudioSource sur l'instance de SoundManager");
            return false;
        }

        return true;
    }
    private static bool CheckResources_SFXSource()
    {
        if (instance.SFXSource == null)
        {
            Debug.LogError("Aucune 'SFX' AudioSource sur l'instance de SoundManager");
            return false;
        }

        return true;
    }
    private static bool CheckResources_Instance()
    {
        if (instance == null)
        {
            Debug.LogError("Aucune instance de Sound manager.");
            return false;
        }

        return true;
    }
    #endregion
}
