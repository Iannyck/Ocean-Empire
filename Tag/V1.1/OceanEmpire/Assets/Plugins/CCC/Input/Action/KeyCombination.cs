using UnityEngine;

namespace CCC.Input.Action
{
    [System.Serializable]
    public struct KeyCombination
    {
        public KeyCode first;
        public KeyCode second;
        public KeyCombination(KeyCode first, KeyCode second = KeyCode.None) { this.first = first; this.second = second; }

        public bool Get()
        {
            if (second == KeyCode.None)
                return UnityEngine.Input.GetKey(first);
            else
                return UnityEngine.Input.GetKey(first) && UnityEngine.Input.GetKey(second);
        }
        public bool GetDown()
        {
            if (second == KeyCode.None)
                return UnityEngine.Input.GetKeyDown(first);
            else
            {
                bool fd = UnityEngine.Input.GetKeyDown(first);
                bool sd = UnityEngine.Input.GetKeyDown(second);

                if (fd && (sd || UnityEngine.Input.GetKey(second)))
                    return true;
                if (sd && (fd || UnityEngine.Input.GetKey(first)))
                    return true;
                return false;
            }
        }
        public bool GetUp()
        {
            if (second == KeyCode.None)
                return UnityEngine.Input.GetKeyUp(first);
            else
            {
                bool fu = UnityEngine.Input.GetKeyUp(first);
                bool f = UnityEngine.Input.GetKey(first);
                bool su = UnityEngine.Input.GetKeyUp(second);
                bool s = UnityEngine.Input.GetKey(second);

                return (fu && (s || su)) || (su && (f || fu));
            }
        }

        public override bool Equals(object obj)
        {
            if ((obj is KeyCombination))
                return false;
            KeyCombination other = (KeyCombination)obj;
            return other.first == first && other.second == second;
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() + second.GetHashCode();
        }

        public override string ToString()
        {
            return first.ToString() + " + " + second.ToString();
        }
    }
}