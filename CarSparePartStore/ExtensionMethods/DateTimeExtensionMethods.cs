using System;
using System.Linq;

namespace CarSparePartStore.ExtensionMethods;

public static class DateTimeExtensionMethods
{
    public static DateTime AddTime(this DateTime dateTime, string time)
    {
        if (string.IsNullOrEmpty(time))
        {
            return dateTime;
        }
        if (time.Length != 4)
        {
            throw new ArgumentException("Length of time must be exactly 4 characters");
        }
        if (!int.TryParse(time.Substring(0,2), out var hours))
        {
            throw new ArgumentException("Invalid time argument - hours must be a number");
        }
        if (!int.TryParse(time.Substring(2,2), out var minutes))
        {
            throw new ArgumentException("Invalid time argument - minutes must be a number");
        }
        if (hours < 0 || hours >= 24)
        {
            throw new ArgumentException("Invalid time argument - hours must be a number between 0 and 23");
        }
        if (minutes < 0 || minutes >= 60)
        {
            throw new ArgumentException("Invalid time argument - minutes must be a number between 0 and 59");
        }
        return dateTime.AddHours(hours).AddMinutes(minutes);
    }
}