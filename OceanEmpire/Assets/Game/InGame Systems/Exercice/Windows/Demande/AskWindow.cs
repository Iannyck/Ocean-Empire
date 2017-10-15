using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AskWindow : MonoBehaviour {

    public const string SCENE_NAME = "AskWindow";

    public Text description;
    public Text option1;
    public Button option1Button;
    public Text option2;
    public WindowAnimation windowAnim;
    public bool debug = false;

    void Start()
    {
        Time.timeScale = 0;
        if (debug)
        {
            MasterManager.Sync();
            InitDisplay("Votre densité de poisson est faible. Vous pouvez la remettre à 0 via ces deux options: ");
        }
    }

    // Envtuellement a rendre plus generique
    public void InitDisplay(string description, string option1 = "Utiliser un Ticket", string option2 = "Faire un exercice")
    {
        this.description.text = description;
        this.option1.text = option1;
        this.option2.text = option2;
        if (PlayerCurrency.GetTickets() < 1)
            option1Button.interactable = false;
        windowAnim.Open();
    }

    public void Hide()
    {
        windowAnim.Close();
        Scenes.UnloadAsync(SCENE_NAME);
        Time.timeScale = 1;
    }

    public void DoExercice()
    {
        // Eventuellement a changer si on veut
        // On le mettre ailleur et passer une action dans le init
        Scenes.LoadAsync(WaitingWindow.SCENE_NAME, LoadSceneMode.Additive, delegate(Scene scene) {
            scene.FindRootObject<WaitingWindow>().InitDisplay("Faites une marche de au moins 5 minutes dans votre quartier. Après cette durée"+
                " l'effet dans l'océan sera instantément appliqué",delegate() {
                    FishPopulation.instance.UpdateOnExercise(0.50f);    
                    Exit();
                });
        });
    }

    public void OnWaitWindowLoad(Scene scene)
    {
        scene.FindRootObject<WaitingWindow>().InitDisplay("A faire : Marcher 1 km", Exit);
        // ajouter un listener en parametre. Le WaitingWindow doit aller ecouter a se qui doit etre fait
        // et se ferme quand le listener est executer
    }

    public void UseTicket()
    {
        // Eventuellement a changer si on veut
        // On le mettre ailleur et passer une action dans le init
        if (PlayerCurrency.RemoveTickets(1))
            Exit();
    }

    public void Exit()
    {
        Hide();
    }
}
