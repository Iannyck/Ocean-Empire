using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CCC.Input
{
    public class KeyBindingHeader : MonoBehaviour
    {
        public class HeaderEvent : UnityEvent<KeyBindingHeader> { }
        
        public Button button;
        public Text text;
        public CCC.Animation.BaseAnimator highlightAnimation;

        private int index;
        public HeaderEvent onClick = new HeaderEvent();
        public int Index
        {
            get { return index; }
        }

        void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            onClick.Invoke(this);
        }

        public void Fill(string text, int index)
        {
            this.index = index;
            this.text.text = text;
        }

        public void Highlight(bool state)
        {
            if (highlightAnimation != null)
                highlightAnimation.Animate(gameObject, state);
        }
    }

}