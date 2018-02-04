 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FullInspector;
using CCC.Persistence;

public class ItemsList : BaseBehavior, IPersistent
{
    [SerializeField] private DataSaver dataSaver;

    public Dictionary<string, string> upgradePaths;
    public Dictionary<string, string> mapsPaths;

    [HideInInspector] public Dictionary<string, bool> ownedUpgrades;
    [HideInInspector] public Dictionary<string, bool> ownedMaps;

    [SerializeField, ReadOnly] private string equipedThruster;
    [SerializeField, ReadOnly] private string equipedHarpoon;
    [SerializeField, ReadOnly] private string equipedFishContainer;
    [SerializeField, ReadOnly] private string equipedGazTank;

    public string defaultThruster;
    public string defaultMap;
    public string defaultFishContainer;
    public string defaultGazTank;

    private const string SAVE_KEY_THRUSTER = "equipedthruster";
    private const string SAVE_KEY_HARPOON = "equipedharpoon";
    private const string SAVE_KEY_FISHCONTAINER = "equipedfishcontainer";
    private const string SAVE_KEY_GAZTANK = "equipedgaztank";

    public static ItemsList instance;

    protected override void Awake()
    {
        base.Awake();

        dataSaver.OnReassignData += Load;
    }

    public void Init(Action onComplete)
    {
        instance = this;
        Load();
        onComplete();
    }

    public UnityEngine.Object DuplicationBehavior()
    {
        return this.DuplicateGO();
    }

    static public T GetItemDuplicate<T>(string itemID) where T : UnityEngine.Object
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
                ShopMapDescription shopMap = GetItemDuplicate<ShopMapDescription>(containedIem.Key);
                ownedMaps.Add(shopMap.GetMapDescription());
            }
        }

        return ownedMaps;
    }


    public bool IsEquiped(string itemID)
    {
        if (equipedThruster == itemID || equipedHarpoon == itemID || equipedGazTank == itemID || equipedFishContainer == itemID)
            return true;
        else
            return false;
    }

    public static FishContainerDescription GetEquipFishContainer()
    {
        return GetItemDuplicate<FishContainerDescription>(instance.equipedFishContainer);
    }

    public static ThrusterDescription GetEquipThruster()
    {
        return GetItemDuplicate<ThrusterDescription>(instance.equipedThruster);
    }

    public static HarpoonThrowerDescription GetEquipHarpoonThrower()
    {
        return GetItemDuplicate<HarpoonThrowerDescription>(instance.equipedHarpoon);
    }

    public static GazTankDescription GetEquipGazTank()
    {
        return GetItemDuplicate<GazTankDescription>(instance.equipedGazTank);
    }

    private static bool BuyItem(ItemDescription item, CurrencyType currencyType, Dictionary<string, bool> bank)
    {
        //--------------Check Null--------------//

        if (item == null)
        {
            MessagePopup.DisplayMessage("L'item est null.");
            return false;
        }


        //--------------In Bank--------------//

        string itemID = item.GetItemID();
        if (!bank.ContainsKey(itemID))
        {
            MessagePopup.DisplayMessage("L'item n'est pas dans la liste des upgrades.");
            return false;
        }


        //--------------Already owned--------------//

        if (bank[itemID])
        {
            MessagePopup.DisplayMessage("L'item a d\u00E9ja \u00E9t\u00E9 achet\u00E9.");
            return false;
        }


        //--------------Montant--------------//

        CurrencyAmount montant = new CurrencyAmount() { currencyType = currencyType };
        switch (currencyType)
        {
            case CurrencyType.Coin:
                montant.amount = item.GetMoneyCost();
                break;
            case CurrencyType.Ticket:
                montant.amount = item.GetTicketCost();
                break;
        }
        bool purchaseResult = PlayerCurrency.RemoveCurrentAmount(montant);


        //--------------Finlité--------------//

        if (!purchaseResult)
        {
            MessagePopup.DisplayMessage("Failed to purchase to upgrade");
            return false;
        }
        else
        {
            PurchaseReport report = new PurchaseReport(montant, item.GetName(), item.GetItemID());
            try { History.instance.AddPurchaseReport(report); }
            catch (Exception e)
            {
                MessagePopup.DisplayMessage("Failed to add purchase report to history.\n\n" + e.Message);
                return false;
            }

            bank[itemID] = true;
            instance.Save();
            return true;
        }
    }

    public static bool BuyUpgrade(string itemID, CurrencyType currencyType)
    {
        return BuyUpgrade(GetItemDuplicate<UpgradeDescriptionOLD>(itemID), currencyType);
    }
    public static bool BuyUpgrade(UpgradeDescriptionOLD upgrade, CurrencyType currencyType)
    {
        return BuyItem(upgrade, currencyType, instance.ownedUpgrades);
    }
    public static bool BuyMap(string mapID, CurrencyType currencyType)
    {
        return BuyMap(GetItemDuplicate<ShopMapDescription>(mapID), currencyType);
    }
    public static bool BuyMap(ShopMapDescription map, CurrencyType currencyType)
    {
        return BuyItem(map, currencyType, instance.ownedMaps);
    }


    public static void EquipUpgrade(UpgradeDescriptionOLD upgrade)
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
        instance.Save();
    }


    private void Save()
    {
        foreach (KeyValuePair<string, bool> containedItem in instance.ownedUpgrades)
        {
            dataSaver.SetBool(containedItem.Key, containedItem.Value);
        }
        foreach (KeyValuePair<string, bool> containedMap in instance.ownedMaps)
        {
            dataSaver.SetBool(containedMap.Key, containedMap.Value);
        }

        dataSaver.SetString(SAVE_KEY_THRUSTER, instance.equipedThruster);
        dataSaver.SetString(SAVE_KEY_HARPOON, instance.equipedHarpoon);
        dataSaver.SetString(SAVE_KEY_FISHCONTAINER, instance.equipedFishContainer);
        dataSaver.SetString(SAVE_KEY_GAZTANK, instance.equipedGazTank);

        dataSaver.Save();
    }


    private void Load()
    {

        LoadUpgrades();
        LoadMaps();
    }

    private void LoadMaps()
    {
        if (ownedMaps != null)
            ownedMaps.Clear();
        ownedMaps = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, string> containedIem in mapsPaths)
        {
            string itemID = containedIem.Key;
            if (ownedMaps.ContainsKey(itemID) == false)
            {
                bool owned = dataSaver.GetBool(itemID, false);
                ownedMaps.Add(itemID, owned);
            }
        }

        if (ownedMaps.ContainsKey(defaultMap))
            ownedMaps[defaultMap] = true;
    }

    private void LoadUpgrades()
    {
        if (ownedUpgrades != null)
            ownedUpgrades.Clear();
        ownedUpgrades = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, string> containedIem in upgradePaths)
        {
            string itemID = containedIem.Key;
            if (ownedUpgrades.ContainsKey(itemID) == false)
            {
                bool owned = dataSaver.GetBool( itemID, false);
                ownedUpgrades.Add(itemID, owned);
            }
        }

        LoadThruster();
        LoadHarpoon();
        LoadFishContainer();
        LoadGazTank();
    }


    private void LoadGazTank()
    {
        string gazTankID = dataSaver.GetString(SAVE_KEY_GAZTANK);

        if (gazTankID != null && upgradePaths.ContainsKey(gazTankID) == true)
        {
            equipedGazTank = gazTankID;
        }
        else
        {
            equipedGazTank = defaultGazTank;
            gazTankID = defaultGazTank;
        }
        if (ownedUpgrades.ContainsKey(gazTankID))
            ownedUpgrades[gazTankID] = true;

    }

    private void LoadFishContainer()
    {
        string fishContainerID = dataSaver.GetString(SAVE_KEY_FISHCONTAINER);

        if (fishContainerID != null && upgradePaths.ContainsKey(fishContainerID) == true)
        {
            equipedFishContainer = fishContainerID;
        }
        else
        {
            equipedFishContainer = defaultFishContainer;
            fishContainerID = defaultFishContainer;
        }
        if (ownedUpgrades.ContainsKey(fishContainerID))
            ownedUpgrades[fishContainerID] = true;

    }


    private void LoadThruster()
    {
        string thrusterID = dataSaver.GetString(SAVE_KEY_THRUSTER);

        if (thrusterID != null && upgradePaths.ContainsKey(thrusterID) == true)
        {
            equipedThruster = thrusterID;
        }
        else
        {
            equipedThruster = defaultThruster;
            thrusterID = defaultThruster;
        }
        if (ownedUpgrades.ContainsKey(thrusterID))
            ownedUpgrades[thrusterID] = true;
    }

    private void LoadHarpoon()
    {
        string harpoonID = dataSaver.GetString(SAVE_KEY_HARPOON);

        if (harpoonID != null && upgradePaths.ContainsKey(harpoonID) == true)
        {
            equipedHarpoon = harpoonID;
        }
        else
        {
            equipedHarpoon = null;
            harpoonID = "";
        }
    }

    public static void UnlockAll()
    {
        List<string> keys = new List<string>(instance.ownedUpgrades.Keys);
        foreach (string key in keys)
        {
            instance.ownedUpgrades[key] = true;
        }

        keys = new List<string>(instance.ownedMaps.Keys); ;
        foreach (string key in keys)
        {
            instance.ownedMaps[key] = true;
        }
    }
}