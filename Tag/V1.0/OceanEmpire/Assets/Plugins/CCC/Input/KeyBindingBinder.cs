using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FullInspector;
using System;
using DG.Tweening;

namespace CCC.Input
{
    public class KeyBindingBinder : BaseBehavior
    {
        [InspectorHeader("Normal")]
        public Image box;
        public Text normalText;
        public Vector2 normalSize;
        [InspectorHeader("Conflict")]
        public Text conflictText;
        public Vector2 conflictSize;
        public CanvasGroup conflictGroup;
        public Button continueButton;

        private KeyCombination currentCombination;
        private InputMapping currentMapping;
        private UnityAction<KeyCombination, KeyCombination> onComplete = null;

        //int totalKeyCount = Enum.GetNames(typeof(KeyCode)).Length;

        KeyCode firstKey = KeyCode.None;
        KeyCode secondKey = KeyCode.None;

        public void Rebind(KeyCombination combination, InputMapping mapping, UnityAction<KeyCombination, KeyCombination> onComplete = null)
        {
            this.onComplete = onComplete;
            currentMapping = mapping;
            currentCombination = combination;

            conflictGroup.alpha = 0;
            conflictGroup.blocksRaycasts = false;

            gameObject.SetActive(true);
            box.rectTransform.sizeDelta = normalSize;

            normalText.text = "Waiting for input...";

            StartCoroutine(InputGetter());
        }

        void SetFirstKey(KeyCode key)
        {
            firstKey = key;
            normalText.text = key.ToString();
        }

        void SetSecondKey(KeyCode key)
        {
            secondKey = key;
            if (key != KeyCode.None)
                normalText.text += " + " + key;
        }

        bool CheckForConflict(KeyCode a, KeyCode b)
        {
            return a == b;
        }
        bool CheckForConflict(KeyCombination target, KeyCombination current)
        {
            if (target != current)
            {
                if ((CheckForConflict(target.first, current.first) && CheckForConflict(target.second, current.second)) ||
                    (CheckForConflict(target.second, current.first) && CheckForConflict(target.first, current.second)))
                {
                    return true;
                }
            }
            return false;
        }

        void GetterEnd()
        {
            float animDuration = 0.25f;

            currentCombination.first = KeyCode.None;
            currentCombination.second = KeyCode.None;
            KeyCombination conflictingCombination = null;
            string conflictingName = "";
            KeyCombination newCombination = new KeyCombination(firstKey, secondKey);


            foreach (KeyValuePair<string, InputKey> key in currentMapping.keys)
            {
                KeyCombination other = key.Value.primary;
                if (CheckForConflict(other, newCombination))
                {
                    conflictingCombination = other;
                    conflictingName = key.Key;
                    break;
                }
                other = key.Value.secondary;
                if (CheckForConflict(other, newCombination))
                {
                    conflictingCombination = other;
                    conflictingName = key.Key;
                    break;
                }
            }

            //Conflict
            if (conflictingCombination != null)
            {
                box.rectTransform.DOSizeDelta(conflictSize, animDuration).SetEase(Ease.OutSine).OnComplete(delegate ()
                {
                    conflictText.text = "There is a conflict with '" + conflictingName + "'.";

                    conflictGroup.DOFade(1, animDuration);
                    conflictGroup.blocksRaycasts = true;

                    continueButton.onClick.AddListener(delegate ()
                    {
                        continueButton.onClick.RemoveAllListeners();

                        //Applique la nouvelle combinaison
                        currentCombination.Copy(newCombination);

                        //Remove celle qui fesait conflit
                        conflictingCombination.first = KeyCode.None;
                        conflictingCombination.second = KeyCode.None;

                        if (onComplete != null)
                        {
                            onComplete.Invoke(currentCombination, conflictingCombination);
                            onComplete = null;
                        }
                        Close();
                    });
                });
            }
            else
            {
                currentCombination.Copy(newCombination);
                if (onComplete != null)
                {
                    onComplete.Invoke(currentCombination, null);
                    onComplete = null;
                }
                Close();
            }
        }

        void Close()
        {
            continueButton.onClick.RemoveAllListeners();
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            if(onComplete != null)
            {
                onComplete.Invoke(currentCombination, null);
                onComplete = null;
            }
            Close();
        }

        IEnumerator InputGetter()
        {
            yield return null; //Necessaire pour éviter les potentiels boucle inifinie
            this.firstKey = KeyCode.None;
            this.secondKey = KeyCode.None;

            //Wait for key press
            while (!UnityEngine.Input.anyKeyDown)
            {
                yield return null;
            }

            //First key press !
            KeyCode firstKey = GetTheKeyDown();

            //Cancel ?
            if (firstKey == KeyCode.Escape)
            {
                Cancel();
                yield return null;
            }

            //Set first key
            SetFirstKey(firstKey);

            //Wait for release OR second key
            while ((!UnityEngine.Input.anyKeyDown && !UnityEngine.Input.GetKeyUp(firstKey)) || UnityEngine.Input.GetKeyDown(firstKey))
            {
                yield return null;
            }

            //Second key press
            KeyCode secondKey = GetTheKeyDown();

            //Cancel ?
            if (secondKey == KeyCode.Escape)
            {
                Cancel();
                yield return null;
            }

            //Set second key
            if (secondKey != firstKey)
                SetSecondKey(secondKey);


            //End
            GetterEnd();
        }

        KeyCode GetTheKeyDown(KeyCode exclude = KeyCode.None)
        {
            for (int i = 0; i < 350; i++)
            {
                if ((KeyCode)i == exclude)
                    continue;
                if (UnityEngine.Input.GetKeyDown((KeyCode)i))
                    return (KeyCode)i;
            }
            return KeyCode.None;
        }
    }

}