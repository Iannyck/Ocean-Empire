[System.Serializable]
public class BoolReference : VarReference<BoolVariable, bool>
{
    public BoolReference() : base()
    { }

    public BoolReference(bool value) : base(value)
    { }
}