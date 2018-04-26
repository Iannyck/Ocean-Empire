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
            if (level == item.upgradeLevel)
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
        if (prebuilt != null)
            return prebuilt;
        else
        {
            if (nextUpgGenCode == "")
                MakeNextGenCode(ownedUpgrade + 1);


            return GenerateNextDescription(ownedUpgGenUpgrade);
        }
    }

    public D GetNextDescription()
    {
        FetchData();
        D prebuilt = GetPrebuilt(ownedUpgrade + 1);
        if (prebuilt != null)
            return prebuilt;
        else
        {
            if (ownedUpgGenUpgrade == "")
                MakeNextGenCode(ownedUpgrade + 1);


            return GenerateNextDescription(nextUpgGenCode);
        }
    }

    public abstract D GenerateNextDescription(string nextUpgGenCode);
    public abstract void MakeNextGenCode(int level);

    public bool Buy(CurrencyType type)
    {
        if (PlayerCurrency.RemoveCurrentAmount(new CurrencyAmount(GetNextDescription().GetCost(type), type)) == false)
            return false;

        ownedUpgrade++;

        ApplyData();
        return true;
    }

    //public int GetPrice(CurrencyType type)
    //{
    //    return GetNextDescription().GetCost(type);
    //}

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

public abstract class UpgradeCategory : ScriptableObject
{
    public bool SavingMode = true;

    [SerializeField, ReadOnly] protected int ownedUpgrade = 0;
    [SerializeField, ReadOnly] protected string nextUpgGenCode = "";
    [SerializeField, ReadOnly] protected string ownedUpgGenUpgrade = "";

    protected abstract string OwnedUpgradeKey { get; }
    protected abstract string NextUpgGenCodeKey { get; }
    protected abstract string OwnedUpgGenKey { get; }

    public DataSaver dataSaver;


    public void OnEnable()
    {
        ownedUpgrade = 0;
        ownedUpgGenUpgrade = "";
        nextUpgGenCode = "";
    }

    #region Load/Save Data
    protected void FetchData()
    {
        if (SavingMode)
        {
            if (ownedUpgrade == 0)
                ownedUpgrade = dataSaver.GetInt(OwnedUpgradeKey, 0);

            if (OwnedUpgGenKey == "")
                ownedUpgGenUpgrade = dataSaver.GetString(OwnedUpgGenKey, "");

            if (nextUpgGenCode == "")
                nextUpgGenCode = dataSaver.GetString(NextUpgGenCodeKey, "");
        }
    }
    protected void ApplyData()
    {
        if (SavingMode)
        {
            dataSaver.SetInt(OwnedUpgradeKey, ownedUpgrade);
            dataSaver.SetString(OwnedUpgGenKey, ownedUpgGenUpgrade);
            dataSaver.SetString(NextUpgGenCodeKey, nextUpgGenCode);
        }
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