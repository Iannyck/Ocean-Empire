public class ShowIfAttribute : HideShowBaseAttribute
{
    public ShowIfAttribute(string name, Type type = Type.Field) : base(name, type) { }
}