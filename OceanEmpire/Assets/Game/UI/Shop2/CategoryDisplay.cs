using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CategoryDisplay : MonoBehaviour {

    private IShopDisplayable displayableCategory;

    public Text titre;
    public Image icon;


    public void SetValues(IShopDisplayable category)
    {
        displayableCategory = category;
        UpdateView();
    }

    public void UpdateView()
    {
        titre.text = displayableCategory.GetTitle();
        icon.sprite = displayableCategory.GetShopIcon();

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => OpenItem());
    }

    public void OpenItem()
    {
        ItemDescScene.OpenItemDescription(displayableCategory, () => UpdateView() );

    }
}
