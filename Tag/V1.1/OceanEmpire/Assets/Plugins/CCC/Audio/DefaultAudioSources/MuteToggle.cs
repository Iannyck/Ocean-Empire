using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
    [SerializeField] bool checkedMeansMuted = true;
    [SerializeField] Toggle toggle;
    [SerializeField] AudioMixerSaver mixerSaver;
    public AudioMixerSaver.ChannelType channelType;


    private void Awake()
    {
        toggle.onValueChanged.AddListener(Mute);
        toggle.isOn = checkedMeansMuted ? mixerSaver.GetMuted(channelType) : !mixerSaver.GetMuted(channelType);
    }

    protected void Mute(bool state)
    {
        mixerSaver.SetMuted(channelType, checkedMeansMuted ? state : !state);
        mixerSaver.Save();
    }
}
