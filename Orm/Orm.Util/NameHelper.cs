namespace VkRadio.Orm.Util;

/// <summary>
/// Helper functions to work with names
/// </summary>
public static class NameHelper
{
    static readonly string[] c_monthNamesRu =
    {
        "январь",
        "февраль",
        "март",
        "апрель",
        "май",
        "июнь",
        "июль",
        "август",
        "сентябрь",
        "октябрь",
        "ноябрь",
        "декабрь"
    };

    static readonly char[] c_consonantsRu = { 'б', 'в', 'г', 'д', 'ж', 'з', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ' };

    /// <summary>
    /// Return a name with a big first letter
    /// </summary>
    /// <param name="name">Name</param>
    /// <returns>Name with a big first letter</returns>
    public static string ToUpperFirstLetter(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        var result = name[..1].ToUpper();

        if (name.Length > 1)
        {
            result += name[1..];
        }

        return result;
    }

    /// <summary>
    /// Get a full name month in Russian (with a small first letter, in a nominative case)
    /// </summary>
    /// <param name="monthNumber">Month number, starting with 1 for January</param>
    /// <returns></returns>
    public static string MonthNumberToRusName(int monthNumber) => c_monthNamesRu[monthNumber - 1];

    /// <summary>
    /// Separate a string to a string value and a possible number in braces in the end
    /// </summary>
    /// <param name="inputString">Input string</param>
    /// <returns>Input string with a number in braces, or if no number in braces, assume a number 1</returns>
    static Tuple<string, int> ExtractMainPartAndNumber(string inputString)
    {
        var number = 1;
        var stringValue = (inputString ?? string.Empty).Trim();

        var chars = stringValue
            .ToCharArray()
            .Reverse()
            .ToArray();

        var invertedString = new string(chars);

        if (invertedString.Length >= 3 && invertedString.StartsWith(')'))
        {
            var indexOfClosingBrace = invertedString.IndexOf('(', 1);

            if (indexOfClosingBrace != -1)
            {
                var numberCandidate = invertedString[1..indexOfClosingBrace];

                var digitChars = numberCandidate
                    .ToCharArray()
                    .Reverse()
                    .ToArray();

                var numberStr = new string(digitChars);

                if (int.TryParse(numberStr, out int value))
                {
                    number = value;

                    // If a length of an original input string is equal to a length of a number in braces,
                    // this means a string value will be empty, becase the whole string is a number in braces.
                    // Otherwise a string value will be a beginning of a string up to a number in braces, but without
                    // any spaces and other space symbols between them
                    stringValue = stringValue.Length == digitChars.Length + 2
                        ? string.Empty
                        : stringValue[..(stringValue.Length - digitChars.Length - 2)].Trim();
                }
            }
        }

        return new Tuple<string, int>(stringValue, number);
    }

    /// <summary>
    /// &quot;Increment&quot; an order number of a name
    /// <para>If there is no number in the end of a name, add &quot; (2)&quot;, otherwise increase that number by one</para>
    /// </summary>
    /// <param name="thisString"></param>
    /// <param name="maxLength">Optional: max length of a name (if result will go over it, cut off part of the symbols in the middle)</param>
    /// <returns>Name, that differs from the input by one in braces</returns>
    public static string IncrementName(this string thisString, int? maxLength = null)
    {
        var mainPartAndNumber = ExtractMainPartAndNumber(thisString);
        var newNumber = mainPartAndNumber.Item2 + 1;
        var result = $"{mainPartAndNumber.Item1} ({newNumber})";

        if (maxLength.HasValue)
        {
            if (result.Length > maxLength.Value)
            {
                var numberWBracketsLength = $"({newNumber})".Length;

                if (numberWBracketsLength > maxLength.Value)
                {
                    throw new ApplicationException($"Unable to create a new string value - max allowed length ({maxLength.Value}) is less then a value in braces ({newNumber}).");
                }

                result = numberWBracketsLength == maxLength.Value || numberWBracketsLength + 1 == maxLength.Value
                    ? $"({newNumber})" // If the length of a number with braces is equal to the string length (or less by one - by a space), then result is a value in braces
                    : mainPartAndNumber.Item1.Substring(0, maxLength.Value - numberWBracketsLength - 1) + $" ({newNumber})";
            }
        }

        return result;
    }

    /// <summary>
    /// Sorten a name
    /// </summary>
    /// <param name="fullName">Full name</param>
    /// <returns></returns>
    public static string Shorten(this string fullName)
    {
        var result = string.Empty;
        var words = fullName.Split([' ', '-', '.', ',', '(', ')', '?', '!', ':', ';']);
        var nextChars = new char[words.Length - 1];
        var startIndex = 0;

        for (var i = 0; i < words.Length - 1; i++)
        {
            startIndex += words[i].Length;
            nextChars[i] = fullName[startIndex];
            startIndex++;
        }

        bool wasShortened;

        for (var i = 0; i < words.Length; i++)
        {
            wasShortened = false;
            words[i] = words[i].Trim();

            if (words[i].Length > 3)
            {
                for (var j = 2; j < words[i].Length - 2; j++)
                {
                    var thisChar = words[i][j].ToString().ToLower()[0];
                    var isConsonant = false;

                    foreach (var consChar in c_consonantsRu)
                    {
                        if (thisChar == consChar)
                        {
                            isConsonant = true;

                            break;
                        }
                    }

                    if (isConsonant)
                    {
                        words[i] = words[i].Substring(0, j + 1) + ".";
                        wasShortened = true;

                        break;
                    }
                }
            }

            result += words[i];

            if (wasShortened)
            {
                if (i != words.Length - 1 && nextChars[i] != '.')
                {
                    result += nextChars[i];
                }
            }
            else
            {
                if (i != words.Length - 1)
                {
                    result += nextChars[i];
                }
            }
        }

        return result;
    }
}
