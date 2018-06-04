using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute
{
    public readonly bool forwardToChildren = true;
    public ReadOnlyAttribute() { }
}
