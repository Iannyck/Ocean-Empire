using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField, ReadOnlyInPlayMode]
    private AudioMixerSaver.ChannelType channelType;
    [SerializeField]
    private AudioMixerSaver audioMixerSaver;

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        slider.value = GetShownVolume();
        slider.onValueChanged.AddListener(OnValueChange);
    }

    void OnDisable()
    {
        //Note: Je test si le slider n'est pas null parce qu'il vient p-e d'être détruit avec le script
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnValueChange);
    }

    private void OnValueChange(float sliderValue)
    {
        bool shouldMute = sliderValue == slider.minValue;

        if (audioMixerSaver != null)
        {
            audioMixerSaver.SetMuted(channelType, shouldMute);
            audioMixerSaver.SetVolume(channelType, sliderValue);
        }
    }

    private float GetShownVolume()
    {
        if (audioMixerSaver == null)
            return 0;
        if (audioMixerSaver.GetMuted(channelType))
        {
            return AudioMixerSaver.MIN_VOLUME;
        }
        else
        {
            return audioMixerSaver.GetVolume(channelType);
        }
    }
}