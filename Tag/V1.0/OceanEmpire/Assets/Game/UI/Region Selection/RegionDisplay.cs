using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionDisplay : MonoBehaviour
{
    [Header("Data")]
    public MapDescription selectedMap;

    [Header("UI Links")]
    public Text titleName;
    public Image mapImage;
    public Button goButton;
    public Image lockImage;

    [Header("Settings")]
    public Color lockedImageColor;

    public void Init(bool isAvailable)
    {
        titleName.text = selectedMap.GetName();
        mapImage.sprite = selectedMap.mapIcon;
        mapImage.color = isAvailable ? Color.white : lockedImageColor;
        goButton.interactable = isAvailable;
        lockImage.enabled = !isAvailable;
    }

    private void Start()
    {
        CCC.Manager.MasterManager.Sync();
    }

    public void Go()
    {
        if (selectedMap != null)
            LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(selectedMap), true);
    }
}
