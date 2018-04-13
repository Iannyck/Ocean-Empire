using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialScene : MonoBehaviour
    {
        public const string SCENENAME = "Tutorial";

        public ProxyButton proxyButton;
        public Spotlight spotlight;
        public TextDisplay textDisplay;
        public InputDisabler inputDisabler;
        public Shortcuts shorcuts;
        public DelayedAction delayedAction;
        public WaitForInput waitForInput;

        /// <summary>
        /// Retourne faux si le tutoriel a deja ete completer
        /// </summary>
        public static bool StartTutorial(string tutorialAssetName, DataSaver tutorialSaver)
        {
            //Check if completed ?
            bool hasBeenCompleted = BaseTutorial.HasBeenCompleted(tutorialAssetName, tutorialSaver); // BUG CECI RETOURNE TOUJOURS FAUX
            Debug.Log("DO WE START TUTORIAL ? " + !hasBeenCompleted);
            if (hasBeenCompleted)
                return false;

            // Load la scene + lance le tuto
            ResourceRequest resourceRequest = Resources.LoadAsync<BaseTutorial>("Tutorial/" + tutorialAssetName);
            resourceRequest.completed += delegate (AsyncOperation obj)
            {
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
                {
                    Scenes.FindRootObject<TutorialScene>(scene).Init((BaseTutorial)resourceRequest.asset);
                });
            };
            return true;
         }

        /// <summary>
        /// Force le tutoriel a être exécuté
        /// </summary>
        public static void ForceStartTutorial(string tutorialAssetName)
        {
            // Load la scene + lance le tuto
            ResourceRequest resourceRequest = Resources.LoadAsync<BaseTutorial>("Tutorial" + tutorialAssetName);
            resourceRequest.completed += delegate (AsyncOperation obj)
            {
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
                {
                    Scenes.FindRootObject<TutorialScene>(scene).Init((BaseTutorial)resourceRequest.asset);
                });
            };
        }

        public bool Init(BaseTutorial tutorial)
        {
            if (tutorial == null)
                return false;

            shorcuts = new Shortcuts(this);

            tutorial.Init(this);

            return true;
        }
    }

}