using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category")]
public abstract class UpgradeCategory : ScriptableObject, IShopDisplayable
{
    [SerializeField, ReadOnly] private int ownedUpgrade = -1;
    [SerializeField, ReadOnly] private string nextUpgGenCode = "";

    private string ownedUpgradeKey;
    private string nextUpgGenCodeKey;

    public DataSaver dataSaver;
    public Dictionary<int, UpgradeDescription> prebuiltUpgrades;


    public UpgradeDescription GetNextDescription()
    {
        if (prebuiltUpgrades.ContainsKey(ownedUpgrade + 1))
            return prebuiltUpgrades[ownedUpgrade + 1];
        else return GenerateNextDescription(nextUpgGenCode);
    }

    public abstract UpgradeDescription GenerateNextDescription(string nextUpgGenCode);

    public bool Buy(CurrencyType type)
    {
        if (PlayerCurrency.RemoveCurrentAmount(new CurrencyAmount(GetPrice(type), type)) == false)
            return false;

        ownedUpgrade++;
        return true;
    }

    public int GetPrice(CurrencyType type)
    {
        return GetNextDescription().GetCost(type);
    }

    private void Load()
    {
        if (ownedUpgrade == -1)
            ownedUpgrade = dataSaver.GetInt(ownedUpgradeKey, -1);
        if (nextUpgGenCode == "")
            nextUpgGenCode = dataSaver.GetString(nextUpgGenCode, "");
    }

    private void Save()
    {
        dataSaver.SetInt(ownedUpgradeKey, ownedUpgrade);
        dataSaver.SetString(nextUpgGenCodeKey, nextUpgGenCode);
    }

    public Sprite GetIcon()
    {
        return GetNextDescription().GetIcon();
    }
    public string GetTitle()
    {
        return GetNextDescription().GetTitle();
    }
    public string GetDescription()
    {
        return GetNextDescription().GetDescription();
    }
}
