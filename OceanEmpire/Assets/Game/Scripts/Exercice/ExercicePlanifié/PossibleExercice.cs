using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleExercice {

    public class PlannedExercice
    {
        public enum ExerciceType
        {
            marche = 0,
            course = 1,
            bicycle = 2 
        }

        public ExerciceType type;
        public int difficulty;
        public float minAmount;
        public bool ended;

        // Fin de session, on hardcore toute
        public PlannedExercice(ExerciceType type, int difficulty)
        {
            this.type = type;
            this.difficulty = difficulty;
            ended = false;
            switch (type)
            {
                case ExerciceType.marche:
                    switch (difficulty)
                    {
                        case 1:
                            minAmount = 3;
                            break;
                        case 2:
                            minAmount = 5;
                            break;
                        case 3:
                            minAmount = 8;
                            break;
                        case 4:
                            minAmount = 10;
                            break;
                        case 5:
                            minAmount = 12;
                            break;
                        case 6:
                            minAmount = 15;
                            break;
                        case 7:
                            minAmount = 18;
                            break;
                        case 8:
                            minAmount = 20;
                            break;
                        case 9:
                            minAmount = 25;
                            break;
                        case 10:
                            minAmount = 30;
                            break;
                        default:
                            break;
                    }
                    break;
                case ExerciceType.course:
                    switch (difficulty)
                    {
                        case 1:
                            minAmount = 1;
                            break;
                        case 2:
                            minAmount = 1;
                            break;
                        case 3:
                            minAmount = 1;
                            break;
                        case 4:
                            minAmount = 1;
                            break;
                        case 5:
                            minAmount = 1;
                            break;
                        case 6:
                            minAmount = 1;
                            break;
                        case 7:
                            minAmount = 1;
                            break;
                        case 8:
                            minAmount = 1;
                            break;
                        case 9:
                            minAmount = 1;
                            break;
                        case 10:
                            minAmount = 1;
                            break;
                        default:
                            break;
                    }
                    break;
                case ExerciceType.bicycle:
                    switch (difficulty)
                    {
                        case 1:
                            minAmount = 1;
                            break;
                        case 2:
                            minAmount = 1;
                            break;
                        case 3:
                            minAmount = 1;
                            break;
                        case 4:
                            minAmount = 1;
                            break;
                        case 5:
                            minAmount = 1;
                            break;
                        case 6:
                            minAmount = 1;
                            break;
                        case 7:
                            minAmount = 1;
                            break;
                        case 8:
                            minAmount = 1;
                            break;
                        case 9:
                            minAmount = 1;
                            break;
                        case 10:
                            minAmount = 1;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    minAmount = 0;
                    break;
            }
        } 
    }

	public static PlannedExercice CreateExercice(PlannedExercice.ExerciceType type, int difficulty)
    {
        return new PlannedExercice(type, difficulty);
    }

    public static string GetInfo(PlannedExercice.ExerciceType type, int difficulty)
    {
        switch (type)
        {
            case PlannedExercice.ExerciceType.marche:
                switch (difficulty)
                {
                    case 1:
                        int temps1 = 3;
                        return "Entre " + temps1 + " et " + (temps1 * 2) + " minutes";
                    case 2:
                        int temps2 = 5;
                        return "Entre " + temps2 + " et " + (temps2 * 2) + " minutes";
                    case 3:
                        int temps3 = 8;
                        return "Entre " + temps3 + " et " + (temps3 * 2) + " minutes";
                    case 4:
                        int temps4 = 10;
                        return "Entre " + temps4 + " et " + (temps4 * 2) + " minutes";
                    case 5:
                        int temps5 = 12;
                        return "Entre " + temps5 + " et " + (temps5 * 2) + " minutes";
                    case 6:
                        int temps6 = 15;
                        return "Entre " + temps6 + " et " + (temps6 * 2) + " minutes";
                    case 7:
                        int temps7 = 18;
                        return "Entre " + temps7 + " et " + (temps7 * 2) + " minutes";
                    case 8:
                        int temps8 = 20;
                        return "Entre " + temps8 + " et " + (temps8 * 2) + " minutes";
                    case 9:
                        int temps9 = 25;
                        return "Entre " + temps9 + " et " + (temps9 * 2) + " minutes";
                    case 10:
                        int temps10 = 30;
                        return "Entre " + temps10 + " et " + (temps10 * 2) + " minutes";
                    default:
                        return "";
                }
            case PlannedExercice.ExerciceType.course:
                switch (difficulty)
                {
                    case 1:
                        int temps1 = 1;
                        return "Entre " + temps1 + " et " + (temps1 * 2) + " minutes";
                    case 2:
                        int temps2 = 1;
                        return "Entre " + temps2 + " et " + (temps2 * 2) + " minutes";
                    case 3:
                        int temps3 = 1;
                        return "Entre " + temps3 + " et " + (temps3 * 2) + " minutes";
                    case 4:
                        int temps4 = 1;
                        return "Entre " + temps4 + " et " + (temps4 * 2) + " minutes";
                    case 5:
                        int temps5 = 1;
                        return "Entre " + temps5 + " et " + (temps5 * 2) + " minutes";
                    case 6:
                        int temps6 = 1;
                        return "Entre " + temps6 + " et " + (temps6 * 2) + " minutes";
                    case 7:
                        int temps7 = 1;
                        return "Entre " + temps7 + " et " + (temps7 * 2) + " minutes";
                    case 8:
                        int temps8 = 1;
                        return "Entre " + temps8 + " et " + (temps8 * 2) + " minutes";
                    case 9:
                        int temps9 = 1;
                        return "Entre " + temps9 + " et " + (temps9 * 2) + " minutes";
                    case 10:
                        int temps10 = 1;
                        return "Entre " + temps10 + " et " + (temps10 * 2) + " minutes";
                    default:
                        return "";
                }
            case PlannedExercice.ExerciceType.bicycle:
                switch (difficulty)
                {
                    case 1:
                        int temps1 = 1;
                        return "Entre " + temps1 + " et " + (temps1 * 2) + " minutes";
                    case 2:
                        int temps2 = 1;
                        return "Entre " + temps2 + " et " + (temps2 * 2) + " minutes";
                    case 3:
                        int temps3 = 1;
                        return "Entre " + temps3 + " et " + (temps3 * 2) + " minutes";
                    case 4:
                        int temps4 = 1;
                        return "Entre " + temps4 + " et " + (temps4 * 2) + " minutes";
                    case 5:
                        int temps5 = 1;
                        return "Entre " + temps5 + " et " + (temps5 * 2) + " minutes";
                    case 6:
                        int temps6 = 1;
                        return "Entre " + temps6 + " et " + (temps6 * 2) + " minutes";
                    case 7:
                        int temps7 = 1;
                        return "Entre " + temps7 + " et " + (temps7 * 2) + " minutes";
                    case 8:
                        int temps8 = 1;
                        return "Entre " + temps8 + " et " + (temps8 * 2) + " minutes";
                    case 9:
                        int temps9 = 1;
                        return "Entre " + temps9 + " et " + (temps9 * 2) + " minutes";
                    case 10:
                        int temps10 = 1;
                        return "Entre " + temps10 + " et " + (temps10 * 2) + " minutes";
                    default:
                        return "";
                }
            default:
                return "";
        }
    }
}
