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


    public Dictionary<string, string> upgradePaths;

    [HideInInspector]
    public Dictionary<string, bool> ownedUpgrades;

    [SerializeField, ReadOnly]
    private string equipedThruster;
    public string defaultThruster;

    private const string SAVE_KEY_THRUSTER = "equipedthruster";


    virtual public T GetItem<T>(string itemID) where T : UnityEngine.Object
    {
        if (upgradePaths.ContainsKey(itemID))
            return Instantiate(Resources.Load(upgradePaths[itemID])) as T;
        else
            return null;
    }


    public static bool ItemOwned(string itemID)
    {
        if (instance.ownedUpgrades.ContainsKey(itemID))
            return instance.ownedUpgrades[itemID];
        else
            return false;
    }


    public bool IsEquiped(string itemID)
    {
        if (equipedThruster == itemID)
            return true;
        else
            return false;
    }


    public static ThrusterDescription GetEquipThruster()
    {
        return instance.GetItem<ThrusterDescription>(instance.equipedThruster);
    }

    public static void BuyUpgrade(string itemID)
    {
        if (instance.ownedUpgrades.ContainsKey(itemID))
            instance.ownedUpgrades[itemID] = true;
    }



    public static void EquipUpgrade( UpgradeDescription upgrade)
    {
        string id = upgrade.GetItemID();

        if (instance.ownedUpgrades.ContainsKey(id) && instance.ownedUpgrades[id] == true)
        {
            if (upgrade is ThrusterDescription)
                instance.equipedThruster = upgrade.GetItemID();
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
        
        foreach (KeyValuePair<string, string> containedIem in instance.upgradePaths)
        {
            string itemID = containedIem.Key;
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

        if (instance.upgradePaths.ContainsKey(thrusterID) == true)
        {
            instance.equipedThruster = thrusterID;
        }
        else
        {
            instance.equipedThruster = instance.defaultThruster;
            thrusterID = instance.defaultThruster;
        }
        if (instance.ownedUpgrades.ContainsKey(thrusterID))
            instance.ownedUpgrades[thrusterID] = true;
    }
}
