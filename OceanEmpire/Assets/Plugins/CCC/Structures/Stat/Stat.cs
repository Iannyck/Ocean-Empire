using System;
using UnityEngine.Events;
using System.Runtime.Serialization;
using UnityEngine;

namespace CCC.Utility
{
    public enum BuffType
    {
        Flat, Percent
    }
    [Serializable]
    public class Stat<T>
    {
        public class StatEvent : UnityEvent<T> { };

        [SerializeField]
        protected T value;
        [NonSerialized]
        public StatEvent onSet = new StatEvent();

        public Stat(T value)
        {
            Set(value);
        }
        protected Stat(T value, bool dontSet)
        {

        }

        [OnDeserializing]
        protected virtual void OnLoad(StreamingContext context)
        {
            onSet = new StatEvent();
        }

        public virtual Stat<T> Set(T value)
        {
            QuickSet(value);
            return this;
        }

        protected void QuickSet(T value)
        {
            this.value = value;
            if (onSet != null)
                onSet.Invoke(value);
        }

        public System.Type Type() { return typeof(T); }

        #region operator overloading

        public static implicit operator T(Stat<T> stat)
        {
            return stat.value;
        }
        public static implicit operator string (Stat<T> stat)
        {
            return stat.ToString();
        }
        public override bool Equals(object obj)
        {
            return this == obj.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return value.ToString();
        }

        static public bool IsNull(Stat<T> a)
        {
            object c = a ?? null;
            return c == null;
        }

        public static bool operator ==(Stat<T> a, string b)
        {
            if (IsNull(a))
            {
                return b == null;
            }
            return a.ToString() == b;
        }
        public static bool operator !=(Stat<T> a, string b)
        {
            return !(a == b);
        }

        #endregion
    }
}
