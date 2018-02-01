using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utile pour englober une série de rapport d'analyseur.
/// Ex: Quel volume d'exercice (tout type inclu) a accomplie le joueur entre 3pm et 4pm -> GroupAnalyserReport.
/// </summary>
[System.Serializable]
public class GroupAnalyserReport
{
    public List<AnalyserReport> individualReports;

    //TODO: Ajouter des fonctions helper pour interpréter les données de groupe. Ex: GetTotalActiveTime
}
