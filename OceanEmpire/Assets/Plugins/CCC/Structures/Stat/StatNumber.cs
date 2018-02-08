using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;


namespace CCC.Utility
{

    public enum BoundMode
    {
        Cap, MaxLoop, MinLoop, BidirectionalLoop
    }
    [Serializable]
    public abstract class StatNumber<T> : Stat<T> where T : IComparable, IEquatable<T>
    {
        [Serializable]
        public class Buff
        {
            public T value;
            public BuffType type;
            public Buff(T value, BuffType type)
            {
                this.value = value;
                this.type = type;
            }
        }

        [SerializeField]
        T min;
        [SerializeField]
        T max;
        [SerializeField]
        T addedFlat;
        [SerializeField]
        bool minSet = false;
        [SerializeField]
        bool maxSet = false;
        public bool beforeBuffsByDefault = true;
        public BoundMode boundMode = BoundMode.Cap;
        Dictionary<string, Buff> buffs = new Dictionary<string, Buff>();
        public List<string> BuffNames
        {
            get { return new List<string>(buffs.Keys); }
        }
        public List<Buff> Buffs
        {
            get { return new List<Buff>(buffs.Values); }
        }

        [NonSerialized]
        public StatEvent onMinReached = new StatEvent();
        [NonSerialized]
        public StatEvent onMaxReached = new StatEvent();
        [NonSerialized]
        public StatEvent onMinSet = new StatEvent();
        [NonSerialized]
        public StatEvent onMaxSet = new StatEvent();

        protected override void OnLoad(StreamingContext context)
        {
            base.OnLoad(context);
            onMinReached = new StatEvent();
            onMaxReached = new StatEvent();
            onMinSet = new StatEvent();
            onMaxSet = new StatEvent();
        }

        public T MAX
        {
            get { return max; }
            set
            {
                max = value;
                maxSet = true;
                if (onMaxSet != null)
                    onMaxSet.Invoke(value);
                Set(this.value);
            }
        }
        public T MIN
        {
            get { return min; }
            set
            {
                min = value;
                minSet = true;
                if (onMinSet != null)
                    onMinSet.Invoke(value);
                Set(this.value);
            }
        }

        public StatNumber(T value) : base(value, true)
        {
            boundMode = BoundMode.Cap;
            Set(value);
        }

        public StatNumber(T value, T min, T max, BoundMode boundMode) : base(value, true)
        {
            this.boundMode = boundMode;
            //set min
            this.min = min;
            minSet = true;
            //set max
            this.max = max;
            maxSet = true;
            Set(value);
        }

        public new StatNumber<T> Set(T value)  // C'est normal de cacher la fonction de base
        {
            return Set(value, beforeBuffsByDefault);
        }

        public StatNumber<T> Set(T value, bool beforeBuffs)
        {
            if (beforeBuffs)
            {
                T delta = Substract(value, this.value);
                Set(Add(this.value, ApplyAllBuffs(delta)), false); //Applique les buffs au delta
                return this;
            }


            if (minSet && (min.CompareTo(value) > 0 || min.Equals(value)))            // Check min
            {
                //Min overflow ?
                if (min.CompareTo(value) > 0)
                {
                    if ((boundMode == BoundMode.MinLoop || boundMode == BoundMode.BidirectionalLoop) && maxSet)
                    {
                        T delta = Substract(MIN, value); // (MIN - value)
                        Set(Substract(MAX, delta), false); // équivaut à MAX - (MIN - value)
                    }
                    else
                        QuickSet(MIN);
                }
                else
                    QuickSet(value);

                if (onMinReached != null)
                    onMinReached.Invoke(value);
            }
            else if (maxSet && (max.CompareTo(value) < 0 || max.Equals(value)))       // Check max
            {
                //Max overflow ?
                if (max.CompareTo(value) < 0)
                {
                    if ((boundMode == BoundMode.MaxLoop || boundMode == BoundMode.BidirectionalLoop) && minSet)
                    {
                        T delta = Substract(value, MAX); // value - MAX
                        Set(Add(delta, MIN), false); // équivaut à MIN + (value - MAX)
                    }
                    else
                        QuickSet(MAX);
                }
                else
                    QuickSet(value);

                if (onMaxReached != null)
                    onMaxReached.Invoke(value);
            }
            else QuickSet(value);

            return this;
        }

        public void PrintTest()
        {
            Debug.Log("value: " + value);
            foreach (KeyValuePair<string, Buff> buff in buffs)
            {
                Debug.Log("id: " + buff.Key + "  effect: " + buff.Value.value + " / " + buff.Value.type);
            }
        }


        /// <summary>
        /// Test if the 'set value' is within min/max range. Returns false if out-of-bounds
        /// </summary>
        public bool TestSet(T value)
        {
            if (minSet && min.CompareTo(value) > 0) return false;
            else if (maxSet && max.CompareTo(value) < 0) return false;
            return true;
        }


        /// <summary>
        /// Add buff that can later be removed. For 'percent' types, use whole numbers. ex: 35 -> +35%
        /// </summary>
        public bool AddBuff(string id, T value, BuffType type = BuffType.Percent)
        {
            if (buffs.ContainsKey(id))
                return false;
            buffs.Add(id, new Buff(value, type));
            switch (type)
            {
                default:
                case BuffType.Flat:
                    Set(Add(value, this.value), false);
                    addedFlat = Add(addedFlat, value);
                    break;
                case BuffType.Percent:
                    Set(Add(ApplyPercentBuff(Substract(this.value, addedFlat), value), addedFlat), false);
                    break;
            }
            return true;
        }

        public bool RemoveBuff(string id)
        {
            if (!buffs.ContainsKey(id))
                return false;

            Buff buff = buffs[id];
            switch (buff.type)
            {
                default:
                case BuffType.Flat:
                    Set(Substract(value, buff.value), false);
                    addedFlat = Substract(addedFlat, buff.value);
                    break;
                case BuffType.Percent:
                    Set(Add(ApplyPercentBuff(Substract(value,addedFlat), buff.value, true), addedFlat), false);
                    break;
            }
            buffs.Remove(id);
            return true;
        }

        private T ApplyAllBuffs(T val)
        {
            foreach (KeyValuePair<string, Buff> buff in buffs)
            {
                if (buff.Value.type == BuffType.Percent)         //Equivaux à: (value * (100+buff)) / 100
                {
                    val = ApplyPercentBuff(val, buff.Value.value);
                }
            }
            return val;
        }

        private T ApplyPercentBuff(T val, T percentBuff, bool invert = false)
        {
            T oneHundred = OneHundred();
            T b = Add(percentBuff, oneHundred);
            if (!invert)
            {
                val = Multiply(b, val);
                val = Divide(val, oneHundred);
            }
            else
            {
                val = Multiply(oneHundred, val);
                val = Divide(val, b);
            }

            return val;
        }

        private T DeltaMinMax()
        {
            return Substract(max, min);
        }

        #region + - * /

        protected abstract T OneHundred();
        protected abstract T Add(T value, T to);
        protected abstract T Substract(T value, T by);
        protected abstract T Multiply(T value, T by);
        protected abstract T Divide(T value, T by);
        protected abstract T Modulo(T value, T modulo);

        #endregion

        #region Operator overloads
        public static T operator +(StatNumber<T> a, StatNumber<T> b)
        {
            return a.Add(a, b);
        }
        public static T operator -(StatNumber<T> a, StatNumber<T> b)
        {
            return a.Substract(a, b);
        }
        public static T operator *(StatNumber<T> a, StatNumber<T> b)
        {
            return a.Multiply(a, b);
        }
        public static T operator /(StatNumber<T> a, StatNumber<T> b)
        {
            return a.Divide(a, b);
        }
        public static T operator %(StatNumber<T> a, StatNumber<T> b)
        {
            return a.Modulo(a, b);
        }

        #endregion
    }
}