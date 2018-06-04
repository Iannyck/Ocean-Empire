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
        public OkButton okButton;

        [SerializeField] Camera cam;

        public static bool IsInTutorial { get; private set; }

        void Awake()
        {
            if (cam)
                cam.gameObject.SetActive(false);
        }

        /// <summary>
        /// Retourne faux si le tutoriel a deja ete completer
        /// </summary>
        public static bool StartTutorial(string tutorialAssetName, DataSaver tutorialSaver)
        {
            //Check if completed ?
            bool hasBeenCompleted = BaseTutorial.HasBeenCompleted(tutorialAssetName, tutorialSaver);
            if (hasBeenCompleted)
                return false;

            if (IsInTutorial)
            {
                Debug.LogWarning("Cannot start \"" + tutorialAssetName + "\". Already in a tutorial");
                return false;
            }

            IsInTutorial = true;


            Debug.Log("Starting tutorial: " + tutorialAssetName);

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

        public void QuitTuto()
        {
            IsInTutorial = false;
            Scenes.UnloadAsync(SCENENAME);
        }
    }

}