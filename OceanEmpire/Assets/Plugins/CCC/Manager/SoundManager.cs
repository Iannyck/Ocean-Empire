using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DG.Tweening;
using UnityEngine.Audio;
using FullInspector;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCC.Manager
{
    public class SoundManager : BaseManager<SoundManager>
    {
        [System.Serializable]
        public struct SoundSettings
        {
            public Setting master;
            public Setting voice;
            public Setting sfx;
            public Setting music;
            public SoundSettings(Setting master, Setting voice, Setting sfx, Setting music)
            {
                this.master = master;
                this.voice = voice;
                this.sfx = sfx;
                this.music = music;
            }
        }
        [System.Serializable]
        public struct Setting
        {
            public float dbBoost;
            public bool muted;
            public Setting(float dbBoost, bool enabled)
            {
                this.dbBoost = dbBoost;
                this.muted = enabled;
            }
        }

        public OpenSavesButton openSavesLocation;
        public bool printLogs = false;
        public AudioSource SFXSource;
        public AudioSource staticSFXSource;
        public AudioSource musicSource;
        public AudioSource voiceSource;
        public AudioMixer mixer;
        public SoundSettings settings;

        public AudioMixerSnapshot[] snapshots;

        public override void Init()
        {
            Load();
            CompleteInit();
        }

        /// <summary>
        /// Plays the audioclip. Leave source to 'null' to play on the standard 2D SFX audiosource.
        /// </summary>
        static public void PlayStaticSFX(AudioClip clip, float delay = 0, float volume = 1, AudioSource source = null)
        {
            if (CheckResources_Instance())
                PlayNonMusic(clip, delay, volume, source, instance.staticSFXSource);
        }

        /// <summary>
        /// Plays the audioclip. Leave source to 'null' to play on the standard 2D Voice audiosource.
        /// </summary>
        static public void PlayVoice(AudioClip clip, float delay = 0, float volume = 1, AudioSource source = null)
        {
            if (CheckResources_Instance())
                PlayNonMusic(clip, delay, volume, source, instance.voiceSource);
        }

        /// <summary>
        /// Plays the audioclip. Leave source to 'null' to play on the standard 2D SFX audiosource.
        /// </summary>
        static public void PlaySFX(AudioClip clip, float delay = 0, float volume = 1, AudioSource source = null)
        {
            if (CheckResources_Instance())
                PlayNonMusic(clip, delay, volume, source, instance.SFXSource);
        }

        private static void PlayNonMusic(AudioClip clip, float delay, float volume, AudioSource source, AudioSource defaultSource)
        {
            if (clip == null)
                return;

            AudioSource theSource = source;
            if (theSource == null) theSource = defaultSource;

            if (delay > 0)
            {
                instance.StartCoroutine(PlayNonMusicIn(clip, delay, volume, theSource));
                return;
            }
            else
            {
                theSource.PlayOneShot(clip, volume);
            }
        }

        private static IEnumerator PlayNonMusicIn(AudioClip clip, float delay, float volume, AudioSource source)
        {
            yield return new WaitForSecondsRealtime(delay);
            source.PlayOneShot(clip, volume);
        }

        static public void PlayMusic(AudioClip clip, bool looping = true, float volume = 1, bool fadePrevious = true)
        {
            if (!CheckResources_Instance() || !CheckResources_MusicSource())
                return;

            if (fadePrevious)
            {
                StopMusic(true, delegate ()
                {
                    PlayMusic(clip, looping, volume, false);
                });
            }
            else
            {
                StopMusic(false, null);

                instance.musicSource.volume = volume;
                instance.musicSource.clip = clip;
                instance.musicSource.loop = looping;
                instance.musicSource.Play();
            }
        }

        static public void StopMusic(bool faded = false, Action onComplete = null)
        {
            if (!CheckResources_Instance() || !CheckResources_MusicSource())
                return;

            if (faded && IsPlayingMusic())
            {
                DOTween.To(() => instance.musicSource.volume, x => instance.musicSource.volume = x, 0, 0.5f).OnComplete(delegate ()
                {
                    StopMusic(false, onComplete);
                });
            }
            else
            {
                instance.musicSource.Stop();
                if (onComplete != null) onComplete();
            }
        }

        static public bool IsPlayingMusic()
        {
            if (!CheckResources_Instance() || !CheckResources_MusicSource())
                return false;

            return instance.musicSource.isPlaying;
        }

        static public void SlowMotionEffect(bool state, float timeToReach = 0.75f)
        {
            if (CheckResources_Instance() && CheckResources_Mixer())
                instance.mixer.TransitionToSnapshots(
                    instance.snapshots,
                    state ? new float[] { 0, 1 } : new float[] { 1, 0 },
                    timeToReach);
        }

        #region Settings Get/Set

        public static void SetMaster(float dbBoost)
        {
            if (!CheckResources_Instance())
                return;

            instance.settings.master.dbBoost = dbBoost;
            instance.ApplyMaster();
        }
        public static void SetMaster(bool muted)
        {
            if (!CheckResources_Instance())
                return;

            instance.settings.master.muted = muted;
            instance.ApplyMaster();
        }
        public static void SetSFX(float dbBoost)
        {
            if (!CheckResources_Instance())
                return;

            instance.settings.sfx.dbBoost = dbBoost;
            instance.ApplySFX();
        }
        public static void SetSFX(bool muted)
        {
            if (!CheckResources_Instance())
                return;

            instance.settings.sfx.muted = muted;
            instance.ApplySFX();
        }
        public static void SetVoice(float dbBoost)
        {
            if (!CheckResources_Instance())
                return;

            instance.settings.voice.dbBoost = dbBoost;
            instance.ApplyVoice();
        }
        public static void SetVoice(bool muted)
        {
            if (!CheckResources_Instance())
                return;

            instance.settings.voice.muted = muted;
            instance.ApplyVoice();
        }
        public static void SetMusic(float dbBoost)
        {
            if (!CheckResources_Instance())
                return;

            instance.settings.music.dbBoost = dbBoost;
            instance.ApplyMusic();
        }
        public static void SetMusic(bool muted)
        {
            if (!CheckResources_Instance())
                return;

            instance.settings.music.muted = muted;
            instance.ApplyMusic();
        }

        public static Setting GetSFXSetting()
        {
            if (CheckResources_Instance())
                return instance.settings.sfx;
            else
                return new Setting();
        }
        public static Setting GetMusicSetting()
        {
            if (CheckResources_Instance())
                return instance.settings.music;
            else
                return new Setting();
        }
        public static Setting GetMasterSetting()
        {
            if (CheckResources_Instance())
                return instance.settings.master;
            else
                return new Setting();
        }
        public static Setting GetVoiceSetting()
        {
            if (CheckResources_Instance())
                return instance.settings.voice;
            else
                return new Setting();
        }

        private bool MusicMuted()
        {
            return settings.music.muted || MasterMuted();
        }
        private bool SFXMuted()
        {
            return settings.sfx.muted || MasterMuted();
        }
        private bool VoiceMuted()
        {
            return settings.voice.muted || MasterMuted();
        }
        private bool MasterMuted()
        {
            return settings.master.muted;
        }

        #endregion

        #region Apply
        [InspectorButton]
        private void ApplyAll()
        {
            ApplyMaster();
            ApplyMusic();
            ApplySFX();
            ApplyVoice();
        }
        private void ApplyMaster()
        {
            if (CheckResources_Mixer())
            {
                float val = MasterMuted() ? -80 : settings.master.dbBoost;
                mixer.SetFloat("master", val);
            }
            //if (CheckResources_MusicSource())
            //    musicSource.mute = MusicMuted();
            //if (CheckResources_SFXSource())
            //    sfxStdSource.mute = SFXMuted();
            //if (CheckResources_VoiceSource())
            //    voiceSource.mute = VoiceMuted();
        }
        private void ApplySFX()
        {
            if (CheckResources_Mixer())
            {
                float val = SFXMuted() ? -80 : settings.sfx.dbBoost;
                mixer.SetFloat("sfx", val);
                mixer.SetFloat("static sfx", val);
            }
            //if (CheckResources_SFXSource())
            //    sfxStdSource.mute = MusicMuted();
        }
        private void ApplyVoice()
        {
            if (CheckResources_Mixer())
            {
                float val = VoiceMuted() ? -80 : settings.voice.dbBoost;
                mixer.SetFloat("voice", val);
            }
            //if (CheckResources_VoiceSource())
            //    voiceSource.mute = VoiceMuted();
        }
        private void ApplyMusic()
        {
            if (CheckResources_Mixer())
            {
                float val = MusicMuted() ? -80 : settings.music.dbBoost;
                mixer.SetFloat("music", val);
            }
            //if (CheckResources_MusicSource())
            //    musicSource.mute = MusicMuted();
        }
        #endregion

        #region Load/Save
        private const string SaveExtension = "/Sound_v2.dat";

        public static void Load()
        {
            if (!CheckResources_Instance())
                return;
            instance.Load_Instance();
        }
        [InspectorButton, InspectorName("Load")]
        private void Load_Instance()
        {
            string savePath = Application.persistentDataPath + SaveExtension;
            if (File.Exists(savePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(savePath, FileMode.Open);
                SoundSettings saveCopy = (SoundSettings)bf.Deserialize(file);

                settings = saveCopy;

                file.Close();

                ApplyAll();
                Log("Sound settings loaded.");
            }
            else
            {
                SetDefaults_Instance();
                Save_Instance();
            }
        }

        public static void Save()
        {
            if (!CheckResources_Instance())
                return;
            instance.Save_Instance();
        }
        [InspectorButton, InspectorName("Save")]
        private void Save_Instance()
        {
            string savePath = Application.persistentDataPath + SaveExtension;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.OpenOrCreate);
            bf.Serialize(file, settings);
            file.Close();
            Log("Sound settings saved.");
        }

        public static void SetDefaults()
        {
            if (!CheckResources_Instance())
                return;
            instance.SetDefaults_Instance();
        }
        [InspectorButton, InspectorName("Set Defaults")]
        private void SetDefaults_Instance()
        {
            settings = GetDefaultSettings();
            Log("Default sound settings.");

            ApplyAll();
        }


        private static SoundSettings GetDefaultSettings()
        {
            return new SoundSettings(
                new Setting(0, false),       //Master
                new Setting(0, false),       //Voice
                new Setting(0, false),       //SFX
                new Setting(0, false));      //Music
        }
        #endregion

        #region Check Resources
        private static bool CheckResources_Mixer()
        {
            if (instance.mixer == null)
            {
                Debug.LogError("Aucun AudioMixer sur l'instance de SoundManager");
                return false;
            }

            return true;
        }
        private static bool CheckResources_MusicSource()
        {
            if (instance.musicSource == null)
            {
                Debug.LogError("Aucune 'Music' AudioSource sur l'instance de SoundManager");
                return false;
            }

            return true;
        }
        private static bool CheckResources_VoiceSource()
        {
            if (instance.voiceSource == null)
            {
                Debug.LogError("Aucune 'Voice' AudioSource sur l'instance de SoundManager");
                return false;
            }

            return true;
        }
        private static bool CheckResources_SFXSource()
        {
            if (instance.SFXSource == null)
            {
                Debug.LogError("Aucune 'SFX' AudioSource sur l'instance de SoundManager");
                return false;
            }

            return true;
        }
        private static bool CheckResources_Instance()
        {
            if (instance == null)
            {
                Debug.LogError("Aucune instance de Sound manager.");
                return false;
            }

            return true;
        }
        #endregion

        #region Log
        private void Log(string message)
        {
            if (printLogs)
                Debug.Log(message);
        }
        private void LogWarning(string message)
        {
            if (printLogs)
                Debug.LogWarning(message);
        }
        private void LogError(string message)
        {
            if (printLogs)
                Debug.LogError(message);
        }
        #endregion
    }
}
