public struct OneWayBool
{
    public OneWayBool(bool startValue)
    {
        this.startValue = startValue;
        value = startValue;
    }
    private bool value;
    private bool startValue;

    public void Invert()
    {
        value = !startValue;
    }
    public static implicit operator bool (OneWayBool val)
    {
        return val.State;
    }
    public bool State
    {
        get
        {
            return value;
        }
    }
}
