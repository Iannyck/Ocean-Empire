using UnityEditor;

[CustomPropertyDrawer(typeof(HideIfAttribute))]
public class HideIfDrawer : HideShowBaseDrawer
{
    protected override bool DefaultMemberValue
    {
        get
        {
            return false;
        }
    }

    protected override bool IsShownIfMemberTrue
    {
        get
        {
            return false;
        }
    }
}
