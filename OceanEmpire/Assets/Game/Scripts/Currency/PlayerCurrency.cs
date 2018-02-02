
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CCC.Persistence;

public class PlayerCurrency : MonoPersistent
{
    private class Currency
    {
        public CurrencyEvent ChangeEvent;
        public string SaveKey;
        public bool TriggerUpdateEvent = true;

        private const string SAVE_KEY_LASTSEEN = "LastSeen";
        private int _amount;
        private int _lastSeenAmount;

        public Currency(string saveKey, CurrencyEvent changeEvent)
        {
            ChangeEvent = changeEvent;
            SaveKey = saveKey;
        }

        public int LastSeenAmount
        {
            get { return _lastSeenAmount; }
            private set { _lastSeenAmount = value; }
        }
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (value == _amount)
                    return;
                _amount = value;

                var delta = _amount - _lastSeenAmount;

                _lastSeenAmount = _amount;

                if (delta != 0 && ChangeEvent != null)
                    ChangeEvent(delta);
                if (TriggerUpdateEvent && CurrencyUpdate != null)
                    CurrencyUpdate();
            }
        }

        public void ApplyDataTo(DataSaver saver)
        {
            saver.SetInt(SAVE_KEY_LASTSEEN + SaveKey, LastSeenAmount);
            saver.SetInt(SaveKey, Amount);
        }

        public void FetchDataFrom(DataSaver saver)
        {
            LastSeenAmount = saver.GetInt(SAVE_KEY_LASTSEEN + SaveKey);
            Amount = saver.GetInt(SaveKey);
        }
    }

    public delegate void CurrencyEvent(int delta);

    /// <summary>
    /// Triggered lorsque les valeurs de currency sont mis a jour 
    /// (ex: apres lecture de sauvegarde, apres ajout de tickets/coins, etc.)
    /// </summary>
    public static event SimpleEvent CurrencyUpdate;

    /// <summary>
    /// Triggered lors que le joueur gagne/perd des coins
    /// </summary>
    public static event CurrencyEvent CoinChange;

    /// <summary>
    /// Triggered lors que le joueur gagne/perd des ticket
    /// </summary>
    public static event CurrencyEvent TicketChange;

    [SerializeField] private Sprite moneyIcon;
    [SerializeField] private Sprite ticketIcon;

    [NonSerialized]
    private Currency coins_ = new Currency("Coin",
        (delta) =>
        {
            if (CoinChange != null)
                CoinChange(delta);
        });

    [NonSerialized]
    private Currency tickets_ = new Currency("Ticket",
        (delta) =>
        {
            if (TicketChange != null)
                TicketChange(delta);
        });

    [SerializeField]
    private DataSaver dataSaver;

    public static PlayerCurrency instance;
    public static bool AutoSave = true;

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


    #region Get
    public static int GetCoins() { return instance.coins_.Amount; }
    public static int GetTickets() { return instance.tickets_.Amount; }

    public static Sprite GetMoneyIcon() { return instance.moneyIcon; }
    public static Sprite GetTicketIcon() { return instance.ticketIcon; }
    #endregion


    #region Add / Remove Currency
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

    public static bool AddCoins(int amount)
    {
        //On empeche le joueur d'aller dans l'argent négatif
        if (amount < 0 && amount.Abs() > GetCoins())
            return false;

        //On ne fait rien si le montant a ajouté == 0
        if (amount == 0)
            return true;

        instance.coins_.Amount += amount;

        if (AutoSave)
            instance.SaveData();

        return true;
    }
    public static bool RemoveCoins(int amount) { return AddCoins(-amount); }

    public static bool AddTickets(int amount)
    {
        //On empeche le joueur d'aller dans les tickets négatif
        if (amount < 0 && amount.Abs() > GetTickets())
            return false;

        //On ne fait rien si le montant a ajouté == 0
        if (amount == 0)
            return true;

        instance.tickets_.Amount += amount;

        if (AutoSave)
            instance.SaveData();

        return true;
    }
    public static bool RemoveTickets(int amount) { return AddTickets(-amount); }
    #endregion


    #region Save/Load
    public void Save() { SaveData(); }
    private void SaveData()
    {
        coins_.ApplyDataTo(dataSaver);
        tickets_.ApplyDataTo(dataSaver);
        dataSaver.Save();
    }

    private void FetchData()
    {
        //Disable event triggering
        coins_.TriggerUpdateEvent = false;
        tickets_.TriggerUpdateEvent = false;

        //Fetch data
        coins_.FetchDataFrom(dataSaver);
        tickets_.FetchDataFrom(dataSaver);

        //Enable event triggering
        coins_.TriggerUpdateEvent = true;
        tickets_.TriggerUpdateEvent = true;

        if (CurrencyUpdate != null)
        {
            CurrencyUpdate();
        }
    }
    #endregion
}
