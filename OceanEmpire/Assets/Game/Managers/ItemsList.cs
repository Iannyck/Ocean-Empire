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
    public Dictionary<string, string> mapsPaths;

    [HideInInspector]
    public Dictionary<string, bool> ownedUpgrades;
    [HideInInspector]
    public Dictionary<string, bool> ownedMaps;

    public string defaultMap;

    [SerializeField, ReadOnly]
    private string equipedThruster;

    [SerializeField, ReadOnly]
    private string equipedHarpoon;

    public string defaultThruster;



    private const string SAVE_KEY_THRUSTER = "equipedthruster";
    private const string SAVE_KEY_HARPOON = "equipedharpoon";

    static public T GetItem<T>(string itemID) where T : UnityEngine.Object
    {
        if (itemID == null) return null;

        if (instance.upgradePaths.ContainsKey(itemID))
            return Instantiate(Resources.Load(instance.upgradePaths[itemID])) as T;

        else if (instance.ownedMaps.ContainsKey(itemID))
            return Instantiate(Resources.Load(instance.mapsPaths[itemID])) as T;

        else
            return null;
    }


    public static bool ItemOwned(string itemID)
    {
        if (itemID == null) return false;

        if (instance.ownedUpgrades.ContainsKey(itemID))
            return instance.ownedUpgrades[itemID];

        else if (instance.ownedMaps.ContainsKey(itemID))
            return instance.ownedMaps[itemID];
        else
            return false;
    }

    public static List<MapDescription> GetAllOwnedMaps()
    {
        List<MapDescription> ownedMaps = new List<MapDescription>();

        foreach (KeyValuePair<string, bool> containedIem in instance.ownedMaps)
        {
            if (containedIem.Value == true && instance.mapsPaths.ContainsKey(containedIem.Key))
            {
                ShopMapDescription shopMap = GetItem<ShopMapDescription>(containedIem.Key);
                ownedMaps.Add(shopMap.GetMapDescription());
            }
        }

        return ownedMaps;
    }


    public bool IsEquiped(string itemID)
    {
        if (equipedThruster == itemID || equipedHarpoon == itemID)
            return true;
        else
            return false;
    }


    public static ThrusterDescription GetEquipThruster()
    {
        return GetItem<ThrusterDescription>(instance.equipedThruster);
    }

    public static HarpoonThrowerDescription GetEquipHarpoonThrower()
    {
        return GetItem<HarpoonThrowerDescription>(instance.equipedHarpoon);
    }

    public static void BuyUpgrade(string itemID)
    {

        if (instance.ownedUpgrades.ContainsKey(itemID))
            instance.ownedUpgrades[itemID] = true;
        Save();
    }

    public static void BuyMap(string itemID)
    {

        if (instance.ownedMaps.ContainsKey(itemID))
            instance.ownedMaps[itemID] = true;
        Save();
    }


    public static void EquipUpgrade(UpgradeDescription upgrade)
    {
        string id = upgrade.GetItemID();

        if (instance.ownedUpgrades.ContainsKey(id) && instance.ownedUpgrades[id] == true)
        {
            if (upgrade is ThrusterDescription)
                instance.equipedThruster = upgrade.GetItemID();

            if (upgrade is HarpoonThrowerDescription)
                instance.equipedHarpoon = upgrade.GetItemID();

        }
        Save();
    }


    private static void Save()
    {
        foreach (KeyValuePair<string, bool> containedItem in instance.ownedUpgrades)
        {
            GameSaves.instance.SetBool(GameSaves.Type.Items, containedItem.Key, containedItem.Value);
        }
        foreach (KeyValuePair<string, bool> containedMap in instance.ownedMaps)
        {
            GameSaves.instance.SetBool(GameSaves.Type.Items, containedMap.Key, containedMap.Value);
        }

        GameSaves.instance.SetString(GameSaves.Type.Items, SAVE_KEY_THRUSTER, instance.equipedThruster);
        GameSaves.instance.SetString(GameSaves.Type.Items, SAVE_KEY_HARPOON, instance.equipedHarpoon);

        GameSaves.instance.SaveData(GameSaves.Type.Items);
    }


    private static void Load()
    {

        LoadUpgrades();
        LoadMaps();
    }

    private static void LoadMaps()
    {
        if (instance.ownedMaps == null)
            instance.ownedMaps = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, string> containedIem in instance.mapsPaths)
        {
            string itemID = containedIem.Key;
            if (instance.ownedMaps.ContainsKey(itemID) == false)
            {
                bool owned = GameSaves.instance.GetBool(GameSaves.Type.Items, itemID);
                instance.ownedMaps.Add(itemID, owned);
            }
        }

        if (instance.ownedMaps.ContainsKey(instance.defaultMap))
            instance.ownedMaps[instance.defaultMap] = true;
    }

    private static void LoadUpgrades()
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
        LoadHarpoon();
    }

    private static void LoadThruster()
    {
        string thrusterID = GameSaves.instance.GetString(GameSaves.Type.Items, SAVE_KEY_THRUSTER);

        if (thrusterID != null && instance.upgradePaths.ContainsKey(thrusterID) == true)
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

    private static void LoadHarpoon()
    {
        string harpoonID = GameSaves.instance.GetString(GameSaves.Type.Items, SAVE_KEY_HARPOON);
        if (harpoonID != null && instance.upgradePaths.ContainsKey(harpoonID) == true)
        {
            instance.equipedHarpoon = harpoonID;
        }
        else
        {
            instance.equipedHarpoon = null;
            harpoonID = "";
        }
    }


    public static void ResetToDefaults()
    {


        Dictionary<string, bool> copy = new Dictionary< string, bool> (instance.ownedUpgrades);
        foreach (KeyValuePair<string, bool> containedItem in copy)
        {
            instance.ownedUpgrades[containedItem.Key] = false;
        }

        copy = new Dictionary<string, bool>(instance.ownedMaps); 
        foreach (KeyValuePair<string, bool> containedItem2 in copy)
        {
            instance.ownedMaps[containedItem2.Key] = false;
        }

        instance.equipedThruster = instance.defaultThruster;

        if (instance.ownedUpgrades.ContainsKey(instance.equipedThruster))
            instance.ownedUpgrades[instance.equipedThruster] = true;

        instance.equipedHarpoon = null;

        if (instance.ownedMaps.ContainsKey(instance.defaultMap))
            instance.ownedMaps[instance.defaultMap] = true;

        Save();
    }

}