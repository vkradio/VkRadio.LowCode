using System.Globalization;

namespace VkRadio.Orm.Util;

public static class FormatHelper
{
    public const string C_DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";
    public const string C_DATE_TIME_FORMAT_Z = "yyyy-MM-ddTHH:mm:ssZ";

    /// <summary>
    /// Decimal string of some integer value (for example, for PFTMoney)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDecimalString(int value, int decimalPositions = 2)
    {
        var result = value.ToString();

        if (value < 0)
        {
            result = result[1..];
        }

        if (result.Length <= decimalPositions)
        {
            var deltaLength = decimalPositions - result.Length;

            for (var i = 0; i <= deltaLength; i++)
            {
                result = "0" + result;
            }
        }

        if (value < 0)
        {
            result = "-" + result;
        }
        
        return GetDecimalStringForFullString(result);
    }

    /// <summary>
    /// Same as GetDecimalString, but receives input as a &quot;normalized&quot; to a minimal length string,
    /// such that it have at least a zero in an integer part
    /// </summary>
    /// <param name="fullStringNotSeparated"></param>
    /// <returns></returns>
    public static string GetDecimalStringForFullString(string fullStringNotSeparated, int decimalPositions = 2) =>
        fullStringNotSeparated.Substring(0, fullStringNotSeparated.Length - decimalPositions) +
        (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator ?? ".") +
        fullStringNotSeparated.Substring(fullStringNotSeparated.Length - decimalPositions);

    public static string[] SplitTextToLines(string text) => (text ?? string.Empty).Split(["\r\n"], StringSplitOptions.None);
}
