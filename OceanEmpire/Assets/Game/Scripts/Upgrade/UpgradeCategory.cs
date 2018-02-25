using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category")]
public abstract class UpgradeCategory<B, D> : ScriptableObject, IUpgradeDisplayable
    where B : UpgradeDescriptionBuilder<D>
    where D : UpgradeDescription
{
    public bool SavingMode = true;

    [SerializeField] private List<B> upgradeBuilders;

    [SerializeField, ReadOnly] protected int ownedUpgrade = 0;
    [SerializeField, ReadOnly] protected string nextUpgGenCode = "";
    [SerializeField, ReadOnly] protected string ownedUpgGenUpgrade = "";

    protected abstract string OwnedUpgradeKey { get; }
    protected abstract string NextUpgGenCodeKey { get; }
    protected abstract string OwnedUpgGenKey { get; }

    public DataSaver dataSaver;

    private UpgradeDescription GetPrebuilt(int level)
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

    public UpgradeDescription GetCurrentDescription()
    {

        FetchDataFrom();
        UpgradeDescription prebuilt = GetPrebuilt(ownedUpgrade);
        if (prebuilt != null)
            return prebuilt;
        else
        {
            if (nextUpgGenCode == "")
                MakeNextGenCode(ownedUpgrade + 1);


            return GenerateNextDescription(ownedUpgGenUpgrade );
        }
    }

    public UpgradeDescription GetNextDescription()
    {

        FetchDataFrom();
        UpgradeDescription prebuilt = GetPrebuilt(ownedUpgrade + 1);
        if (prebuilt != null)
            return prebuilt;
        else
        {
            if (ownedUpgGenUpgrade == "") 
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

        ApplyDataTo();
        return true;
    }

    public int GetPrice(CurrencyType type)
    {
        return GetNextDescription().GetCost(type);
    }


    public void OnEnable()
    {
            ownedUpgrade = 0;
            ownedUpgGenUpgrade = "";
            nextUpgGenCode = "";
    }

    protected void FetchDataFrom()
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

    protected void ApplyDataTo()
    {
        if (SavingMode)
        { 
            dataSaver.SetInt(OwnedUpgradeKey, ownedUpgrade);
            dataSaver.SetString(OwnedUpgGenKey, ownedUpgGenUpgrade);
            dataSaver.SetString(NextUpgGenCodeKey, nextUpgGenCode);
        }
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

    public List<Statistic> GetStatistics()
    {
        return GetNextDescription().GetStatistics();
    }


    public void ResetData()
    {
        ownedUpgrade = 0;
        ownedUpgGenUpgrade = "";
        nextUpgGenCode = "";
        ApplyDataTo();
    }
}
