using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using CCC.Persistence;

[CreateAssetMenu(menuName = "CCC/Audio/Audio Mixer Saver")]
public partial class AudioMixerSaver : ScriptablePersistent
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

    [SerializeField] private string fileName = "Audio Settings";
    [SerializeField] private bool loadOnInit = true;
    [SerializeField] private AudioMixer mixer;

    public override void Init(Action onComplete)
    {
        if (loadOnInit)
            Load();
        onComplete();
    }

    private Channel GetChannelFromType(ChannelType type)
    {
        return channels[type];
    }

    #region Get
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

    #region Save / Load
    private string FileName
    {
        get { return "/" + fileName + ".dat"; }
    }

    public void Load()
    {
        string savePath = Application.persistentDataPath + FileName;
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            try
            {
                SavedData = (ChannelData[])bf.Deserialize(file);
            }
            catch (Exception e)
            {
                file.Close();
                throw e;
            }

            file.Close();

            ApplyAllChannelSettings();
        }
        else
        {
            SetDefaults();
            Save();
        }
    }

    public bool Save()
    {
        try
        {
            string savePath = Application.persistentDataPath + FileName;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.OpenOrCreate);
            bf.Serialize(file, SavedData);
            file.Close();

            return true;
        }
        catch
        {
            return false;
        }
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

    public void SetDefaults()
    {
        DefaultSettings();
    }
    #endregion
}
