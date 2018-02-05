﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAnalyser : BaseAnalyser {

    public float multiplier = 1.0f;

    public override float CalculateExerciceVolume(GoogleActivities.ActivityReport previous, GoogleActivities.ActivityReport now)
    {
        // Nombre de minute marcher
        return now.time.Subtract(previous.time).Minutes * multiplier;
    }

    public override AnalyserReport GetExerciseVolume(TimeSlot analysedTimeslot)
    {
        // Informations sur les exercices fait
        List<GoogleActivities.ActivityReport> activities = GoogleActivities.instance.records;

        // Parametres a calculer
        ExerciseVolume volume;
        // Le volume est de la marche
        volume.type = ExerciseType.Walk;
        // Volume start a 0
        volume.volume = 0;

        // Temps premier et dernier exercice pour rapport
        bool firstExericeEncounter = false;
        DateTime firstExerciceTime = new DateTime();
        DateTime lastExerciceTime = new DateTime();

        // Gestion d'une zone d'exercice
        bool previousExerciceWasWalk = false;
        GoogleActivities.ActivityReport zoneFirstExercice = null;
        GoogleActivities.ActivityReport zoneLastExercice = null;

        for (int i = 0; i < activities.Count; i++)
        {
            // Si l'exercice est dans la timeslot
            if(activities[i].time >= analysedTimeslot.start && activities[i].time <= analysedTimeslot.end)
            {
                // Si l'exercice precedent etait marche
                if (previousExerciceWasWalk)
                {
                    // On compte les donnees de l'exercice dans le volume total
                    volume.volume += CalculateExerciceVolume(zoneLastExercice, activities[i]);

                    // Si on a deja rencontrer le premier exercice, on enregistre le courrant comme etant le dernier rencontrer
                    if (firstExericeEncounter)
                        zoneLastExercice = activities[i-1];
                }

                // Si l'exercice est de la marche
                if (activities[i].best.type == PrioritySheet.ExerciseTypes.walk)
                {
                    // Si on a pas encore rencontrer le premier exercice
                    if (!firstExericeEncounter)
                    {
                        // On enregistre son temps
                        firstExerciceTime = activities[i].time;
                        firstExericeEncounter = true;
                    }

                    // Si l'exercice precedent n'etait pas de la marche, maintenant s'en ai !
                    if (!previousExerciceWasWalk)
                    {
                        previousExerciceWasWalk = true;
                        zoneFirstExercice = activities[i];
                    }
                } else
                {
                    // On est toujours pas en train de marcher
                    previousExerciceWasWalk = false;
                }
            }
            // On prend le temps du dernier exercice
            lastExerciceTime = zoneLastExercice.time;
        }

        // construction du rapport
        AnalyserReport report = new AnalyserReport(volume, analysedTimeslot, firstExerciceTime, lastExerciceTime);

        // envoie du rapport
        return report;
    }
}
