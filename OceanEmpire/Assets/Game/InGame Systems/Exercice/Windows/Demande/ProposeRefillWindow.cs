using CCC.Manager;
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

    public WindowAnimation windowAnim;

    public bool debug = false;

    void Start()
    {
        Time.timeScale = 0;
        if (debug)
            MasterManager.Sync();

        if (PlayerCurrency.GetTickets() < 1)
            ticketButton.interactable = false;

        windowAnim.Open(delegate()
        {
            DelayManager.LocalCallTo(UpdateFishPopDisplay, delayToUpdatePop, this);
        });
    }

    private void UpdateFishPopDisplay()
    {
        widgetFishPop.DecrementRate(FishPopulation.PopulationRate);
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
        Debug.LogError("Mon ti chnappant, cossé tu fa icite ? FARME LA CRISS DE PORTE PI RVIENS PU !");

        // Exercice Instantanné
    }

    private void OnTrackerWindowLoad(Scene scene)
    {
        // ...
    }

    public void UseTicket()
    {
        if (PlayerCurrency.RemoveTickets(1))
            Hide();
    }
}
