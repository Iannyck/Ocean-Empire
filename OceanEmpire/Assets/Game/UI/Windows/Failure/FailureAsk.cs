using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailureAsk : MonoBehaviour {

    private int maxChoiceAmount = 4;

    public ChoiceButton buttonPrefab;
    public Transform countainer;

    public WindowAnimation adviceWindow;

    [Serializable]
    public struct Choice
    {
        public string choiceText;
        public string advice;
    }
 
    public List<Choice> choixPossible = new List<Choice>();

	void Start ()
    {
        for (int i = 0; i < choixPossible.Count; i++)
        {
            ChoiceButton currentButton = Instantiate(buttonPrefab, countainer).GetComponent<ChoiceButton>();
            if (currentButton == null)
                Debug.Log("ChoiceButton Inexistant lors de sa creation");
            currentButton.UpdateButtonInfo(DisplayAdvice, choixPossible[i].choiceText, choixPossible[i].advice);
        }
	}

    void DisplayAdvice(string advice)
    {
        WindowAnimation askWindowAnim = GetComponent<WindowAnimation>();
        if(askWindowAnim == null)
            Debug.Log("Pas de window anim sur la fenetre ASK");
        askWindowAnim.Close(delegate() {
            adviceWindow.GetComponent<FailureAdvice>().UpdateInfo(advice);
            adviceWindow.Open();
        });
    }

    private void OnValidate()
    {
        if(choixPossible.Count >= maxChoiceAmount)
        {
            Debug.Log("Trop de choix dans : FailureAsk");
            choixPossible.RemoveRange(4, Mathf.Max(choixPossible.Count-4,0));
        }
    }
}
