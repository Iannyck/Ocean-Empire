using UnityEngine;
using CCC.Persistence;
using System;
using System.Text;

public class Logger : MonoPersistent
{
    public enum Category
    {
        GameEvent,
        PlannedExercise,
        Shop,
        Questing,
        ContinuousReward
    }
    [SerializeField] private DataSaver saver;
    private const string SAVE_KEY = "log";

    public static Logger Instance { get; private set; }
    private StringBuilder stringBuilder;
    private bool isDirty = false;

    protected void Awake()
    {
        saver.OnReassignData += FetchData;
    }

    public override void Init(Action onComplete)
    {
        Instance = this;
        FetchData();
        onComplete();
    }

    void FetchData()
    {
        stringBuilder = new StringBuilder(saver.GetString(SAVE_KEY));
        isDirty = false;
    }

    public void SaveAsync()
    {
        saver.SetString(SAVE_KEY, stringBuilder.ToString());
        saver.SaveAsync();
        isDirty = false;
    }
    public void Save()
    {
        saver.SetString(SAVE_KEY, stringBuilder.ToString());
        saver.Save();
        isDirty = false;
    }

    public static void Log(Category category, string text)
    {
        Instance.stringBuilder.Append(Calendar.Now.ToString()).Append(',')
            .Append(category.ToString()).Append(',')
            .Append(text).Append('\n');
        Instance.isDirty = true;
    }
    public string GetTotalLogWithHeader()
    {
        return "TIMESTAMP,TYPE,DESCRIPTION\n" + stringBuilder.ToString();
    }

    void LateUpdate()
    {
        if (isDirty)
            SaveAsync();
    }
}

