using System;

/// <summary>
/// Structure de donnée représentant un temps bonifié
/// </summary>
[Serializable]
public class BonifiedTime
{
    public TimeSlot timeSlot;
    public Bonus bonus;
    public PossibleExercice.PlannedExercice plannedExercice;

    public BonifiedTime(TimeSlot timeSlot, Bonus bonus)
    {
        this.timeSlot = timeSlot;
        this.bonus = bonus;
        plannedExercice = new PossibleExercice.PlannedExercice(PossibleExercice.PlannedExercice.ExerciceType.marche, 1);
    }

    /// <summary>
    /// Croise le temps bonifié avec la timeslot pour construire un nouveau BonifiedTime
    /// <para/>Retourne null si le bonifiedTime et la timeSlot ne s'overlap pas
    /// </summary>
    public static BonifiedTime Cross(BonifiedTime bonifiedTime, TimeSlot timeSlot) { int bidon; return Cross(bonifiedTime, timeSlot, out bidon); }
    /// <summary>
    /// Croise le temps bonifié avec la timeslot pour construire un nouveau BonifiedTime
    /// <para/>Retourne null si le bonifiedTime et la timeSlot ne s'overlap pas
    /// <para/>-1 = bonifiedTime -> timeSlot
    /// <para/>0 = overlap
    /// <para/>1 = timeSlot -> bonifiedTime
    /// </summary>
    public static BonifiedTime Cross(BonifiedTime bonifiedTime, TimeSlot timeSlot, out int compareResult)
    {
        TimeSlot overlap;
        compareResult = bonifiedTime.timeSlot.IsOverlappingWith(timeSlot, out overlap);

        // OVERLAP !
        if (compareResult == 0)
            return new BonifiedTime(overlap, bonifiedTime.bonus);
        return null;
    }

    /// <summary>
    /// Met en commun deux bonifiedTime pour en construire un nouveau
    /// <para/>Retourne null si les deux temps ne s'overlap pas
    /// </summary>
    public static BonifiedTime Cross(BonifiedTime a, BonifiedTime b) { int bidon; return Cross(a, b, out bidon); }
    /// <summary>
    /// Met en commun deux bonifiedTime pour en construire un nouveau
    /// <para/>Retourne null si les deux temps ne s'overlap pas
    /// <para/>-1 = a -> b
    /// <para/>0 = overlap
    /// <para/>1 = b -> a
    /// </summary>
    public static BonifiedTime Cross(BonifiedTime a, BonifiedTime b, out int compareResult)
    {
        TimeSlot overlap;
        compareResult = a.timeSlot.IsOverlappingWith(b.timeSlot, out overlap);

        // OVERLAP !
        if (compareResult == 0)
            return new BonifiedTime(overlap, Bonus.Join(a.bonus, b.bonus));
        return null;
    }
}
