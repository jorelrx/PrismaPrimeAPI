using System.Globalization;
using Newtonsoft.Json;

namespace PrismaPrimeInvest.Application.Extensions;

public static class DateTimeExtensions
{
    private static HashSet<DateTime>? _brazilianHolidays;

    public static DateTime GetLastBusinessDayOfMonth(this DateTime date)
    {
        DateTime lastDay = new(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

        while (lastDay.IsWeekend() || lastDay.IsHoliday())
        {
            lastDay = lastDay.AddDays(-1);
        }

        return lastDay;
    }
    public static DateTime GetLastBusinessDay(this DateTime date)
    {
        DateTime lastBusinessDay = date;

        while (lastBusinessDay.IsWeekend() || lastBusinessDay.IsHoliday())
        {
            lastBusinessDay = lastBusinessDay.AddDays(-1);
        }

        return lastBusinessDay;
    }

    public static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    public static bool IsHoliday(this DateTime date)
    {
        return _brazilianHolidays.Contains(date.Date);
    }

    public static void LoadHolidaysFromJson(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Arquivo de feriados não encontrado: {filePath}");

        var json = File.ReadAllText(filePath);
        var holidayStrings = JsonConvert.DeserializeObject<List<string>>(json);
        _brazilianHolidays = [.. holidayStrings.Select(date => DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture))];
    }
}
