using UnityEngine;

public abstract class BaseAnalyser : ScriptableObject
{
    public abstract AnalyserReport GetExerciseVolume(TimeSlot analysedTimeslot);

    public abstract float CalculateExerciceVolume(GoogleActivities.ActivityReport previous, GoogleActivities.ActivityReport now);
}
