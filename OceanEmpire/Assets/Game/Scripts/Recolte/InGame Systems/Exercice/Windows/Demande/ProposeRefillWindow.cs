
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProposeRefillWindow : MonoBehaviour
{
    public const string SCENE_NAME = "ProposeRefill";

    public float delayToUpdatePop = 1f;

    public WidgetFishPop widgetFishPop;
    public Button ticketButton;

    public Text textTicket;
    private const string baseTextTicket = "x ticket";
    private CurrencyAmount refillCost;

    public WindowAnimation windowAnim;


    public bool debug = false;

    private bool exerciseListeners = false;

    void Start()
    {
        Time.timeScale = 0;
        if (debug)
            PersistentLoader.LoadIfNotLoaded();

        SetTicketCost();

        windowAnim.Open(delegate ()
        {
            this.DelayedCall(UpdateFishPopDisplay, delayToUpdatePop);
        });
    }

    private void UpdateFishPopDisplay()
    {
        widgetFishPop.UpdateMeter();//DecrementRate(FishPopulation.PopulationRate);
    }

    public void SetTicketCost()
    {
        refillCost = Market.GetCurrencyAmountFromValue(CurrencyType.Ticket, Market.GetOceanRefillValue());
        textTicket.text = refillCost.amount.ToString() + baseTextTicket;
        if (PlayerCurrency.GetTickets() < refillCost.amount)
            ticketButton.interactable = false;
    }

    private void Hide()
    {
        windowAnim.Close(delegate ()
        {
            Scenes.UnloadAsync(SCENE_NAME);
            Time.timeScale = 1;
        });
    }

    public void DoExercice()
    {
        RemoveExerciseListeners();

        // Exercice Instantanné
        //InstantExerciseChoice.ProposeTasks(RewardType.OceanRefill);
        AddExerciseListeners();
    }

    void OnDestroy()
    {
        RemoveExerciseListeners();
    }

    private void AddExerciseListeners()
    {
        if (exerciseListeners)
            return;

        exerciseListeners = true;
        if (PendingReports.instance != null)
            PendingReports.instance.onReportConcluded += Hide;
    }
    private void RemoveExerciseListeners()
    {
        if (!exerciseListeners)
            return;

        exerciseListeners = false;
        if (PendingReports.instance != null)
            PendingReports.instance.onReportConcluded -= Hide;
    }

    private void OnTrackerWindowLoad(Scene scene)
    {
        // ...
    }

    public void UseTicket()
    {
        if (PlayerCurrency.RemoveTickets(refillCost.amount))
        {
            widgetFishPop.AutoUpdate = false;
            widgetFishPop.Fill(() => Hide());
        }
    }
}
