
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public abstract class BaseTutorial : ScriptableObject
    {
        [Serializable]
        public class TutorialEvent
        {
            public string eventID;
            public string functionName;
            public bool useSpecificTime = false;
            public bool useRealTime = true;
            public float when = 0;
            public bool invokeOnGameStarted = true;
            public bool startAfterAnotherStep = false;
            public int startAfterTutorialStepIndex;
            public SimpleEvent onComplete;

            [HideInInspector, NonSerialized]
            public bool alreadyExecute = false;

            public void OnComplete()
            {
                if (onComplete != null)
                {
                    onComplete.Invoke();
                }
                Debug.Log(functionName + " Event Complete");
            }
        }
        public DataSaver dataSaver;
        public bool startTutorialOnInit = true;
        public bool forceStart = false;
        [SerializeField]
        public List<TutorialEvent> tutorialEvents = new List<TutorialEvent>();

        [NonSerialized]
        protected TutorialScene modules;

        private const string TUTORIALSAVE = "cmplt"; //Short pour 'Completed'

        public virtual void Init(TutorialScene modules)
        {
            this.modules = modules;

            if (startTutorialOnInit)
                Start();
        }

        /// <summary>
        /// Debut du tutoriel
        /// </summary>
        public void Start()
        {
            OnStart(delegate () {
                // Avant meme de commencer a faire les events, on doit s'assurer que l'enchainement se fera comme il faut
                for (int i = 0; i < tutorialEvents.Count; i++)
                {
                    TutorialEvent currentTutorialEvent = tutorialEvents[i];
                    if (currentTutorialEvent.startAfterAnotherStep)
                        tutorialEvents[currentTutorialEvent.startAfterTutorialStepIndex].onComplete += delegate () {
                            Execute(currentTutorialEvent, currentTutorialEvent.useRealTime);
                        };
                }

                // On peut ensuite commencer les events qui sont start au debut
                for (int i = 0; i < tutorialEvents.Count; i++)
                {
                    TutorialEvent currentEvent = tutorialEvents[i];
                    if (currentEvent.alreadyExecute)
                        continue;
                    if (currentEvent.invokeOnGameStarted)
                        Execute(currentEvent, currentEvent.useRealTime);
                }
            });
        }

        protected abstract void OnStart(Action onComplete = null);

        public void ManuallyExecuteEvent(string eventId)
        {
            for (int i = 0; i < tutorialEvents.Count; i++)
            {
                if (tutorialEvents[i].eventID == eventId)
                    Execute(tutorialEvents[i], tutorialEvents[i].useRealTime);
            }
        }

        /// <summary>
        /// Fin du tutoriel
        /// </summary>
        public void End(bool markAsCompleted)
        {
            if (markAsCompleted)
            {
                dataSaver.SetBool(TUTORIALSAVE + name, true);
                dataSaver.Save(delegate ()
                {
                    Debug.Log("SAVED! " + "(" + TUTORIALSAVE + name + ") " + dataSaver + " Result: " + HasBeenCompleted(name, dataSaver));
                    Quit();
                });
                return;
            }
            else
            {
                Quit();
            }
        }
        public void End()
        {
            End(true);
        }

        protected virtual void Cleanup()
        {
            
        }

        private void Quit()
        {
            Cleanup();
            modules.QuitTuto();
            modules = null;
        }

        public static bool HasBeenCompleted(string assetName, DataSaver tutorialSaver)
        {
            //Debug.Log("Checking if it has been completed..." + "(Saving in:" + tutorialSaver.name + ") at " + TUTORIALSAVE + assetName);
            if (!tutorialSaver.ContainsBool(TUTORIALSAVE + assetName))
            {
                tutorialSaver.SetBool(TUTORIALSAVE + assetName, false);
                tutorialSaver.SaveAsync();
                return false;
            } else 
                return tutorialSaver.GetBool(TUTORIALSAVE + assetName);
        }

        public void Execute(TutorialEvent tutorialEvent, bool useTime)
        {
            if (tutorialEvent.alreadyExecute)
                return;
            else
                tutorialEvent.alreadyExecute = true; // Never Execute It Again

            // Find the Action/Method to Invoke
            Type thisType = GetType();
            MethodInfo theMethod = thisType.GetMethod(tutorialEvent.functionName);

            // Gere le temps
            Sequence sequence = DOTween.Sequence().SetUpdate(true);

            // Parametre de la method = Fonction On Complete
            Action[] parameters;
            parameters = new Action[1];
            parameters[0] = delegate ()
            {
                tutorialEvent.OnComplete();
            };

            if (tutorialEvent.useSpecificTime)
                sequence.InsertCallback(tutorialEvent.when, delegate ()
                {
                    theMethod.Invoke(this, parameters);
                }).SetUpdate(useTime);
            else
            {
                theMethod.Invoke(this, parameters);
            }
        }

        public virtual bool LaunchCondition() { return true; }
    }
}