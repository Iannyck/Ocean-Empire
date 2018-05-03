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
        //int index = level - 1;
        //if (upgradeBuilders.Count <= index)
        //    return null;
        //else
        //    return upgradeBuilders[index] != null ? upgradeBuilders[index].BuildUpgradeDescription() : null;

        foreach (var item in upgradeBuilders)
        {
            if (item != null && level == item.GetUpgradeLevel())
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

    protected override int DefaultUnlockedLevel
    {
        get
        {
            if (upgradeBuilders.Count == 0 || upgradeBuilders[0] == null)
                return 0;
            else
                return upgradeBuilders[0].GetUpgradeLevel();
        }
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
    protected abstract string AvailableSaveKey { get; }
    protected abstract bool AvailableByDefault { get; }


    public DataSaver dataSaver;

    public bool IsAvailable
    {
        get
        {
            return AvailableByDefault || dataSaver.GetBool(AvailableSaveKey);
        }
    }

    public void MakeAvailable(bool andSave = true)
    {
        dataSaver.SetBool(AvailableSaveKey, true);
        if (andSave)
            dataSaver.LateSave();
    }

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
        ownedUpgrade = dataSaver.GetInt(OwnedUpgradeKey, DefaultUnlockedLevel);
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

    protected abstract int DefaultUnlockedLevel { get; }
}