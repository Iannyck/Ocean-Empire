 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CCC.Persistence;

public class PlayerCurrency : MonoPersistent
{
    private const string SAVE_KEY_COINS = "coins";
    private const string SAVE_KEY_TICKETS = "tickets";


    public static event SimpleEvent CurrencyUpdate;

    [SerializeField]
    private Sprite moneyIcon;
    [SerializeField]
    private Sprite ticketIcon;

    public static Sprite GetMoneyIcon() { return instance.moneyIcon; }
    public static Sprite GetTicketIcon() { return instance.ticketIcon; }



    [SerializeField, ReadOnly]
    private int coins;
    [SerializeField, ReadOnly]
    private int tickets;
    [SerializeField]
    private DataSaver dataSaver;

    public static PlayerCurrency instance;

    public override void Init(Action onComplete)
    {
        instance = this;
        FetchData();
        onComplete();
    }

    protected void Awake()
    {
        dataSaver.OnReassignData += FetchData;
    }

    public static int GetCoins()
    {
        return instance.coins;
    }

    /// <summary>
    /// Ajoute des coins ET sauvegarde. Retourne FALSE si la transaction a échoué.
    /// </summary>
    public static bool AddCoins(int amount)
    {
        //On empeche le joueur d'aller dans l'argent négatif
        if (amount < 0 && amount.Abs() > GetCoins())
            return false;

        //On ne fait rien si le montant a ajouté == 0
        if (amount == 0)
            return true;


        instance.coins += amount;

        if (CurrencyUpdate != null)
        {
            CurrencyUpdate();
        }

        instance.SaveData();

        return true;
    }

    /// <summary>
    /// Ajoute des coins ET sauvegarde. Retourne FALSE si la transaction a échoué.
    /// </summary>
    public static bool RemoveCoins(int amount)
    {
        return AddCoins(-amount);
    }


    public static int GetTickets()
    {
        return instance.tickets;
    }

    /// <summary>
    /// Ajoute des tickets ET sauvegarde. Retourne FALSE si la transaction a échoué.
    /// </summary>
    public static bool AddTickets(int amount)
    {
        //On empeche le joueur d'aller dans les tickets négatif
        if (amount < 0 && amount.Abs() > GetTickets())
            return false;

        //On ne fait rien si le montant a ajouté == 0
        if (amount == 0)
            return true;
    
        instance.tickets += amount;

        if (CurrencyUpdate != null)
        {
            CurrencyUpdate();
        }

        instance.SaveData();

        return true;
    }

    /// <summary>
    /// Ajoute des tickets ET sauvegarde. Retourne FALSE si la transaction a échoué.
    /// </summary>
    public static bool RemoveTickets(int amount)
    {
        return AddTickets(-amount);
    }

    public static bool AddCurrencyAmount(CurrencyAmount montant)
    {
        switch (montant.currencyType)
        {
            case CurrencyType.Coin:
                return AddCoins(montant.amount);
            case CurrencyType.Ticket:
                return AddTickets(montant.amount);
        }
        return false;
    }
    public static bool RemoveCurrentAmount(CurrencyAmount montant)
    {
        switch (montant.currencyType)
        {
            case CurrencyType.Coin:
                return RemoveCoins(montant.amount);
            case CurrencyType.Ticket:
                return RemoveTickets(montant.amount);
        }
        return false;
    }

    private void SaveData()
    {
        dataSaver.SetInt(SAVE_KEY_TICKETS, GetTickets());
        dataSaver.SetInt(SAVE_KEY_COINS, GetCoins());
        dataSaver.Save();
    }

    private void FetchData()
    {
        coins = dataSaver.GetInt(SAVE_KEY_COINS);
        tickets = dataSaver.GetInt(SAVE_KEY_TICKETS);
        if (CurrencyUpdate != null)
        {
            CurrencyUpdate();
        }
    }
}
