using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IShopDisplayable : IBuyable
{
    Sprite GetShopIcon();
    string GetTitle();
    string GetDescription();
}

public interface IBuyable
{
    bool Buy(CurrencyType type);
    int GetPrice(CurrencyType type);
}

public interface IDescription
{
    string GetDescription();
}

public interface IUpgradeDisplayable : IShopDisplayable
{
    List<Statistic> GetStatistics();
    int GetCurrentLevel();
    UpgradeDescription GetCurrentUpgradeDescription();
    UpgradeDescription GetNextUpgradeDescription();
}