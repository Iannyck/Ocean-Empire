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



    [SerializeField, ReadOnly]
    private string equipedThruster;

    [SerializeField, ReadOnly]
    private string equipedHarpoon;

    [SerializeField, ReadOnly]
    private string equipedFishContainer;

    [SerializeField, ReadOnly]
    private string equipedGazTank;

    public string defaultThruster;
    public string defaultMap;
    public string defaultFishContainer;
    public string defaultGazTank;

    private const string SAVE_KEY_THRUSTER = "equipedthruster";
    private const string SAVE_KEY_HARPOON = "equipedharpoon";
    private const string SAVE_KEY_FISHCONTAINER = "equipedfishcontainer";
    private const string SAVE_KEY_GAZTANK = "equipedgaztank";

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
        if (equipedThruster == itemID || equipedHarpoon == itemID || equipedGazTank == itemID || equipedFishContainer == itemID )
            return true;
        else
            return false;
    }

    public static FishContainerDescription GetEquipFishContainer()
    {
        return GetItem<FishContainerDescription>(instance.equipedFishContainer);
    }

    public static ThrusterDescription GetEquipThruster()
    {
        return GetItem<ThrusterDescription>(instance.equipedThruster);
    }

    public static HarpoonThrowerDescription GetEquipHarpoonThrower()
    {
        return GetItem<HarpoonThrowerDescription>(instance.equipedHarpoon);
    }

    public static GazTankDescription GetEquipGazTank()
    {
        return GetItem<GazTankDescription>(instance.equipedGazTank);
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

            if (upgrade is FishContainerDescription)
                instance.equipedFishContainer = upgrade.GetItemID();

            if (upgrade is GazTankDescription)
                instance.equipedGazTank = upgrade.GetItemID();
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
        GameSaves.instance.SetString(GameSaves.Type.Items, SAVE_KEY_FISHCONTAINER, instance.equipedFishContainer);
        GameSaves.instance.SetString(GameSaves.Type.Items, SAVE_KEY_GAZTANK, instance.equipedGazTank);

        GameSaves.instance.SaveData(GameSaves.Type.Items);
    }


    private static void Load()
    {

        LoadUpgrades();
        LoadMaps();
    }

    private static void LoadMaps()
    {
        if (instance.ownedMaps != null)
            instance.ownedMaps.Clear();
        instance.ownedMaps = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, string> containedIem in instance.mapsPaths)
        {
            string itemID = containedIem.Key;
            if (instance.ownedMaps.ContainsKey(itemID) == false)
            {
                bool owned = GameSaves.instance.GetBool(GameSaves.Type.Items, itemID, false);
                instance.ownedMaps.Add(itemID, owned);
            }
        }

        if (instance.ownedMaps.ContainsKey(instance.defaultMap))
            instance.ownedMaps[instance.defaultMap] = true;
    }

    private static void LoadUpgrades()
    {
        if (instance.ownedUpgrades != null)
            instance.ownedUpgrades.Clear();
        instance.ownedUpgrades = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, string> containedIem in instance.upgradePaths)
        {
            string itemID = containedIem.Key;
            if (instance.ownedUpgrades.ContainsKey(itemID) == false)
            {
                bool owned = GameSaves.instance.GetBool(GameSaves.Type.Items, itemID, false);
                instance.ownedUpgrades.Add(itemID, owned);
            }
        }

        LoadThruster();
        LoadHarpoon();
        LoadFishContainer();
        LoadGazTank();
    }


    private static void LoadGazTank()
    {
        string gazTankID = GameSaves.instance.GetString(GameSaves.Type.Items, SAVE_KEY_GAZTANK);

        if (gazTankID != null && instance.upgradePaths.ContainsKey(gazTankID) == true)
        {
            instance.equipedGazTank = gazTankID;
        }
        else
        {
            instance.equipedGazTank = instance.defaultGazTank;
            gazTankID = instance.defaultGazTank;
        }
        if (instance.ownedUpgrades.ContainsKey(gazTankID))
            instance.ownedUpgrades[gazTankID] = true;

    }

    private static void LoadFishContainer()
    {
        string fishContainerID = GameSaves.instance.GetString(GameSaves.Type.Items, SAVE_KEY_FISHCONTAINER);

        if (fishContainerID != null && instance.upgradePaths.ContainsKey(fishContainerID) == true)
        {
            instance.equipedFishContainer = fishContainerID;
        }
        else
        {
            instance.equipedFishContainer = instance.defaultFishContainer;
            fishContainerID = instance.defaultFishContainer;
        }
        if (instance.ownedUpgrades.ContainsKey(fishContainerID))
            instance.ownedUpgrades[fishContainerID] = true;

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


    public static void Reload()
    {
        Load();
    }

    public static void UnlockAll()
    {
        foreach (KeyValuePair<string, bool> containedItem in instance.ownedUpgrades)
        {
            instance.ownedUpgrades[containedItem.Key] = true;
        }
        foreach (KeyValuePair<string, bool> containedMap in instance.ownedMaps)
        {
            instance.ownedMaps[containedMap.Key] = true;
        }
    }
}