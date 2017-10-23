using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences
{
    public class ExercisePreference
    {
        public ExerciseType type;
        public float Love
        {
            get { return love; }
            set { love = value; }
        }
        private float love;
    }

    public List<ExercisePreference> myExercises = new List<ExercisePreference>();

    public void FilterExercises(List<ExerciseType> list)
    {
        list.RemoveAll((ExerciseType type) =>
        {
            ExercisePreference preference = myExercises.Find((x) => x.type == type);
            return preference == null || preference.Love == 0;
        });
    }
}
