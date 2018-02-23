using UnityEngine;
using System;
using CCC.Persistence;

public class FishPopulation : MonoPersistent
{
    private const string SAVE_KEY_POPULATION = "population";
    private const string SAVE_KEY_LASTUPDATE = "lastUpdate";

    [SerializeField, ReadOnly] private const float limitPopulation = 200;
    private TimeSpan refreshingTime = new TimeSpan(0, 0, 80, 0);

    public float PopulationDensityVariation = 4;

    [SerializeField, ReadOnly] private float population;
    private DateTime lastUpdate;

    [SerializeField] private DataSaver dataSaver;
    private float time = 0;

    public bool periodicLogs = false;

    public static FishPopulation instance;

    protected void Awake()
    {
        dataSaver.OnReassignData += FetchData;
    }

    public override void Init(Action onComplete)
    {
        instance = this;
        FetchData();
        onComplete();
    }

    public void Update()
    {
        if (!periodicLogs)
            return;

        if (time < 0)
        {
            print(population);
            time = 4;
        }
        time -= Time.deltaTime;
    }

    public static TimeSpan GetTimeToRefill()
    {
        return new TimeSpan((long)((float)instance.refreshingTime.Ticks * (1.0f - PopulationRate)));
    }

    private void FetchData()
    {
        population = dataSaver.GetFloat(SAVE_KEY_POPULATION, limitPopulation);
        lastUpdate = (DateTime)dataSaver.GetObjectClone(SAVE_KEY_LASTUPDATE, System.DateTime.Now);
    }

    private void SaveData()
    {
        dataSaver.SetFloat(SAVE_KEY_POPULATION, Population);
        dataSaver.SetObjectClone(SAVE_KEY_LASTUPDATE, LastUpdate);

        dataSaver.Save();
    }

    /// <summary>
    /// 0 = 0%      1 = 100%
    /// </summary>
    public void AddRate(float value)
    {
        PopulationRate = Mathf.Min(value + PopulationRate, 1);
    }

    public float FishNumberToRate(float fishes)
    {
        return fishes / limitPopulation;
    }

    public static float PopulationRate
    {
        private set { Population = value * limitPopulation; }
        get { return Population / limitPopulation; }
    }

    public static float FishDensity
    {
        get { return Mathf.Pow(FishPopulation.PopulationRate, instance.PopulationDensityVariation); }
    }

    public static float GetFishDensityFromRate(float rate)
    {
        return Mathf.Pow(rate, instance.PopulationDensityVariation);
    }

    public static float Population
    {
        private set
        {
            instance.population = value.Clamped(0, limitPopulation);
        }
        get { return instance.population; }
    }

    public static DateTime LastUpdate
    {
        private set { instance.lastUpdate = value; }
        get { return instance.lastUpdate; }

    }

    public static void LowerRate(float value)
    {
        PopulationRate = (PopulationRate - value).Raised(0);
    }

    public void RefreshPopulation()
    {
        DateTime now = System.DateTime.Now;

        TimeSpan deltaTime = now - LastUpdate;
        float refreshRate = (float)(deltaTime.TotalSeconds / refreshingTime.TotalSeconds);

        population = (population += (refreshRate * limitPopulation)).Capped(limitPopulation);
        LastUpdate = now;
    }


    void OnApplicationQuit()
    {
        RefreshPopulation();
        SaveData();
    }

    public void UpdateOnFishing(float capturedFishes)
    {
        LastUpdate = System.DateTime.Now;
        Population -= capturedFishes.Raised(0.0f);

        SaveData();
    }

    public void UpdateOnExercise(float exerciseRateValue)
    {
        RefreshPopulation();
        PopulationRate += exerciseRateValue.Capped(limitPopulation);

        SaveData();
    }

    public void DebugInGameExercice()
    {
        UpdateOnExercise(50);
        if (Game.Instance != null)
            if (Game.FishSpawner != null)
                Game.FishSpawner.SetPalierFishLimit();
    }
}
