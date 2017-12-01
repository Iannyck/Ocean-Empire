using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using FullInspector;

namespace CCC.Input
{
    public class KeyBindingEntry : BaseBehavior
    {
        public class EntryEvent : UnityEvent<KeyBindingEntry> { }

        [InspectorHeader("Settings")]
        public string notBindedText = "Not binded";

        [InspectorHeader("UI")]
        public Text text;
        public Button primary;
        public Text primaryText;

        [InspectorHeader("Optional UI")]
        public Button secondary;
        public Text secondaryText;
        [NotSerialized]
        public EntryEvent onClick = new EntryEvent();

        [NotSerialized]
        public KeyValuePair<string, InputKey> key;
        [NotSerialized]
        public int pressedKey = -1;
        [NotSerialized]
        public int mappingIndex = -1;

        void Start()
        {
            primary.onClick.AddListener(OnFirstClick);
            if (secondary != null)
                secondary.onClick.AddListener(OnSecondClick);
        }

        void OnFirstClick()
        {
            pressedKey = 0;
            onClick.Invoke(this);
        }

        void OnSecondClick()
        {
            pressedKey = 1;
            onClick.Invoke(this);
        }

        public void Rewrite()
        {
            primaryText.text = ((key.Value.primary.first == KeyCode.None) ? notBindedText : key.Value.primary.first.ToString())
                +
                ((key.Value.primary.second == KeyCode.None) ? "" : " + " + key.Value.primary.second.ToString());
            if (secondaryText != null)
                secondaryText.text = ((key.Value.secondary.first == KeyCode.None) ? notBindedText : key.Value.secondary.first.ToString())
                    +
                    ((key.Value.secondary.second == KeyCode.None) ? "" : " + " + key.Value.secondary.second.ToString());
        }

        public void Fill(KeyValuePair<string, InputKey> key, int mappingIndex)
        {
            this.mappingIndex = mappingIndex;
            this.key = key;
            this.text.text = key.Key;
            Rewrite();
        }

        private string KeyCodeToString(KeyCode key)
        {
            return key == KeyCode.None ? "" : key.ToString();
        }

    }
}
