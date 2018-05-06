

public static class PossibleTasks
{
    public static void GetTaskDurations(int difficulty, out float minDuration, out float maxDuration)
    {
        // minDuration: la VRAI durée de l'exercice
        // maxDuration: la durée maximal de l'exercice (ce n'est que pour rendre la période de temps flou au joueur)

        // NB: Pour l'instant, c'est une simple formule,
        // mais on pourrait mettre un tableau pour avoir des chiffres plus rounds

        minDuration = difficulty;
        maxDuration = (difficulty + 2) + difficulty * 0.25f;
    }
}
