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

    }
	
    void OpenTab(int ite)
    {
        if (ite == openedTab)
            return;
        openedTab = ite;

        for (int i = 0; i < items.Count; ++i)
        {
            Destroy(items[i].gameObject);
        }

        items.Clear();

        for (int i = 0; i < tabsList[ite].coutainedItems.Count; ++i)
        {
            ItemDisplay newItem = Instantiate(shopItem, shopItemLayout.transform);
            newItem.item = tabsList[ite].coutainedItems[i];
            items.Insert(i, newItem);
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            items[i].UpdateButton();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
