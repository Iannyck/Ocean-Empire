using System;

public static class DateTimeExtensions
{
    public static DateTime AddWeeks(this DateTime date, int weeks)
    {
        return date.AddDays(weeks * 7);
    }
    public static string ToCondensedDayOfTimeString(this DateTime date)
    {
        string minutes = date.Minute.ToString();
        if (minutes.Length < 2)
            minutes.Insert(0, "0");
        return date.Hour + "h" + minutes;
    }
}
