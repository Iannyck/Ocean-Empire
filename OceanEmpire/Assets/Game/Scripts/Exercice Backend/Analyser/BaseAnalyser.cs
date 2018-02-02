using UnityEngine;

public abstract class BaseAnalyser : ScriptableObject
{
    public abstract AnalyserReport GetExerciseVolume(TimeSlot analysedTimeslot);
}
