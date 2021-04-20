using System;
using System.Globalization;

namespace VkRadio.LowCode.Gui.Utils
{
    public static class FormatHelper
    {
        public const string C_DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";
        public const string C_DATE_TIME_FORMAT_Z = "yyyy-MM-ddTHH:mm:ssZ";

        /// <summary>
        /// На входе подается значение int, на выходе получаем строку
        /// для десятичного представления (например, для PFTMoney).
        /// </summary>
        /// <param name="in_value"></param>
        /// <returns></returns>
        public static string GetDecimalString(int in_value, int in_decimalPositions = 2)
        {
            string result = in_value.ToString();
            if (in_value < 0)
                result = result.Substring(1);

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
        /// Делает то же, что и GetDecimalString, но на входе подается &quot;нормализованная&quot;
        /// до минимальной длины строка, т.е. чтобы как миниум в ней был ноль в целом разряде.
        /// </summary>
        /// <param name="in_fullStringNotSeparated"></param>
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
