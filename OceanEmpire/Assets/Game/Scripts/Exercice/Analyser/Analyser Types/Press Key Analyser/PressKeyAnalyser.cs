using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Analyser/Press Key Analyser")]
public class PressKeyAnalyser : BaseAnalyser
{
    [SerializeField] private float volumePerPress = 1;

    public override float CalculateExerciceVolume(GoogleActivities.ActivityReport previous, GoogleActivities.ActivityReport now)
    {
        return 0;
    }

    public override AnalyserReport GetExerciseVolume(TimeSlot analysedTimeslot)
    {
        if (KeyPressRecorder.instance == null)
        {
            Debug.LogError("PressKeyAnalyser needs an instance of KeyPressRecorder.");
            return null;
        }

        var presses = KeyPressRecorder.instance.GetKeyPresses();
        float volume = 0;

        DateTime firstActivity = analysedTimeslot.end;
        DateTime lastActivity = analysedTimeslot.start;

        for (int i = 0; i < presses.Count; i++)
        {
            if (analysedTimeslot.IsOverlappingWith(presses[i]) == 0)
            {
                if (presses[i] < firstActivity)
                    firstActivity = presses[i];

                if (presses[i] > lastActivity)
                    lastActivity = presses[i];

                volume += volumePerPress;
            }
        }

        var exerciseVolume = new ExerciseVolume()
        {
            type = ExerciseType.PressKey,
            volume = volume
        };

        if (lastActivity < firstActivity)
            lastActivity = firstActivity;

        var activeTimeslot = new TimeSlot(firstActivity, lastActivity);

        var activeRate = activeTimeslot.duration.TotalMilliseconds / analysedTimeslot.duration.TotalMilliseconds;

        return new AnalyserReport(exerciseVolume, analysedTimeslot, activeTimeslot, (float)activeRate);
    }
}
