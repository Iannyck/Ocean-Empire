using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category")]
public abstract class UpgradeCategory<B, D> : ScriptableObject, IShopDisplayable
    where B : UpgradeDescriptionBuilder<D>
    where D : UpgradeDescription
{

    [SerializeField] private List<B> upgradeBuilders;

    [SerializeField, ReadOnly] protected int ownedUpgrade = -1;
    [SerializeField, ReadOnly] protected string nextUpgGenCode = "";

    private string ownedUpgradeKey;
    private string nextUpgGenCodeKey;

    public DataSaver dataSaver;

    private UpgradeDescription GetPrebuilt(int level)
    {
        foreach (var item in upgradeBuilders)
        {
            if (level == item.upgradeLevel)
                return item.BuildUpgradeDescription();
        }
        return null;
    }

    public UpgradeDescription GetNextDescription()
    {
        UpgradeDescription prebuilt = GetPrebuilt(ownedUpgrade + 1);
        if (prebuilt != null)
            return prebuilt;
        else
        {
            if (nextUpgGenCode == "")
                MakeNextGenCode(ownedUpgrade + 1);

            return GenerateNextDescription(nextUpgGenCode);
        }
    }

    public abstract UpgradeDescription GenerateNextDescription(string nextUpgGenCode);
    public abstract void MakeNextGenCode(int level);

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

    protected void Load()
    {
        if (ownedUpgrade == -1)
            ownedUpgrade = dataSaver.GetInt(ownedUpgradeKey, -1);
        if (nextUpgGenCode == "")
            nextUpgGenCode = dataSaver.GetString(nextUpgGenCode, "");
    }

    protected void Save()
    {
        dataSaver.SetInt(ownedUpgradeKey, ownedUpgrade);
        dataSaver.SetString(nextUpgGenCodeKey, nextUpgGenCode);
    }

    public Sprite GetShopIcon()
    {
        return GetNextDescription().GetShopIcon();
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
