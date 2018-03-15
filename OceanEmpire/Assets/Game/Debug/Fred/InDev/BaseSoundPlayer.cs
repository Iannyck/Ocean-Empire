using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSoundPlayer : MonoBehaviour
{
    private enum DefaultSourceType { SFX, SFX_Static, Music, Voice }

    [Header("Asset")]
    [SerializeField] AudioPlayable _audioPlayable;

    [Header("Audio Source")]
    [SerializeField] public bool _useDefaultSources = true;

    [HideIf("_useDefaultSources", HideShowBaseAttribute.Type.Field)]
    [SerializeField] AudioSource _localSource;

    [ShowIf("_useDefaultSources", HideShowBaseAttribute.Type.Field)]
    [SerializeField] DefaultSourceType defaultSourceType = DefaultSourceType.SFX_Static;

    public void Play()
    {
        if (_useDefaultSources)
        {
            switch (defaultSourceType)
            {
                case DefaultSourceType.SFX:
                    DefaultAudioSources.PlaySFX(_audioPlayable);
                    break;
                case DefaultSourceType.SFX_Static:
                    DefaultAudioSources.PlayStaticSFX(_audioPlayable);
                    break;
                case DefaultSourceType.Music:
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
