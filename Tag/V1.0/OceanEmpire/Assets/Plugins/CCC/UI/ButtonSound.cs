using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CCC.Manager;

namespace CCC.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        public AudioClip clip;
        void Start()
        {
            MasterManager.Sync();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        void OnClick()
        {
            SoundManager.PlaySFX(clip);
        }
    }
}
