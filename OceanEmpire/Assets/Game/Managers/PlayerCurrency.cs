using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCurrency : BaseManager<PlayerCurrency>
{
    private const string SAVE_KEY_MONEY = "money";

    public override void Init()
    {
        Money = GameSaves.instance.GetInt(GameSaves.Type.Currency, SAVE_KEY_MONEY);
        CompleteInit();
    }

    private int money;

    public static int Money
    {
        private set
        {
            instance.money = value;
            Save();
        }

        get
        {
            return instance.money;
        }
    }

    public static int Add
    {
        set
        {
            instance.add(value);
        }
    }

    public static int Substract
    {
        set
        {
            instance.remove(value);
        }
    }

    public void remove(int amount)
    {
        if ((Money - amount) >= 0)
        {
            Money = 0;
            return;
        }
        Money -= amount;
    }

    public void add(int amount)
    {
        if (amount <= 0)
            return;
        Money += amount;
    }

    private static void Save()
    {
        GameSaves.instance.SetInt(GameSaves.Type.Currency, SAVE_KEY_MONEY, Money);
        GameSaves.instance.SaveData(GameSaves.Type.Currency);
    }
}
