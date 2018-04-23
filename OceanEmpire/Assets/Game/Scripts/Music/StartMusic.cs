using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour {

    public AudioPlayable music;

    private void Start()
    {
        DefaultAudioSources.TransitionToMusic(music, true);
    }
}
