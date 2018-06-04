[System.Serializable]
public class IntReference : VarReference<IntVariable, int>
{
    public IntReference() : base()
    { }

    public IntReference(int value) : base(value)
    { }
}