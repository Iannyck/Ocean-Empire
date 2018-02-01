
[System.Serializable]
public struct ExerciseVolume
{
    public ExerciseType type;
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
