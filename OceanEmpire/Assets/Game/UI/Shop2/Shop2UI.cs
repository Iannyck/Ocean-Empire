using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop2UI : MonoBehaviour {

    public CategoryDisplay prefabCategoryDisplay;
    public GameObject shopCategoryLayout;

    public List<ScriptableObject> categories;
    private List<CategoryDisplay> categoryList;

    private void OnValidate()
    {
        for (int i = 0; i < categories.Count; i++)
        {
            if (!(categories[i] is IShopDisplayable))
            { 
                categories.RemoveAt(i);
                i--;
                Debug.LogError("Les élements de categories doivent être IShopDisplayable!!!");
            }
        }
    }

    // Use this for initialization
    void Start () {
        Init();
    }

    private void Init()
    {
        categoryList = new List<CategoryDisplay>();
        UpdateDisplay();

        PlayerCurrency.CurrencyUpdate += UpdateDisplay;
    }


    void UpdateDisplay()
    {

        for (int i = 0; i < categoryList.Count; ++i)
            Destroy(categoryList[i].gameObject);

        categoryList.Clear();
        int categoryCount = categories.Count;

        for (int i = 0; i < categoryCount; ++i)
            DisplayCategories(categories[i] as IShopDisplayable);
    }

    void DisplayCategories(IShopDisplayable itemDescription)
    {
        CategoryDisplay newCategory = Instantiate(prefabCategoryDisplay, shopCategoryLayout.transform);
        newCategory.SetValues(itemDescription);
            categoryList.Add(newCategory);        
    }

}
