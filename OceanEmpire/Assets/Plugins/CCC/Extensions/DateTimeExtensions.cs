using System;
using System.Text;

public static class DateTimeExtensions
{
    public static string ToEfficientString(this TimeSpan dur)
    {
        StringBuilder txt = new StringBuilder(8)
            .Append(dur.Hours.ToString())
            .Append(':')
            .Append(dur.Minutes.ToString().PadLeft(2, '0'))
            .Append(':')
            .Append(dur.Seconds.ToString().PadLeft(2, '0'));

        return txt.ToString();
    }
    public static DateTime AddWeeks(this DateTime date, int weeks)
    {
        return date.AddDays(weeks * 7);
    }
    public static string ToCondensedTimeOfDayString(this DateTime date, bool withSpaces = false)
    {
        string minutes = date.Minute.ToString();
        if (minutes.Length < 2)
            minutes = minutes.PadLeft(2, '0');

        if (withSpaces)
            return date.Hour + " h " + minutes;
        else
            return date.Hour + "h" + minutes;
    }

    public static DateTime Floored(this DateTime dateTime, TimeSpan interval)
    {
        return dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));
    }

    public static DateTime Ceiled(this DateTime dateTime, TimeSpan interval)
    {
        var overflow = dateTime.Ticks % interval.Ticks;

        return overflow == 0 ? dateTime : dateTime.AddTicks(interval.Ticks - overflow);
    }

    public static DateTime Rounded(this DateTime dateTime, TimeSpan interval)
    {
        var halfIntervelTicks = ((interval.Ticks + 1) >> 1);

        return dateTime.AddTicks(halfIntervelTicks - ((dateTime.Ticks + halfIntervelTicks) % interval.Ticks));
    }
}
