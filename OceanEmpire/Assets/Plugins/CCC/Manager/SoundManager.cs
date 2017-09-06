using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCC.Manager
{
    public class SoundManager : BaseManager<SoundManager>
    {
        [System.Serializable]
        public class VolumeSave
        {
            //Value '0' is the default setting. 
            public float master = 0;
            public float voice = 0;
            public float sfx = 0;
            public float music = 0;
            public bool activeSfx = true;
            public bool activeMusic = true;
        }

        public AudioSource stdSource;
        public AudioSource musicSource;
        public AudioMixer mixer;
        public VolumeSave save;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
            save.activeSfx = true;
            save.activeMusic = true;
        }

        public override void Init()
        {
            Load();
            CompleteInit();
        }

        /// <summary>
        /// Plays the audioclip. Leave source to 'null' to play on the standard 2D SFX audiosource.
        /// </summary>
        public static void PlaySFX(AudioClip clip, float delay = 0, float volume = 1, AudioSource source = null)
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }

            if (!instance.save.activeSfx)
                return;

            if (clip == null) return;
            if (delay > 0)
            {
                instance.StartCoroutine(instance.PlayIn(clip, delay, volume, source));
                return;
            }
            AudioSource theSource = source;
            if (theSource == null) theSource = instance.stdSource;
            
            theSource.PlayOneShot(clip, volume); //avant stdSource.PlayOneShot(clip, delay); 
        }

        public static void PlayMusic(AudioClip clip, bool looping = true, float volume = 1, bool faded = false)
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }

            if (!instance.save.activeMusic)
                return;

            if (faded)
            {
                StopMusic(true, delegate ()
                {
                    PlayMusic(clip, looping, volume);
                });
            }
            else
            {
                instance.musicSource.volume = volume;
                StopMusic();
                instance.musicSource.clip = clip;
                instance.musicSource.loop = looping;
                instance.musicSource.Play();
            }
        }

        public static void StopMusic(bool faded = false, TweenCallback onComplete = null)
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }

            if (faded)
            {
                DOTween.To(() => instance.musicSource.volume, x => instance.musicSource.volume = x, 0, 0.5f).OnComplete(delegate ()
                {
                    StopMusic(false, onComplete);
                });
            }
            else
            {
                instance.musicSource.Stop();
                if (onComplete != null) onComplete.Invoke();
            }
        }

        IEnumerator PlayIn(AudioClip clip, float delay, float volume = 1, AudioSource source = null)
        {
            yield return new WaitForSecondsRealtime(delay);
            PlaySFX(clip, 0, volume, source);
        }

        static public bool IsPlayingMusic()
        {
            return instance.musicSource.isPlaying;
        }

        #region Volume Set

        public static void SetMaster(float value)
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }
            if (instance.mixer == null) return;

            instance.save.master = value;
            instance.mixer.SetFloat("master", value);
        }
        public static void SetVoice(float value)
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }
            if (instance.mixer == null) return;

            instance.save.voice = value;
            instance.mixer.SetFloat("voice", value);
        }
        public static void SetMusic(float value)
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }
            if (instance.mixer == null) return;

            instance.save.music = value;
            instance.mixer.SetFloat("music", value);
        }
        public static void SetSfx(float value)
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }
            if (instance.mixer == null) return;

            instance.save.sfx = value;
            instance.mixer.SetFloat("sfx", value);
        }
        public static void SetActiveSFX(bool value)
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }
            if (instance.mixer == null) return;

            instance.save.activeSfx = value;
        }
        public static void SetActiveMusic(bool value)
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }
            if (instance.mixer == null) return;

            instance.save.activeMusic = value;
        }

        public static float GetMaster()
        {
            return instance.save.master;
        }

        public static float GetMusic()
        {
            return instance.save.music;
        }

        public static float GetVoice()
        {
            return instance.save.voice;
        }

        public static float GetSfx()
        {
            return instance.save.sfx;
        }

        public static bool GetActiveSfx()
        {
            return instance.save.activeSfx;
        }

        public static bool GetActiveMusic()
        {
            return instance.save.activeMusic;
        }

        private void ApplyAll()
        {
            if (instance.mixer == null) return;

            mixer.SetFloat("master", save.master);
            mixer.SetFloat("sfx", save.sfx);
            mixer.SetFloat("voice", save.voice);
            mixer.SetFloat("music", save.music);
        }

        #endregion

        #region Load/Save

        public static void Load()
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }

            string savePath = Application.persistentDataPath + "/Sound.dat";
            if (File.Exists(savePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(savePath, FileMode.Open);
                VolumeSave saveCopy = (VolumeSave)bf.Deserialize(file);
                instance.save.master = saveCopy.master;
                instance.save.voice = saveCopy.voice;
                instance.save.sfx = saveCopy.sfx;
                instance.save.music = saveCopy.music;
                instance.save.activeMusic = saveCopy.activeMusic;
                instance.save.activeSfx = saveCopy.activeSfx;
                file.Close();
            }
            else
            {
                instance.save = new VolumeSave();
                Save();
            }

            instance.ApplyAll();
        }

        public static void Save()
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }

            string savePath = Application.persistentDataPath + "/Sound.dat";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.OpenOrCreate);
            bf.Serialize(file, instance.save);
            file.Close();
        }

        public static void Clear()
        {
            if (instance == null) { Debug.LogError("SoundManager instance is null"); return; }

            instance.save = new VolumeSave();
            Save();
        }

        #endregion


#if UNITY_EDITOR
        [CustomEditor(typeof(SoundManager))]
        public class SoundManagerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                //SoundManager manager = target as SoundManager;

                if (Application.isPlaying)
                {
                    if (GUILayout.Button("Clear"))
                    {
                        SoundManager.Clear();
                    }
                    if (GUILayout.Button("Save"))
                    {
                        SoundManager.Save();
                    }
                    if (GUILayout.Button("Load"))
                    {
                        SoundManager.Load();
                    }
                }
            }
        }
#endif
    }
}
