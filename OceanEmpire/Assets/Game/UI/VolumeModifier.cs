using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class VolumeModifier : MonoBehaviour
{
    public Toggle sfxToggle;
    public Toggle musicToggle;

    void Awake()
    {
        sfxToggle.isOn = SoundManager.GetSFXSetting().muted;
        sfxToggle.onValueChanged.AddListener(delegate (bool newValue)
        {
            SoundManager.SetSFX(newValue);
        });
        musicToggle.isOn = SoundManager.GetMusicSetting().muted;
        musicToggle.onValueChanged.AddListener(delegate (bool newValue)
        {
            SoundManager.SetMusic(newValue);
        });
    }
}