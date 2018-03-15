using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSoundPlayer : MonoBehaviour
{
    [SerializeField] bool _useDefaultSources;
    [SerializeField] AudioSource _localSource;
    [SerializeField] AudioPlayable _playable;
}
