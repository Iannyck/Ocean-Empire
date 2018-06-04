using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionDisplay : MonoBehaviour
{
    [Header("Data")]
    //public MapDescription selectedMap;

    [Header("UI Links")]
    public Text titleName;
    public Image mapImage;
    public Button goButton;
    public Image lockImage;

    [Header("Settings")]
    public Color lockedImageColor;

    public void Init(bool isAvailable)
    {
        //titleName.text = selectedMap.GetName();
        //mapImage.sprite = selectedMap.mapIcon;
        mapImage.color = isAvailable ? Color.white : lockedImageColor;
        goButton.interactable = isAvailable;
        lockImage.enabled = !isAvailable;
    }

    private void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
    }

    public void Go()
    {
        if (FishingFrenzy.Instance != null)
        {
            if (FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available)
                FishingFrenzy.Instance.Activate();
        }

        //if (selectedMap != null)
        //{
        //    GameSettings gameSettings = new GameSettings
        //        (
        //        mapScene: selectedMap.sceneToLoad,
        //        canUseFishingFrenzy: true
        //        );
        //    LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(gameSettings), true);
        //}
    }
}
