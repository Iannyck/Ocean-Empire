/*
using System;
using System.Reflection;
using UnityEngine;

public abstract class TestScript : MonoBehaviour, IShopDisplayable, IDescription
{
    public abstract bool Buy(CurrencyType type);

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public Sprite GetIcon()
    {
        throw new NotImplementedException();
    }

    public int GetPrice(CurrencyType type)
    {
        throw new NotImplementedException();
    }

    public string GetTitle()
    {
        throw new NotImplementedException();
    }
}

public interface IShopDisplayable : IBuyable
{
    Sprite GetIcon();
    string GetTitle();
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
*/