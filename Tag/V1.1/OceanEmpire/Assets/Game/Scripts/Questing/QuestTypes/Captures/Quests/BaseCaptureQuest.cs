using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public abstract class BaseCaptureQuest<T> : Quest<T> where T : CaptureQC
    {
        [SerializeField] protected int capturedYet;

        [NonSerialized] protected bool isListening;

        public override void Launch()
        {
            capturedYet = 0;
        }

        public override QuestState UpdateState()
        {
            // Validate Listeners
            UpdateListeners();

            // Update state according to goal
            if (capturedYet >= context.captureGoal)
            {
                state = QuestState.Completed;
            }
            else
            {
                state = QuestState.Ongoing;
            }
            return state;
        }

        void UpdateListeners()
        {
            if (Game.Instance != null && state == QuestState.Ongoing)
            {
                Listen();
            }
            else
            {
                DontListen();
            }
        }

        protected virtual void OnFishCaptured(Capturable capturable, CaptureTechnique captureTechnique)
        {
            if (state != QuestState.Ongoing || (context.onlyInFishingFrenzy && !Game.Instance.IsInFishingFrenzy))
                return;

            if (IsValidFishCapture(capturable) && (captureTechnique & context.allowedTechniques) != 0)
            {
                capturedYet++;
                UpdateState();
                UpdateListeners();

                // Have we just completed the objective ?
                if (state == QuestState.Completed)
                {
                    Complete();
                }
                else
                    DirtyState = DirtyState.Dirty;
            }
        }

        protected abstract bool IsValidFishCapture(Capturable capturable);

        void Listen()
        {
            if (isListening)
                return;
            isListening = true;
            Game.Instance.PlayerStats.OnCapture += OnFishCaptured;
        }


        void DontListen()
        {
            if (!isListening)
                return;
            isListening = false;
            if (Game.Instance != null)
                Game.Instance.PlayerStats.OnCapture -= OnFishCaptured;
        }

        public override string GetDisplayedProgressText()
        {
            return capturedYet + " / " + context.captureGoal;
        }

        public override float GetProgress01()
        {
            return capturedYet / (float)context.captureGoal;
        }
    }
}
