using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category")]
public abstract class UpgradeCategory<B, D> : UpgradeCategory
    where B : UpgradeDescriptionBuilder<D>
    where D : UpgradeDescription
{

    [SerializeField] private List<B> upgradeBuilders;

    private D GetPrebuilt(int level)
    {
        string t = OwnedUpgradeKey;
        foreach (var item in upgradeBuilders)
        {
            if (level == item.GetUpgradeLevel())
            {
                return item.BuildUpgradeDescription();
            }
        }
        return null;
    }

    public D GetCurrentDescription()
    {
        FetchData();
        D prebuilt = GetPrebuilt(ownedUpgrade);
        //if (prebuilt != null)
            return prebuilt;
        //else
        //{
        //    if (nextUpgGenCode == "")
        //        MakeNextGenCode(ownedUpgrade + 1);


        //    return GenerateNextDescription(ownedUpgGenUpgrade);
        //}
    }

    public D GetNextDescription()
    {
        FetchData();
        D prebuilt = GetPrebuilt(ownedUpgrade + 1);
        //if (prebuilt != null)
            return prebuilt;
        //else
        //{
        //    if (ownedUpgGenUpgrade == "")
        //        MakeNextGenCode(ownedUpgrade + 1);


        //    return GenerateNextDescription(nextUpgGenCode);
        //}
    }

    public abstract D GenerateNextDescription(string nextUpgGenCode);
    public abstract void MakeNextGenCode(int level);

    //public Sprite GetShopIcon()
    //{
    //    return GetNextDescription().GetShopIcon();
    //}
    //public string GetTitle()
    //{
    //    return GetNextDescription().GetTitle();
    //}
    //public string GetDescription()
    //{
    //    return GetNextDescription().GetDescription();
    //}

    //public List<Statistic> GetStatistics()
    //{
    //    return GetNextDescription().GetStatistics();
    //}

    public override UpgradeDescription GetCurrentUpgradeDescription()
    {
        return GetCurrentDescription();
    }

    public override UpgradeDescription GetNextUpgradeDescription()
    {
        return GetNextDescription();
    }
}

public abstract class UpgradeCategory : ScriptableObject, IBuyable
{
    [SerializeField, ReadOnly] protected int ownedUpgrade = 0;
    [SerializeField, ReadOnly] protected string nextUpgGenCode = "";
    [SerializeField, ReadOnly] protected string ownedUpgGenUpgrade = "";
    [SerializeField] public string CategoryName;

    protected abstract string OwnedUpgradeKey { get; }
    protected abstract string NextUpgGenCodeKey { get; }
    protected abstract string OwnedUpgGenKey { get; }

    public DataSaver dataSaver;

    public bool Buy(CurrencyType type)
    {
        if (PlayerCurrency.RemoveCurrentAmount(new CurrencyAmount(GetNextUpgradeDescription().GetCost(type), type)) == false)
            return false;

        ownedUpgrade++;

        ApplyData();
        dataSaver.LateSave();
        return true;
    }

    public int GetPrice(CurrencyType type)
    {
        return GetNextUpgradeDescription().GetCost(type);
    }

    public void OnEnable()
    {
        ownedUpgrade = 0;
        ownedUpgGenUpgrade = "";
        nextUpgGenCode = "";
    }

    #region Load/Save Data
    protected void FetchData()
    {
        ownedUpgrade = dataSaver.GetInt(OwnedUpgradeKey, 1);
        ownedUpgGenUpgrade = dataSaver.GetString(OwnedUpgGenKey, "");
        nextUpgGenCode = dataSaver.GetString(NextUpgGenCodeKey, "");
    }
    protected void ApplyData()
    {
        dataSaver.SetInt(OwnedUpgradeKey, ownedUpgrade);
        dataSaver.SetString(OwnedUpgGenKey, ownedUpgGenUpgrade);
        dataSaver.SetString(NextUpgGenCodeKey, nextUpgGenCode);
    }
    #endregion

    public void ResetData()
    {
        ownedUpgrade = 0;
        ownedUpgGenUpgrade = "";
        nextUpgGenCode = "";
        ApplyData();
    }

    public abstract UpgradeDescription GetCurrentUpgradeDescription();
    public abstract UpgradeDescription GetNextUpgradeDescription();
}