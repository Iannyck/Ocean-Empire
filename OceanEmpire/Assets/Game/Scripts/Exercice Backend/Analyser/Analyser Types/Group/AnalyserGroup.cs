using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Analyser/Analyser Group")]
public class AnalyserGroup : ScriptableObject
{
    [SerializeField, Reorderable] private List<BaseAnalyser> analysers = new List<BaseAnalyser>();

    public AnalyserGroupReport GetExerciseVolume(TimeSlot analysedTimeslot)
    {
        AnalyserGroupReport report = new AnalyserGroupReport();

        for (int i = 0; i < analysers.Count; i++)
        {
            report.individualReports.Add(analysers[i].GetExerciseVolume(analysedTimeslot));
        }

        return report;
    }
}
