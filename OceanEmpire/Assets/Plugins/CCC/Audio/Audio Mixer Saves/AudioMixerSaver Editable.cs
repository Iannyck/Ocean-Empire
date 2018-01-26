using System.Collections.Generic;

public partial class AudioMixerSaver
{
    public enum ChannelType
    {
        Master,
        Voice,
        SFX,
        Music
    }
    private Dictionary<ChannelType, Channel> channels = new Dictionary<ChannelType, Channel>()
    {
        {ChannelType.Master, new Channel("master") },
        {ChannelType.Voice, new Channel("voice") },
        {ChannelType.SFX, new Channel("sfx", "static sfx") },
        {ChannelType.Music, new Channel("music") }
    };

    private void DefaultSettings()
    {
        foreach (var pair in channels)
        {
            var data = pair.Value.data;
            data.dbBoost = 0;
            data.muted = false;
        }
    }
}

