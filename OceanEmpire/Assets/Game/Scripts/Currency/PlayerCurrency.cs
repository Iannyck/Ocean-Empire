
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

                CurrencyEventArgs e = new CurrencyEventArgs() { Seen = false };

                if (delta != 0 && ChangeEvent != null)
                {
                    ChangeEvent(delta, e);
                    if (e.Seen)
                    {
                        _lastSeenAmount = _amount;
                    }
                }

                if (TriggerUpdateEvent && CurrencyUpdate != null)
                    CurrencyUpdate();
            }
        }
        public int UnseenDelta { get { return Amount - LastSeenAmount; } }

        /// <summary>
        /// Indique que le joueur a vue le changement de monnaie
        /// </summary>
        public void SeeDelta()
        {
            _lastSeenAmount = _amount;
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

    public delegate void CurrencyEvent(int delta, CurrencyEventArgs eventArgs);

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

    /// <summary>
    /// Triggered lors que le joueur gagne/perd des harpon
    /// </summary>
    public static event CurrencyEvent HarpoonChange;

    [SerializeField] private Sprite moneyIcon;
    [SerializeField] private Sprite ticketIcon;
    [SerializeField] private Sprite harpoonIcon;

    [NonSerialized]
    private Currency coins = new Currency("Coin",
        (delta, e) =>
        {
            if (CoinChange != null)
                CoinChange(delta, e);
        });

    [NonSerialized]
    private Currency tickets = new Currency("Ticket",
        (delta, e) =>
        {
            if (TicketChange != null)
                TicketChange(delta, e);
        });

    [NonSerialized]
    private Currency harpoons = new Currency("Harpoon",
        (delta, e) =>
        {
            if (HarpoonChange != null)
                HarpoonChange(delta, e);
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

    #region Seen
    /// <summary>
    /// Le gain/perte de coins que le joueur n'a pas encore vue
    /// </summary>
    public static int GetUnseenDeltaCoins() { return instance.coins.UnseenDelta; }

    /// <summary>
    /// Indique que le joueur a vue les récents gains/pertes de coins
    /// </summary>
    public static void SeeDeltaCoins()
    {
        instance.coins.SeeDelta();
        if (AutoSave)
            instance.SetDirty();
    }

    /// <summary>
    /// Le gain/perte de tickets que le joueur n'a pas encore vue
    /// </summary>
    public static int GetUnseenDeltaTickets() { return instance.tickets.UnseenDelta; }

    /// <summary>
    /// Indique que le joueur a vue les récents gains/pertes de ticket
    /// </summary>
    public static void SeeDeltaTickets()
    {
        instance.tickets.SeeDelta();
        if (AutoSave)
            instance.SetDirty();
    }

    /// <summary>
    /// Le gain/perte de harpon que le joueur n'a pas encore vue
    /// </summary>
    public static int GetUnseenDeltaHarpoon() { return instance.harpoons.UnseenDelta; }

    /// <summary>
    /// Indique que le joueur a vue les récents gains/pertes de harpon
    /// </summary>
    public static void SeeDeltaHarpoon()
    {
        instance.harpoons.SeeDelta();
        if (AutoSave)
            instance.SetDirty();
    }
    #endregion


    #region Get
    public static int GetCurrency(CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Coin:
                return instance.coins.Amount;
            case CurrencyType.Ticket:
                return instance.tickets.Amount;
            case CurrencyType.Harpoon:
                return instance.harpoons.Amount;
            default:
                return -1;
        }
    }

    public static Sprite GetIcon( CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Coin:
                return instance.moneyIcon;
            case CurrencyType.Ticket:
                return instance.ticketIcon;
            case CurrencyType.Harpoon:
                return instance.harpoonIcon;
            default:
                return null;
        }
    }
    
    public static int GetCoins() { return instance.coins.Amount; }
    public static int GetTickets() { return instance.tickets.Amount; }
    public static int GetHarpoons() { return instance.harpoons.Amount; }

    public static Sprite GetMoneyIcon() { return instance.moneyIcon; }
    public static Sprite GetTicketIcon() { return instance.ticketIcon; }
    public static Sprite GetHarpoonIcon() { return instance.harpoonIcon; }
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
            case CurrencyType.Harpoon:
                return AddHarpoons(montant.amount);
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
            case CurrencyType.Harpoon:
                return RemoveHarpoons(montant.amount);
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

        instance.coins.Amount += amount;

        if (AutoSave)
            instance.SetDirty();

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

        instance.tickets.Amount += amount;

        if (AutoSave)
            instance.SetDirty();

        return true;
    }
    public static bool RemoveTickets(int amount) { return AddTickets(-amount); }

    public static bool AddHarpoons(int amount)
    {
        //On empeche le joueur d'aller dans les tickets négatif
        if (amount < 0 && amount.Abs() > GetHarpoons())
            return false;

        //On ne fait rien si le montant a ajouté == 0
        if (amount == 0)
            return true;

        instance.harpoons.Amount += amount;

        if (AutoSave)
            instance.SetDirty();

        return true;
    }
    public static bool RemoveHarpoons(int amount) { return AddHarpoons(-amount); }
    #endregion


    #region Save/Load

    private void ApplyDataToSaver()
    {
        coins.ApplyDataTo(dataSaver);
        tickets.ApplyDataTo(dataSaver);
        harpoons.ApplyDataTo(dataSaver);
    }
    private void SetDirty()
    {
        ApplyDataToSaver();
        dataSaver.LateSave();
    }

    private void FetchData()
    {
        //Disable event triggering
        coins.TriggerUpdateEvent = false;
        tickets.TriggerUpdateEvent = false;
        harpoons.TriggerUpdateEvent = false;

        //Fetch data
        coins.FetchDataFrom(dataSaver);
        tickets.FetchDataFrom(dataSaver);
        harpoons.FetchDataFrom(dataSaver);

        //Enable event triggering
        coins.TriggerUpdateEvent = true;
        tickets.TriggerUpdateEvent = true;
        harpoons.TriggerUpdateEvent = true;

        if (CurrencyUpdate != null)
        {
            CurrencyUpdate();
        }
    }
    #endregion
}


public class CurrencyEventArgs : EventArgs
{
    public bool Seen { get; set; }
}