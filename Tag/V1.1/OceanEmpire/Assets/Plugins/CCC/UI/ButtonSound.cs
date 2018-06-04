using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CCC.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        public AudioClip clip;
        public AudioAsset audioAsset;

        private void Awake()
        {
            PersistentLoader.LoadIfNotLoaded();
        }

        void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveListener(OnClick);
        }

        void OnClick()
        {
            if (clip == null)
                DefaultAudioSources.PlaySFX(audioAsset);
            else
                DefaultAudioSources.PlaySFX(clip);
        }
    }
}
