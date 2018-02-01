
public static class ExerciseValue
{
    /// <summary>
    /// La valeur d'un exercice pour un volume donnée
    /// </summary>
    public static float GetValue(ExerciseType type, float volume)
    {
        return GetBaseValue(type) * volume;
    }

    /// <summary>
    /// La valeur unitaire d'un type d'exercice (valeur, par unité de volume)
    /// </summary>
    public static float GetBaseValue(ExerciseType type)
    {
        switch (type)
        {
            case ExerciseType.Walk:
                return 1;
            case ExerciseType.Run:
                return 2;
            case ExerciseType.Stairs:
                return 2;
            default:
                return -1;
        }
    }

}
