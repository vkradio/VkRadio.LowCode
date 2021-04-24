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
        /// <param name="in_value"></param>
        /// <param name="in_decimalPositions"></param>
        /// <returns></returns>
        public static string GetDecimalString(int in_value, int in_decimalPositions = 2)
        {
            string result = in_value.ToString();
            if (in_value < 0)
                result = result[1..];

            if (result.Length <= in_decimalPositions)
            {
                int deltaLength = in_decimalPositions - result.Length;
                for (int i = 0; i <= deltaLength; i++)
                    result = "0" + result;
            }

            if (in_value < 0)
                result = "-" + result;
            
            return GetDecimalStringForFullString(result);
        }
        /// <summary>
        /// Does the same as GetDecimalString, but as input receiving a string &quot;normalized&quot;
        /// to minimal length, so that it contains at least zero as a whole part.
        /// </summary>
        /// <param name="in_fullStringNotSeparated"></param>
        /// <param name="in_decimalPositions"></param>
        /// <returns></returns>
        public static string GetDecimalStringForFullString(string in_fullStringNotSeparated, int in_decimalPositions = 2)
        {
            return
                in_fullStringNotSeparated.Substring(0, in_fullStringNotSeparated.Length - in_decimalPositions) +
                (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator ?? ".") +
                in_fullStringNotSeparated.Substring(in_fullStringNotSeparated.Length - in_decimalPositions);
        }

        public static string[] SplitTextToLines(string in_text)
        {
            return (in_text ?? string.Empty).Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
    };
}
