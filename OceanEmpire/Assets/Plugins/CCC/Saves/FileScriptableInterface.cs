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

        public string CompletePath
        {
            get { return Application.persistentDataPath + "/" + fileName + FILE_EXTENSION; }
        }


        private void _OverwriteLocalData(object graph)
        {
            if (OnReassignData != null)
                OnReassignData();
            OverwriteLocalData(graph);
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
