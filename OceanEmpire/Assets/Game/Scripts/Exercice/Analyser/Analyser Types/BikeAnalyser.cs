using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeAnalyser : BaseAnalyser
{
    public float multiplier = 1.0f;
    public float minimumValidation = 50;

    public override float CalculateExerciceVolume(GoogleActivities.ActivityReport previous, GoogleActivities.ActivityReport now)
    {
        // Nombre de minute a faire du bicycle
        return now.time.Subtract(previous.time).Seconds * multiplier;
    }

    public override AnalyserReport GetExerciseVolume(TimeSlot analysedTimeslot)
    {
        // Informations sur les exercices fait
        List<GoogleActivities.ActivityReport> activities = GoogleActivities.instance.records;

        // Parametres a calculer
        ExerciseVolume volume;
        // Le volume est de la marche
        volume.type = ExerciseType.Bike;
        // Volume start a 0
        volume.volume = 0;

        // Temps premier et dernier exercice pour rapport
        bool firstExericeEncounter = true;
        DateTime firstExerciceTime = new DateTime();
        DateTime lastExerciceTime = new DateTime();

        bool nextExitIsOutOfTimeslot = false;

        for (int i = 0; i < activities.Count; i++)
        {
            GoogleActivities.ActivityReport currentRecord = activities[i];
            GoogleActivities.ActivityReport previousRecord = null;
            if (i != 0)
                previousRecord = activities[i - 1];

            // Si l'exercice est dans la timeslot
            if (currentRecord.time >= analysedTimeslot.start && currentRecord.time <= analysedTimeslot.end)
            {
                // Prochaine fois qu'on rentre pas c'est qu'on a fini notre job
                nextExitIsOutOfTimeslot = true;

                // as-t-on vraiment un enregistrement precedent ?
                if (previousRecord != null)
                {
                    // L'exercice precedent etait du bicycle ?
                    if (previousRecord.best.type == PrioritySheet.ExerciseTypes.bicycle && previousRecord.best.rate >= minimumValidation)
                    {
                        // Oui, on considere donc la difference de temps comme etant le volume
                        volume.volume += CalculateExerciceVolume(previousRecord, currentRecord);

                        if (firstExericeEncounter)
                        {
                            firstExericeEncounter = false;
                            firstExerciceTime = previousRecord.time;
                        }
                    }
                }
            }
            else if (nextExitIsOutOfTimeslot) // On a fini, dernier calcul et on retourne le resultat
            {
                // L'exercice precedent etait du bicycle ?
                if (previousRecord.best.type == PrioritySheet.ExerciseTypes.bicycle && previousRecord.best.rate >= minimumValidation)
                {
                    // Oui, on considere donc la difference de temps comme etant le volume
                    volume.volume += CalculateExerciceVolume(previousRecord, currentRecord);
                    lastExerciceTime = currentRecord.time;
                }
                break;
            }
        }

        // construction du rapport
        //Debug.Log("Volume | " + volume + "(" + analysedTimeslot.start + "-" + analysedTimeslot.end + ")");
        AnalyserReport report = new AnalyserReport(volume, analysedTimeslot, firstExerciceTime, lastExerciceTime);

        // envoie du rapport
        return report;
    }
}

