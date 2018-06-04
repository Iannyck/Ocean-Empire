using UnityEditor;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : HideShowBaseDrawer
{
    protected override bool DefaultMemberValue
    {
        get
        {
            return true;
        }
    }

    protected override bool IsShownIfMemberTrue
    {
        get
        {
            return true;
        }
    }
}
