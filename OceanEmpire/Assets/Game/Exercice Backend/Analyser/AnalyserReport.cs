using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Un rapport d'une analyse d'exercice. Ex: Quel volume de marche a accomplie le joueur entre 3pm et 4pm -> AnalyserReport.
/// </summary>
[System.Serializable]
public class AnalyserReport
{
    /// <summary>
    /// Le volume d'activité accomplie au total
    /// </summary>
    public ExerciseVolume volume;

    /// <summary>
    /// Le temps analysé
    /// </summary>
    public TimeSlot analysedTimeslot;

    /// <summary>
    /// Le temps dans lequel le joueur à été actif à faire de l'exercice 
    /// (le début de son exercice jusqu'à sa dernière activité)
    /// </summary>
    public TimeSlot activeTimeslot;

    /// <summary>
    /// Le pourcentage de temps active par rapport au activeTimeslot (ex: le joueur a marché 12 des 30min)
    /// </summary>
    public float activeRate;
}
