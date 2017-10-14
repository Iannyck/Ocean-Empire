using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCurrency : BaseManager<PlayerCurrency>
{
    private const string SAVE_KEY_COINS = "coins";
    private const string SAVE_KEY_TICKETS = "tickets";

    public override void Init()
    {
        Load();
        CompleteInit();
    }

    [SerializeField, ReadOnly]
    private int coins;
    [SerializeField, ReadOnly]
    private int tickets;

    public static int GetCoins()
    {
        return instance.coins;
    }

    private static void AddCoinsAndSave(int amount)
    {
        instance.coins += amount;
        Save();
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

        AddCoinsAndSave(amount);
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

    private static void AddTicketsAndSave(int amount)
    {
        instance.tickets += amount;
        Save();
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

        AddTicketsAndSave(amount);
        return true;
    }

    /// <summary>
    /// Ajoute des tickets ET sauvegarde. Retourne FALSE si la transaction a échoué.
    /// </summary>
    public static bool RemoveTickets(int amount)
    {
        return AddTickets(-amount);
    }

    private static void Save()
    {
        GameSaves.instance.SetInt(GameSaves.Type.Currency, SAVE_KEY_TICKETS, GetTickets());
        GameSaves.instance.SetInt(GameSaves.Type.Currency, SAVE_KEY_COINS, GetCoins());
        GameSaves.instance.SaveData(GameSaves.Type.Currency);
    }

    private static void Load()
    {
        instance.coins = GameSaves.instance.GetInt(GameSaves.Type.Currency, SAVE_KEY_COINS);
    }
}
