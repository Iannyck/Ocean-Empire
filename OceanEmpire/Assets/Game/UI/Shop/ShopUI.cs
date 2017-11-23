using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FullInspector;
using CCC;

public class ShopUI : BaseBehavior {

    [System.Serializable]
    public  struct shopTab
    {
        public string tabName;
        public List<ItemDescription> coutainedItems;
    }
    public Button tabButton;
    public ItemDisplay shopItem;

    public GameObject tabButtonLayout;
    public GameObject shopItemLayout;

    public List<shopTab> tabsList;

    private List<Button> tabs;
    private List<ItemDisplay> items;
    private int openedTab = -1;

    // Use this for initialization
    void Start () {


        tabs = new List<Button>();
        for (int i = 0; i < tabsList.Count; ++i)
        {
            Button newTab = Instantiate(tabButton.gameObject, tabButtonLayout.transform).GetComponent<Button>();

            newTab.GetComponentInChildren<Text>().text = tabsList[i].tabName;
            int number = i;
            newTab.onClick.AddListener( () => { OpenTab(number); } );
            tabs.Insert(i, newTab.GetComponent<Button>());
        }

        items = new List<ItemDisplay>();
        if (tabsList.Count > 0)
            OpenTab(0);


        PlayerCurrency.CurrencyUpdate += UpdateDisplay;
    }

    private void OnDestroy()
    {
        PlayerCurrency.CurrencyUpdate -= UpdateDisplay;
    }

    void OpenTab(int ite)
    {
        /*
        if (ite == openedTab)
            return;*/
        openedTab = ite;

        for (int i = 0; i < items.Count; ++i)
            Destroy(items[i].gameObject);
      
        items.Clear();
        int itemCount = tabsList[ite].coutainedItems.Count;

        for (int i = 0; i < itemCount; ++i)
            DisplayItem(ite, tabsList[ite].coutainedItems[i]);       
    }

    void DisplayItem(int tab, ItemDescription itemDescription)
    {
        if (ItemIsRelevant(itemDescription))
        {
            if (itemDescription is UpgradeDescription )
            {        
                    ItemDisplay newItem = Instantiate(shopItem, shopItemLayout.transform);
                    newItem.item = itemDescription;
                    items.Add(newItem);
            }
            if (itemDescription is ShopMapDescription)
            {        
                    ItemDisplay newItem = Instantiate(shopItem, shopItemLayout.transform);
                    newItem.item = itemDescription;
                    items.Add(newItem);        
            }
        }
    }


    public bool ItemIsRelevant(ItemDescription item)
    {
        if (item is UpgradeDescription)
            return UpgradeIsRelevant(item as UpgradeDescription);
        else if (item is ShopMapDescription)
            return !ItemsList.ItemOwned(item.GetItemID());
        return false;
    }

    public bool UpgradeIsRelevant(UpgradeDescription item)
    {
        int CurrentLevel = -1;

        if (item is ThrusterDescription && ItemsList.GetEquipThruster())
            CurrentLevel = ItemsList.GetEquipThruster().GetUpgradeLevel();
        else if (item is HarpoonThrowerDescription && ItemsList.GetEquipHarpoonThrower())
            CurrentLevel = ItemsList.GetEquipHarpoonThrower().GetUpgradeLevel();

        if (item.GetUpgradeLevel() >= CurrentLevel)
            return true;
        else
            return false;
    }


    public void UpdateDisplay()
    {
        OpenTab(openedTab);
        /*
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].item is UpgradeDescription && !UpgradeIsRelevant(items[i].item as UpgradeDescription))
            {
                ItemDisplay temp = items[i];
                items.RemoveAt(i);
                Destroy(temp);
            }
            else
                items[i].UpdateButton();
        }*/
    }
}
