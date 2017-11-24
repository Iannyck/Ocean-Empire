<<<<<<< .merge_file_a15192
using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AskWindowWithGage : MonoBehaviour
{

    public const string SCENE_NAME = "AskWindowWithGage";

    public Text description;
    public Text option1;
    public Button option1Button;
    public Text option2;
    public WindowAnimation windowAnim;
    public WidgetFishPop widgetGage;
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
=======
using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AskWindowWithGage : MonoBehaviour
{

    public const string SCENE_NAME = "AskWindowWithGage";

    public Text description;
    public Text option1;
    public Button option1Button;
    public Text option2;
    public WindowAnimation windowAnim;
    public WidgetFishPop widgetGage;
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
>>>>>>> .merge_file_a16144
        windowAnim.Close(delegate()
        {
            Scenes.UnloadAsync(SCENE_NAME);
            Time.timeScale = 1;
<<<<<<< .merge_file_a15192
        });
    }

    public void DoExercice()
    {
        // Eventuellement a changer si on veut
        // On le mettre ailleur et passer une action dans le init
        Scenes.LoadAsync(InstantExerciseChoice.SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            Debug.LogWarning("A changer");
            //scene.FindRootObject<InstantExerciseChoice>().Init(false, ()=> {
            //    widgetGage.IncrementRate(0.5f);
            //    widgetGage.AnimComplete += Exit;
            //});
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
=======
        });
    }

    public void DoExercice()
    {
        // Eventuellement a changer si on veut
        // On le mettre ailleur et passer une action dans le init
        Scenes.LoadAsync(InstantExerciseChoice.SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            Debug.LogWarning("A changer");
            //scene.FindRootObject<InstantExerciseChoice>().Init(false, ()=> {
            //    widgetGage.IncrementRate(0.5f);
            //    widgetGage.AnimComplete += Exit;
            //});
        });
    }

    public void OnWaitWindowLoad(Scene scene)
    {
        scene.FindRootObject<TrackingWindow>().InitDisplay("A faire : Marcher 1 km",null,null, Exit);
        // ajouter un listener en parametre. Le WaitingWindow doit aller ecouter a se qui doit etre fait
        // et se ferme quand le listener est executer
    }

    public void UseTicket()
    {
        // Eventuellement a changer si on veut
        // On le mettre ailleur et passer une action dans le init
        if (PlayerCurrency.RemoveTickets(1))
            Exit(null);
    }

    public void Exit(ExerciseTrackingReport tracker)
    {
        Hide();
    }
}
>>>>>>> .merge_file_a16144
