using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private enum DefaultSourceType { SFX, SFX_Static, Music, Voice }

    [Header("Asset")]
    [SerializeField] AudioPlayable _audioPlayable;

    [Header("Audio Source")]
    [SerializeField] public bool _useDefaultSources = true;

    [HideIf("_useDefaultSources", HideShowBaseAttribute.Type.Field)]
    [SerializeField] AudioSource _localSource;

    [ShowIf("_useDefaultSources", HideShowBaseAttribute.Type.Field)]
    [SerializeField] DefaultSourceType _defaultSourceType = DefaultSourceType.SFX_Static;


    [ShowIf("GonnaPlayMusic", HideShowBaseAttribute.Type.Property)]
    [SerializeField] bool _transitionSmoothly = false;

    private bool GonnaPlayMusic
    {
        get
        {
            return _useDefaultSources && _defaultSourceType == DefaultSourceType.Music;
        }
    }


    public virtual void Play()
    {
        if (_useDefaultSources)
        {
            switch (_defaultSourceType)
            {
                case DefaultSourceType.SFX:
                    DefaultAudioSources.PlaySFX(_audioPlayable);
                    break;
                case DefaultSourceType.SFX_Static:
                    DefaultAudioSources.PlayStaticSFX(_audioPlayable);
                    break;
                case DefaultSourceType.Music:
                    if (_transitionSmoothly)
                        DefaultAudioSources.TransitionToMusic(_audioPlayable);
                    else
                        DefaultAudioSources.PlayMusic(_audioPlayable);
                    break;
                case DefaultSourceType.Voice:
                    DefaultAudioSources.PlayVoice(_audioPlayable);
                    break;
            }
        }
        else
        {
            _audioPlayable.PlayOn(_localSource);
        }
    }
}
