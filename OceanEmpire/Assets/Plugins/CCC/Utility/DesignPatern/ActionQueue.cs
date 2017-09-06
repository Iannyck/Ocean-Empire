using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

namespace CCC.Utility
{
    // Permet d'effectuer une série d'action une à la suite de l'autre
    public class ActionQueue : MonoBehaviour
    {
        public class ActionEvent: UnityEvent<Action> { }

        public struct Action
        {
            public UnityAction action;
            public Transform target;
        }

        List<Action> list;
        public UnityEvent onQueueCompleted;
        public ActionEvent onNextItem = new ActionEvent();
        public bool AutoPlay = true;

        private bool isPlaying = false;
        /// <summary>
        /// Es ce qu'une action est en train d'être effectué ?
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            private set
            {
                isPlaying = value;
            }
        }

        void Awake()
        {
            list = new List<Action>();
        }

        /// <summary>
        /// Retourne le nombre d'actions qui reste à terminer
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return list.Count;
        }

        /// <summary>
        /// Ajoute une action à effectuer de facon à la prioriser
        /// </summary>
        /// <param name="action"></param>
        public void Prioritize(UnityAction action)
        {
            int moved = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].action == action)
                {
                    if (i != 0)
                    {
                        Action priority = list[i];
                        list[i] = list[moved];
                        list[moved] = priority;
                    }
                    moved++;
                }
            }
        }

        /// <summary>
        /// Ajoute une action à effectuer
        /// </summary>
        /// <param name="action"></param>
        /// <param name="target"></param>
        public void Add(UnityAction action, Transform target = null)
        {
            Action act = new Action();
            act.action = action;
            act.target = target;

            list.Add(act);

            if (AutoPlay && list.Count == 1) NextItem();
        }

        /// <summary>
        /// Effectuer l'action en cours
        /// </summary>
        public void Play()
        {
            if (list.Count > 0 && !IsPlaying) NextItem();
        }

        /// <summary>
        /// On applique l'action et on start l'évennement relié à 
        /// l'accomplisement d'une action de la liste
        /// </summary>
        private void NextItem()
        {
            IsPlaying = true;
            list[0].action.Invoke();

            onNextItem.Invoke(list[0]);
        }

        /// <summary>
        /// Retire l'élément courrant de la liste et on passe au prochain s'il y en a un
        /// </summary>
        public void EndItem()
        {
            if (list.Count <= 0) return;

            list.RemoveAt(0);

            if (list.Count > 0) NextItem();
            else
            {
                isPlaying = false;
                onQueueCompleted.Invoke();
            }
        }


    }
}
