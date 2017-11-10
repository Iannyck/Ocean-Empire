using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FullInspector;

public class ItemsList : BaseManager<ItemsList>
{
   
    public override void Init()
    {
        Load();
        CompleteInit();
    }


    public Dictionary<string, Item> upgradeList;
    [HideInInspector]
    public Dictionary<string, bool> ownedUpgrades;

    [SerializeField, ReadOnly]
    private Thruster equipedThruster;
    public Thruster defaultThruster;


    private const string SAVE_KEY_THRUSTER = "equipedthruster";

    public static bool ItemOwned(string itemID)
    {
        if (instance.ownedUpgrades.ContainsKey(itemID))
            return instance.ownedUpgrades[itemID];
        else
            return false;
    }
    public bool IsEquiped(string itemID)
    {
        if (equipedThruster.GetItemID() == itemID)
            return true;
        else
            return false;
    }


    public static Thruster GetEquipThruster()   {
        return instance.equipedThruster;
    }

    public static void BuyUpgrade(string itemID)
    {
        if (instance.ownedUpgrades.ContainsKey(itemID))
            instance.ownedUpgrades[itemID] = true;
    }


    public static void EquipUpgrade(string itemID)
    {

        if (instance.ownedUpgrades.ContainsKey(itemID) && instance.ownedUpgrades[itemID] == true)
        {
            if (instance.upgradeList[itemID] is Thruster)
            {
                instance.equipedThruster = (Thruster)instance.upgradeList[itemID];
            }

        }
        Save();
    }


    private static void Save()
    {


        /*
         * 
         * Override pour pouvoir plus facilement tester
         * 
         * 
         * 
         * 
         * 
        foreach (KeyValuePair<string, bool> containedIem in instance.ownedUpgrades)
        {
            GameSaves.instance.SetBool(GameSaves.Type.Items, containedIem.Key, containedIem.Value);
        }

        GameSaves.instance.SetString(GameSaves.Type.Items, SAVE_KEY_THRUSTER, instance.equipedThruster.GetItemID());

        GameSaves.instance.SaveData(GameSaves.Type.Items);
        */
    }


    private static void Load()
    {
        if (instance.ownedUpgrades == null)
            instance.ownedUpgrades = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, Item> containedIem in instance.upgradeList)
        {
            string itemID = containedIem.Value.GetItemID();
            if (instance.ownedUpgrades.ContainsKey(itemID) == false)
            {
                bool owned = GameSaves.instance.GetBool(GameSaves.Type.Items, itemID);
                instance.ownedUpgrades.Add(itemID, owned);
            }
        }

        LoadThruster();
    }       


    private static void LoadThruster()
    {
        string thrusterID = GameSaves.instance.GetString(GameSaves.Type.Items, SAVE_KEY_THRUSTER);

        if (instance.upgradeList.ContainsKey(thrusterID) == true && instance.upgradeList[thrusterID] is Thruster)
        {
            instance.equipedThruster = (Thruster)instance.upgradeList[thrusterID];
            if (instance.ownedUpgrades.ContainsKey(thrusterID))
                instance.ownedUpgrades[thrusterID] = true;
        }
        else
        {
            instance.equipedThruster = instance.defaultThruster;

            string defaultID = instance.defaultThruster.GetItemID();
            if (instance.ownedUpgrades.ContainsKey(defaultID))
                instance.ownedUpgrades[defaultID] = true;
        }
    }
}
