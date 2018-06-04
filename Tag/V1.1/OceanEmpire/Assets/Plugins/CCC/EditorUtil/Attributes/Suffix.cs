using UnityEngine;

public class Suffix : PropertyAttribute
{
    public string text;
    public Suffix(string text)
    {
        this.text = text;
    }
}