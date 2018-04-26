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
        public int minAmount;
        public bool ended;

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
}
