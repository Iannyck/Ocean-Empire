using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Preferences
{
    [System.Serializable]
    public class ExercisePreference
    {
        public ExerciseType type;
        public float Love
        {
            get { return love; }
            set { love = value; }
        }
        [SerializeField, ReadOnly]
        private float love;
    }

    [SerializeField]
    public List<ExercisePreference> myExercises = new List<ExercisePreference>();

    public void FilterExercises(List<ExerciseType> list)
    {
        list.RemoveAll((ExerciseType type) =>
        {
            ExercisePreference preference = myExercises.Find((x) => x.type == type);
            return preference == null || preference.Love == 0;
        });
    }

    Preferences()
    {
        ExercisePreference walking = new ExercisePreference();
        walking.type = ExerciseType.Walk;
        walking.Love = 0f;

        myExercises.Add(walking);
    }
}
