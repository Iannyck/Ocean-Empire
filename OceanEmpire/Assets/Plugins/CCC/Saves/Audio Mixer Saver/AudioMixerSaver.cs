using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using CCC.Serialization;
using CCC.Persistence;

[CreateAssetMenu(menuName = "CCC/Audio/Audio Mixer Saver")]
public partial class AudioMixerSaver : FileScriptableInterface, IPersistent
{
    [Serializable]
    public class ChannelData
    {
        public float dbBoost;
        public bool muted;
        public ChannelData(float dbBoost, bool muted)
        {
            this.dbBoost = dbBoost;
            this.muted = muted;
        }
    }
    public class Channel
    {
        public ChannelData data = new ChannelData(0, false);
        public string[] volumeParameters;

        public Channel(params string[] volumeParameters)
        {
            this.volumeParameters = volumeParameters;
        }
    }

    public const int MAX_VOLUME = 20;
    public const int MIN_VOLUME = -80;

    [SerializeField] private bool loadOnInit = true;
    [SerializeField] private AudioMixer mixer;

    #region IPersistent
    public void Init(Action onComplete)
    {
        if (loadOnInit)
            Load();
        onComplete();
    }
    public UnityEngine.Object DuplicationBehavior()
    {
        return this;
    }
    #endregion

    #region Get
    private Channel GetChannelFromType(ChannelType type)
    {
        return channels[type];
    }

    public bool GetMuted(ChannelType channelType)
    {
        return GetMuted(GetChannelFromType(channelType));
    }
    private bool GetMuted(Channel channel)
    {
        return channel.data.muted;
    }

    public float GetVolume(ChannelType channelType)
    {
        return GetVolume(GetChannelFromType(channelType));
    }
    private float GetVolume(Channel channel)
    {
        return channel.data.dbBoost;
    }
    #endregion

    #region Set
    public void SetMuted(ChannelType channelType, bool muted)
    {
        SetMuted(GetChannelFromType(channelType), muted);
    }
    private void SetMuted(Channel channel, bool muted)
    {
        channel.data.muted = muted;
        ApplyChannelSettings(channel);
    }

    public void SetVolume(ChannelType channelType, float value)
    {
        SetVolume(GetChannelFromType(channelType), value);
    }
    private void SetVolume(Channel channel, float value)
    {
        channel.data.dbBoost = value;
        ApplyChannelSettings(channel);
    }

    public void SetDefaults()
    {
        DefaultSettings();
        CoroutineLauncher.Instance.CallNextFrame(ApplyAllChannelSettings);
    }
    #endregion

    #region Apply
    private void ApplyChannelSettings(ChannelType type)
    {
        ApplyChannelSettings(GetChannelFromType(type));
    }
    private void ApplyChannelSettings(Channel channel)
    {
        var volume = channel.data.muted ? MIN_VOLUME : channel.data.dbBoost;
        foreach (var parameterName in channel.volumeParameters)
        {
            mixer.SetFloat(parameterName, volume);
        }
    }
    public void ApplyAllChannelSettings()
    {
        foreach (var pair in channels)
        {
            ApplyChannelSettings(pair.Value);
        }
    }
    #endregion

    #region File Scriptable Interface
    protected override object GetLocalData()
    {
        return SavedData;
    }
    protected override void OverwriteLocalData(object graph)
    {
        SavedData = (ChannelData[])graph;
        CoroutineLauncher.Instance.CallNextFrame(ApplyAllChannelSettings);
    }
    protected override void SetDefaultLocalData()
    {
        SetDefaults();
    }

    private ChannelData[] SavedData
    {
        get
        {
            var data = new ChannelData[channels.Count];
            int i = 0;
            foreach (var pair in channels)
            {
                data[i] = pair.Value.data;
                i++;
            }
            return data;
        }
        set
        {
            var data = value;
            int i = 0;
            foreach (var pair in channels)
            {
                pair.Value.data = data[i];
                i++;
            }
        }
    }
    #endregion
}
