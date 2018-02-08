public struct OneWayBool
{
    public OneWayBool(bool startValue)
    {
        this.startValue = startValue;
        value = startValue;
    }
    private bool value;
    private bool startValue;

    public void FlipValue()
    {
        value = !startValue;
    }
    public static implicit operator bool (OneWayBool val)
    {
        return val.Value;
    }
    public bool Value
    {
        get
        {
            return value;
        }
    }

    /// <summary>
    /// Retourne vrai si la valeur a été affecté avec succès
    /// </summary>
    public bool TryToSet(bool newValue)
    {
        if (value == startValue)
        {
            value = newValue;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Equals(object obj)
    {
        if(obj is OneWayBool)
        {
            return ((OneWayBool)obj).value == value;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
