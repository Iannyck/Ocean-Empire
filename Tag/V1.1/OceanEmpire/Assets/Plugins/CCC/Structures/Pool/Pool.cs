using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.DesignPattern
{
    public abstract class Pool<T> : MonoBehaviour where T : Pool<T>.PoolItem
    {
        protected Queue<T> deactivatedPool = new Queue<T>();

        private void AddToPool(T item)
        {
            item.Deactivate();
            deactivatedPool.Enqueue(item);
        }

        protected T GetFromPool()
        {
            T item = null;
            if (deactivatedPool.Count <= 0)
            {
                item = NewItem();
                item.pooler = this;
            }
            else
            {
                item = deactivatedPool.Dequeue();
            }
            item.Activate();
            return item;
        }

        protected abstract T NewItem();

        public abstract class PoolItem : MonoBehaviour
        {
            [System.NonSerialized]
            public Pool<T> pooler;

            protected void PutBackIntoPool()
            {
                pooler.AddToPool((T)this);
            }
            public abstract void Activate();
            public abstract void Deactivate();
        }
    }

}