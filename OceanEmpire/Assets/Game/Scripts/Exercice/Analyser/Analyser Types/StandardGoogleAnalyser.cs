using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StandardGoogleAnalyser : BaseAnalyser
{
    public float multiplier = 1.0f;
    public float minimumValidation = 50;

    public abstract PrioritySheet.ExerciseTypes GoogleExerciseType
    {
        get;
    }
    public abstract ExerciseType ExerciseType
    {
        get;
    }

    public override float CalculateExerciceVolume(GoogleActivities.ActivityReport previous, GoogleActivities.ActivityReport now)
    {
        // Nombre de secondes marcher
        return CalculateExerciceVolume(previous.time, now.time);
    }

    public float CalculateExerciceVolume(DateTime leftmost, DateTime rightmost)
    {
        if (rightmost <= leftmost)
            return 0;

        // Nombre de secondes marcher
        return ((float)rightmost.Subtract(leftmost).TotalSeconds * multiplier) / 60;
    }
    
    public override AnalyserReport GetExerciseVolume(TimeSlot analysedTimeslot)
    {
        // Informations sur les exercices fait
        List<GoogleActivities.ActivityReport> activities = GoogleActivities.instance.records;

        ExerciseVolume volume = new ExerciseVolume
        {
            type = ExerciseType,
            volume = 0
        };


        bool firstExericeEncounter = false;
        bool hasExitedTheTimeslot = false;
        DateTime firstExerciceTime = new DateTime();
        DateTime lastExerciceTime = new DateTime();
        GoogleActivities.ActivityReport currentRecord = null;
        GoogleActivities.ActivityReport previousRecord = null;

        for (int i = 0; i < activities.Count; i++)
        {
            previousRecord = currentRecord;
            currentRecord = activities[i];

            if (i != 0)
                previousRecord = activities[i - 1];

            int overlap = analysedTimeslot.IsOverlappingWith(currentRecord.time);
            // -1 -> on a dépassé dans le futur
            //  0 -> on est dans la timeslot
            //  1 -> on est encore dans le passé, continuons

            if (overlap == 0) // Exercice est dans la timeslot
            {
                if (previousRecord == null)
                {
                    // Situation: On fait face à notre premier record, mais la timeslot à analyser débute plus tôt
                    // Solution: On considère que tout le temps AVANT le premier record était identique au record
                    if (IsRecordValid(currentRecord))
                    {
                        volume.volume += CalculateExerciceVolume(analysedTimeslot.start, currentRecord.time);
                        lastExerciceTime = currentRecord.time;

                        if (!firstExericeEncounter)
                        {
                            firstExericeEncounter = true;
                            firstExerciceTime = currentRecord.time;
                        }
                    }
                }
                else
                {
                    // Situation: On est pas à notre premier record
                    DateTime inbetween = new DateTime((previousRecord.time.Ticks + currentRecord.time.Ticks) / 2);

                    if (IsRecordValid(previousRecord))
                    {
                        // On compte la moitié de droite du record précédent
                        DateTime latest = analysedTimeslot.start < previousRecord.time ? previousRecord.time : analysedTimeslot.start;
                        float vol = CalculateExerciceVolume(latest, inbetween);
                        volume.volume += vol;
                        lastExerciceTime = inbetween;

                        if (vol > 0 && !firstExericeEncounter)
                        {
                            firstExericeEncounter = true;
                            firstExerciceTime = latest;
                        }
                    }

                    if (IsRecordValid(currentRecord))
                    {
                        // On compte la moitié de gauche du record courant
                        DateTime latest = analysedTimeslot.start < inbetween ? inbetween : analysedTimeslot.start;
                        volume.volume += CalculateExerciceVolume(latest, currentRecord.time);
                        lastExerciceTime = currentRecord.time;

                        if (!firstExericeEncounter)
                        {
                            firstExericeEncounter = true;
                            firstExerciceTime = latest;
                        }
                    }
                }
            }
            else if (overlap == -1) // On a dépassé la timeslot dans le futur, dernier calcul et on retourne le resultat
            {
                if (previousRecord == null)
                {
                    //Situation: La timeslot complête est sans record
                    //Solution: On considère qu'il n'y a aucun exercice
                }
                else
                {
                    // Situation: On est pas à notre premier record
                    DateTime inbetween = new DateTime((previousRecord.time.Ticks + currentRecord.time.Ticks) / 2);

                    if (IsRecordValid(previousRecord))
                    {
                        // On compte la moitié de droite du record précédent
                        DateTime right = analysedTimeslot.end < inbetween ? analysedTimeslot.end : inbetween;
                        DateTime left = analysedTimeslot.start < previousRecord.time ? previousRecord.time : analysedTimeslot.start;
                        volume.volume += CalculateExerciceVolume(left, right);
                        lastExerciceTime = right;
                        hasExitedTheTimeslot = true;
                    }

                    if (IsRecordValid(currentRecord))
                    {
                        // On compte la moitié de gauche du record courant
                        DateTime left = analysedTimeslot.start < inbetween ? inbetween : analysedTimeslot.start;
                        volume.volume += CalculateExerciceVolume(left, analysedTimeslot.end);
                        lastExerciceTime = analysedTimeslot.end;
                        hasExitedTheTimeslot = true;
                    }
                }

                break;
            }
        }

        if (!hasExitedTheTimeslot && IsRecordValid(currentRecord))
        {
            DateTime right = analysedTimeslot.end;
            DateTime left = analysedTimeslot.start < currentRecord.time ? currentRecord.time : analysedTimeslot.start;
            volume.volume += CalculateExerciceVolume(left, right);
            lastExerciceTime = right;
        }

        AnalyserReport report = new AnalyserReport(volume, analysedTimeslot, firstExerciceTime, lastExerciceTime);
        
        return report;
    }

    private bool IsRecordValid(GoogleActivities.ActivityReport report)
    {
        return report != null && report.best.type == GoogleExerciseType && report.best.rate >= minimumValidation;
    }
}
