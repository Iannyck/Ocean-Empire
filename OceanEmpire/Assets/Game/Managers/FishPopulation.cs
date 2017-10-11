using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FishPopulation : BaseManager<FishPopulation>
{

    private const string SAVE_KEY_POPULATION = "population";

    private const float limitPopulation = 300;
    private TimeSpan refreshingTime = new TimeSpan(0, 0, 3, 0);    //3 minutes

    private float population;
    private DateTime lastUpdate;

    public override void Init()
    {
        population = GameSaves.instance.GetFloat(GameSaves.Type.Shop, SAVE_KEY_POPULATION, limitPopulation);
        lastUpdate = GameSaves.instance.GetDateTime(GameSaves.Type.Shop, SAVE_KEY_POPULATION, System.DateTime.Now);

        CompleteInit();
    }

    public void UpdatePopulation()
    {
        DateTime now = System.DateTime.Now;

        TimeSpan deltaTime = now.Subtract(LastUpdate);
        float refreshRate = (float)(deltaTime.TotalSeconds / refreshingTime.TotalSeconds);

        population = (population += (refreshRate * limitPopulation)).Capped(limitPopulation);
    }


    public float GetPopulationRate()
    {
        return Population/ limitPopulation;
    }


    public static float Population
    {

        private set
        {
            instance.population = value;
            Save();
        }

        get
        {
            return instance.population;
        }
    }

    public static DateTime LastUpdate
    {
        private set
        {
            instance.lastUpdate = value;
            Save();
        }
        get
        {
            return instance.lastUpdate;
        }
    }


    private static void Save()
    {
        GameSaves.instance.SetFloat(GameSaves.Type.Shop, SAVE_KEY_POPULATION, Population);
        GameSaves.instance.SetDateTime(GameSaves.Type.Shop, SAVE_KEY_POPULATION, LastUpdate);
        GameSaves.instance.SaveData(GameSaves.Type.Shop);
    }
}
