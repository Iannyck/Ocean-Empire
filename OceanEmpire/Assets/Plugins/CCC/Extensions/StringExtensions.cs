using UnityEngine;

public static class StringExtensions
{
    public static void Log(this string text)
    {
        Debug.Log(text);
    }
    public static void LogFormat(this string text, params object[] args)
    {
        Debug.LogFormat(text, args);
    }
    public static void LogWarning(this string text)
    {
        Debug.LogWarning(text);
    }
    public static void LogWarningFormat(this string text, params object[] args)
    {
        Debug.LogWarningFormat(text, args);
    }
    public static void LogError(this string text)
    {
        Debug.LogError(text);
    }
    public static void LogErrorFormat(this string text, params object[] args)
    {
        Debug.LogErrorFormat(text, args);
    }
}
