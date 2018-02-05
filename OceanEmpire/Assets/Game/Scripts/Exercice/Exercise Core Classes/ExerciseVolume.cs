
[System.Serializable]
public struct ExerciseVolume
{
    public ExerciseType type;
    /// <summary>
    /// Valeur quantifiable d'une exercice qui est determiner comme on veut (ex: 12 push up, 10 minutes, 1 km)
    /// </summary>
    public float volume;

    public override bool Equals(object obj)
    {
        if (!(obj is ExerciseVolume))
        {
            return false;
        }

        var objCT = (ExerciseVolume)obj;

        return objCT.type == type && objCT.volume == volume;
    }
    public override int GetHashCode()
    {
        return volume.GetHashCode();
    }
    public override string ToString()
    {
        return ExerciseComponents.GetDisplayName(type) + ": " + volume;
    }
}
