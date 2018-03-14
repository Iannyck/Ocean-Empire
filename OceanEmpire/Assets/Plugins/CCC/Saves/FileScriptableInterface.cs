using CCC.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Serialization
{
    public abstract class FileScriptableInterface : ScriptableObject
    {
        [Suffix(FILE_EXTENSION), SerializeField] protected string fileName = "someData";
        public const string FILE_EXTENSION = ".dat";

        public delegate void SaverEvent();

        /// <summary>
        /// Évenement appelé lorsqu'on réassigne les données (load/clear)
        /// </summary>
        public event SaverEvent OnReassignData;

        /// <summary>
        /// Read/Write operation queue. C'est une queue qui assure l'ordonnancement des opérations read/write
        /// </summary>
        private Queue<Action> rwoQueue = new Queue<Action>();

        private bool lateSave = false;
        private Queue<Action> pendingLateSaveCallbacks = new Queue<Action>();

        private bool lateLoad = false;
        private Queue<Action> pendingLateLoadCallbacks = new Queue<Action>();

        public string CompletePath
        {
            get { return Application.persistentDataPath + "/" + fileName + FILE_EXTENSION; }
        }

        /// <summary>
        /// Retourne vrai si l'objet a Load au moins une fois à date
        /// </summary>
        public bool HasEverLoaded { get; private set; }


        private void _OverwriteLocalData(object graph)
        {
            OverwriteLocalData(graph);
            if (OnReassignData != null)
                OnReassignData();
        }
        protected void _SetDefaultLocalData()
        {
            if (OnReassignData != null)
                OnReassignData();
            SetDefaultLocalData();
        }
        protected abstract void OverwriteLocalData(object graph);
        protected abstract void SetDefaultLocalData();
        protected abstract object GetLocalData();

        private void OnEnable()
        {
            lateSave = false;
        }

        /// <summary>
        /// Semblable au SaveAsync, mais on va laisser 1 frame s'écouler avant.
        /// <para/>
        /// Si LateSave est appelé plusieur fois, l'objet ne sera que sauvegarder 1 fois.
        /// Ça nous permet d'appeler plusieur fois LateSave par frame sans avoir à se soucier
        /// du coût de performance de plusieurs sauvegardes.
        /// </summary>
        public void LateSave() { LateSave(null); }
        /// <summary>
        /// Semblable au SaveAsync, mais on va laisser 1 frame s'écouler avant.
        /// <para/>
        /// Si LateSave est appelé plusieur fois, l'objet ne sera que sauvegarder 1 fois.
        /// Ça nous permet d'appeler plusieur fois LateSave par frame sans avoir à se soucier
        /// du coût de performance de plusieurs sauvegardes.
        /// </summary>
        public void LateSave(Action callback)
        {
            //Add callback to queue
            if (callback != null)
                pendingLateSaveCallbacks.Enqueue(callback);

            if (lateSave)
                return;
            lateSave = true;

            CoroutineLauncher.Instance.CallNextFrame(() =>
            {
                SaveAsync(() =>
                {
                    //Clear all pending callbacks
                    while(pendingLateSaveCallbacks.Count > 0)
                    {
                        pendingLateSaveCallbacks.Dequeue().Invoke();
                    }
                    lateSave = false;
                });
            });
        }

        /// <summary>
        /// Semblable au LoadAsync, mais on va laisser 1 frame s'écouler avant.
        /// <para/>
        /// Si LateLoad est appelé plusieur fois, l'objet ne sera que loadé 1 fois.
        /// Ça nous permet d'appeler plusieur fois LateLoad par frame sans avoir à se soucier
        /// du coût de performance de plusieurs loading.
        /// </summary>
        public void LateLoad() { LateLoad(null); }
        /// <summary>
        /// Semblable au LoadAsync, mais on va laisser 1 frame s'écouler avant.
        /// <para/>
        /// Si LateLoad est appelé plusieur fois, l'objet ne sera que loadé 1 fois.
        /// Ça nous permet d'appeler plusieur fois LateLoad par frame sans avoir à se soucier
        /// du coût de performance de plusieurs loading.
        /// </summary>
        public void LateLoad(Action callback)
        {
            //Add callback to queue
            if (callback != null)
                pendingLateLoadCallbacks.Enqueue(callback);

            if (lateLoad)
                return;
            lateLoad = true;

            CoroutineLauncher.Instance.CallNextFrame(() =>
            {
                LoadAsync(() =>
                {
                    //Clear all pending callbacks
                    while (pendingLateLoadCallbacks.Count > 0)
                    {
                        pendingLateLoadCallbacks.Dequeue().Invoke();
                    }
                    lateLoad = false;
                });
            });
        }

        #region Save/Load
        public void LoadAsync() { LoadAsync(null); }
        public void LoadAsync(Action onLoadComplete)
        {
            AddRWOperation(() =>
            {
                var path = CompletePath;

                //Exists ?
                if (Saves.Exists(path))
                    Saves.ThreadLoad(path,
                        delegate (object graph)
                        {
                            _OverwriteLocalData(graph);
                            HasEverLoaded = true;

                            if (onLoadComplete != null)
                                onLoadComplete();

                            CompleteRWOperation();
                        });
                else
                {
                    //Nouveau fichier !
                    _SetDefaultLocalData();
                    SaveAsync(onLoadComplete);

                    CompleteRWOperation();
                }
            });
        }

        public void Load() { Load(null); }
        public void Load(Action onLoadComplete)
        {
            AddRWOperation(() =>
            {
                var path = CompletePath;

                //Exists ?
                if (Saves.Exists(path))
                {
                    //Load and apply
                    object graph = Saves.InstantLoad(path);
                    _OverwriteLocalData(graph);
                    HasEverLoaded = true;

                    if (onLoadComplete != null)
                        onLoadComplete();
                }
                else
                {
                    //Nouveau fichier !
                    _SetDefaultLocalData();

                    Save(onLoadComplete);
                }

                CompleteRWOperation();
            });
        }

        public void SaveAsync() { SaveAsync(null); }
        public void SaveAsync(Action onSaveComplete)
        {
            AddRWOperation(() =>
            {
                Action onComplete = () =>
                {
                    if (onSaveComplete != null)
                        onSaveComplete();

                    CompleteRWOperation();
                };

                var data = GetLocalData();
                if (data != null)
                    Saves.ThreadSave(CompletePath, data, onComplete);
                else
                    onComplete();
            });
        }

        public void Save() { Save(null); }
        public void Save(Action onSaveComplete)
        {
            AddRWOperation(() =>
            {
                var data = GetLocalData();
                if (data != null)
                    Saves.InstantSave(CompletePath, data);

                if (onSaveComplete != null)
                    onSaveComplete();

                CompleteRWOperation();
            });
        }

        public void ClearSave() { ClearSave(null); }
        public void ClearSave(Action onComplete)
        {
            AddRWOperation(() =>
            {
                Saves.Delete(CompletePath);
                _SetDefaultLocalData();

                if (onComplete != null)
                    onComplete();

                CompleteRWOperation();
            });
        }
        #endregion

        #region RW Operations
        private void AddRWOperation(Action action)
        {
            //On s'enfile
            rwoQueue.Enqueue(action);

            //Sommes nous les premier a etre dans la queue ?
            if (rwoQueue.Count == 1)
                action();
        }

        private void CompleteRWOperation()
        {
            //On enleve la derniere action
            rwoQueue.Dequeue();

            //On execute la prochaine s'il y en a une
            if (rwoQueue.Count > 0)
                rwoQueue.Peek()();
        }
        #endregion
    }
}
