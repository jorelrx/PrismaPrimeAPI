using System.Globalization;

namespace PrismaPrimeInvest.Application.Extensions;

public static class StringExtensions
{
    private static readonly string[] Formats = ["MM/yyyy", "dd/MM/yyyy", "dd/MM/yyyy HH:mm"];

    public static DateTime ConvertToDateTime(this string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
        {
            throw new ArgumentException("The date string cannot be null or empty.", nameof(dateString));
        }

        foreach (var format in Formats)
        {
            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
        }

        throw new FormatException($"Unable to convert '{dateString}' to DateTime using the expected formats.");
    }
}
