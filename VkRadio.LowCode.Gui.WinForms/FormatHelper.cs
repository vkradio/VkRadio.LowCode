using Ardalis.GuardClauses;
using System;
using System.Globalization;

namespace VkRadio.LowCode.Gui.Utils
{
    public static class FormatHelper
    {
        public const string C_DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";
        public const string C_DATE_TIME_FORMAT_Z = "yyyy-MM-ddTHH:mm:ssZ";

        /// <summary>
        /// Receiving int value and returning its decimal representation (for example,
        /// for PFTMoney).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPositions"></param>
        /// <returns></returns>
        public static string GetDecimalString(int value, int decimalPositions = 2)
        {
            var result = value.ToString(CultureInfo.CurrentCulture);
            if (value < 0)
                result = result[1..];

            if (result.Length <= decimalPositions)
            {
                var deltaLength = decimalPositions - result.Length;
                for (var i = 0; i <= deltaLength; i++)
                    result = "0" + result;
            }

            if (value < 0)
                result = "-" + result;
            
            return GetDecimalStringForFullString(result);
        }
        /// <summary>
        /// Does the same as GetDecimalString, but as input receiving a string, &quot;normalized&quot;
        /// to minimal length, so that it contains at least zero as a whole part.
        /// </summary>
        /// <param name="fullStringNotSeparated"></param>
        /// <param name="decimalPositions"></param>
        /// <returns></returns>
        public static string GetDecimalStringForFullString(string fullStringNotSeparated, int decimalPositions = 2)
        {
            Guard.Against.Null(fullStringNotSeparated, nameof(fullStringNotSeparated));

            return
                fullStringNotSeparated.Substring(0, fullStringNotSeparated.Length - decimalPositions) +
                (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator ?? ".") +
                fullStringNotSeparated[^decimalPositions..];
        }

        public static string[] SplitTextToLines(string text) => (text ?? string.Empty).Split(new string[] { "\r\n" }, StringSplitOptions.None);
    }
}
