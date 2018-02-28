using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OceanEmpire.AskForTimeWindow
{
    public class TimeText : MonoBehaviour
    {
        public int minimumTextCharacters;
        public char paddingCharacter;
        public int startNumber = 0;
        public int increment = 1;
        public int modulo = 24;

        public List<Text> textElements = new List<Text>();

        private int currentFirstNumber;
        public int CurrentFirstNumer { get { return currentFirstNumber; } }

        void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            currentFirstNumber = startNumber;
            ApplyToTextElements();
        }

        public void OnVerticalRewind(int amount)
        {
            currentFirstNumber = VaryNumber(currentFirstNumber, -amount);
            ApplyToTextElements();
        }

        private void ApplyToTextElements()
        {
            var value = currentFirstNumber;

            for (int i = 0; i < textElements.Count; i++)
            {
                var text = value.ToString();
                if (text.Length < minimumTextCharacters)
                    text = text.PadLeft(minimumTextCharacters, paddingCharacter);
                textElements[i].text = text;
                value = VaryNumber(value, 1);
            }
        }

        public int GetNumberAt(int elementIndex)
        {
            return VaryNumber(currentFirstNumber, elementIndex);
        }

        private int VaryNumber(int value, int deltaElement)
        {
            return (value + deltaElement * increment).Mod(modulo);
        }
    }
}
