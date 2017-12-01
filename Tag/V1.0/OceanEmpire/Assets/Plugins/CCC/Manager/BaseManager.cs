using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using FullInspector;

namespace CCC.Manager
{
    /// <summary>
    /// NE PAS HÉRITER DIRECTEMENT DE CETTE CLASSE. UTILIEZ BaseManager(T)
    /// </summary>
    public abstract class BaseManager : BaseBehavior
    {
        public abstract void Init();
        public class BaseManagerEvent : UnityEvent<BaseManager> { }
        [HideInInspector]
        public BaseManagerEvent onCompleteInit = new BaseManagerEvent();
        [HideInInspector]
        public bool initComplete = false;

        protected void CompleteInit()
        {
            initComplete = true;
            onCompleteInit.Invoke(this);
        }
    }
    public abstract class BaseManager<T> : BaseManager where T :class
    {
        static public T instance;

        protected override void Awake()
        {
            base.Awake();

            if (!(this is T))
            {
                Debug.LogError("Trying to make a BaseManager<" + typeof(T).Name + "> but instance is a " + this.GetType().Name + ".");
                return;
            }

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
