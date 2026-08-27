using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.Names.Translit.RU;

namespace VkRadio.LowCode.AppGenerator;

public class NameHelper
{
    static bool LastLetterIsSLike(string word)
    {
        char lastLetter = word[word.Length - 1];
        char prevLastLetter = word[word.Length - 2];

        return
            lastLetter == 's' ||
            (prevLastLetter == 's' && lastLetter == 'h') ||
            (prevLastLetter == 'c' && lastLetter == 'h');
    }

    public static string[] GetNameWords(string name)
    {
        var preparedName = name
            .Replace("(", string.Empty)
            .Replace(")", string.Empty)
            .Replace(":", string.Empty)
            .Replace("'", string.Empty)
            .Replace(",", string.Empty)
            .Replace(".", string.Empty)
            .Replace(":", string.Empty)
            .Replace(";", string.Empty)
            .Replace("?", string.Empty)
            .Replace("!", string.Empty)
            .Replace("\"", string.Empty)
            .Replace("%", string.Empty)
            .Trim();

        return preparedName.Split([' ', '-', '/']);
    }

    public static string[] GetNameWords(IDictionary<HumanLanguageEnum, string> names)
    {
        return GetNameWords(names[HumanLanguageEnum.En]);
    }

    public static string NamesToPascalCase(IDictionary<HumanLanguageEnum, string> names, bool firstLetterUpperCase)
    {
        var words = GetNameWords(names);
        var name = string.Empty;

        for (var i = 0; i < words.Length; i++)
        {
            var nextWord = words[i].ToLower();

            if (i != 0 || firstLetterUpperCase)
            {
                if (nextWord != string.Empty)
                {
                    nextWord = char.ToUpper(nextWord[0]) + (nextWord.Length > 1 ? nextWord.Substring(1) : string.Empty);
                    name += nextWord;
                }
            }
            else
            {
                name += nextWord;
            }
        }

        return name;
    }

    public static string NamesToPascalCase(IDictionary<HumanLanguageEnum, string> names) => NamesToPascalCase(names, true);

    public static string NamesToCamelCase(IDictionary<HumanLanguageEnum, string> names) => NamesToPascalCase(names, false);

    public static string NamesToCamelCasePlural(IDictionary<HumanLanguageEnum, string> names)
    {
        var result = NamesToCamelCase(names);

        if (result.Length > 1)
        {
            if (result[result.Length - 1] == 'y')
            {
                result = result.Substring(0, result.Length - 1) + "ies";
            }
            else if (LastLetterIsSLike(result))
            {
                result += "es";
            }
            else
            {
                result += "s";
            }
        }

        return result;
    }

    /// <summary>
    /// Are there next non-empty words
    /// </summary>
    /// <param name="words">Words</param>
    /// <param name="thisIndex">Zero-based index of a current workd, beginning from which searching for non-empty words</param>
    /// <returns></returns>
    static bool NextWordsAreNotEmpty(string[] words, int thisIndex)
    {
        var result = false;

        for (var i = thisIndex + 1; i < words.Length; i++)
        {
            if (words[i] != string.Empty)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Extract a name, words from which separated by a _ symbol, from a DOT name
    /// </summary>
    /// <param name="dotName">DOT name as a base</param>
    public static string NameToUnderscoreSeparatedName(string dotName)
    {
        var words = GetNameWords(dotName);
        var name = string.Empty;

        for (var i = 0; i < words.Length; i++)
        {
            var nextWord = words[i].ToLower();

            if (nextWord != string.Empty)
            {
                name += nextWord;

                if (NextWordsAreNotEmpty(words, i))
                {
                    name += "_";
                }
            }
        }

        return name;
    }

    /// <summary>
    /// Extract a name, words from which separated by a _ symbol, from a DOT name
    /// </summary>
    /// <param name="in_dotName">DOT name as a base</param>
    public static string NameToUnderscoreSeparatedName(IDictionary<HumanLanguageEnum, string> names) => NameToUnderscoreSeparatedName(names[HumanLanguageEnum.En]);

    /// <summary>
    /// Get an Id constant name from an object name, with a priority for English name (but if does not exist, transliterate a localized value)
    /// </summary>
    /// <param name="names">Object name package</param>
    /// <returns>Constant name looking like C_ID_WORD1_WORD2_WORDN</returns>
    public static string NameToConstantId(IDictionary<HumanLanguageEnum, string> names) => NameToConstant(names, true, true);

    /// <summary>
    /// Get an Id constant name from an object name, with a priority for English name (but if does not exist, transliterate a localized value)
    /// </summary>
    /// <param name="names">Object name package</param>
    /// <param name="addCPrefix">Add C_ prefix</param>
    /// <param name="idConstant">Is the constant an Id of an object (false by default)</param>
    /// <returns>Constant name looking like C_[ID_]WORD1_WORD2_WORDN</returns>
    public static string NameToConstant(IDictionary<HumanLanguageEnum, string> names, bool addCPrefix = true, bool idConstant = false)
    {
        var name = names.ContainsKey(HumanLanguageEnum.En)
            ? names[HumanLanguageEnum.En]
            : Transliteration.Front(names[HumanLanguageEnum.Ru]);

        var words = GetNameWords(name);
        var result = string.Empty;

        foreach (var word in words)
        {
            if (result != string.Empty)
            {
                result += "_";
            }

            result += word.ToUpper();
        }

        return string.Format("{0}{1}{2}", addCPrefix ? "C_" : string.Empty, idConstant ? "ID_" : string.Empty, result);
    }

    /// <summary>
    /// Create a name in a localized language beginning from a big letter
    /// </summary>
    /// <param name="names">Name dictionary</param>
    /// <returns>Localized name with a big first letter</returns>
    public static string GetLocalNameUpperCase(IDictionary<HumanLanguageEnum, string> names)
    {
        var localName = GetLocalNameLowerCase(names);

        if (localName.Length > 0)
        {
            localName = char.ToUpper(localName[0]) + (localName.Length > 1 ? localName.Substring(1) : string.Empty);
        }

        return localName;
    }

    /// <summary>
    /// Create a name in a localized language beginning from a small letter
    /// </summary>
    /// <param name="names">Name dictionary</param>
    /// <returns>Localized name with a small first letter</returns>
    public static string GetLocalNameLowerCase(IDictionary<HumanLanguageEnum, string> names)
    {
        var localName = names.ContainsKey(HumanLanguageEnum.Ru)
            ? names[HumanLanguageEnum.Ru]
            : names[HumanLanguageEnum.En];

        return localName;
    }

    /// <summary>
    /// Filling a string with escapes such that they are represented a valid C# string literal
    /// </summary>
    /// <param name="stringValue"></param>
    /// <returns></returns>
    public static string GetStringSuitableToCSharp(string stringValue) => stringValue.Replace("\"", "\\\"");

    /// <summary>
    /// Filling a string with escapes such that they are represented a valid XML tag value
    /// </summary>
    /// <param name="stringValue"></param>
    /// <returns></returns>
    public static string GetStringSuitableToXmlText(string stringValue) => stringValue
        .Replace("&", "&amp;")
        .Replace("\"", "&quot;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;");

    /// <summary>
    /// If string starts with a digit, add N to the beginning
    /// </summary>
    /// <param name="originalStringValue"></param>
    /// <param name="bigN"></param>
    /// <returns></returns>
    public static string AddBeginningNIfNeeded(string originalStringValue, bool bigN = true)
    {
        var invalidChars = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        if (string.IsNullOrWhiteSpace(originalStringValue))
        {
            return originalStringValue;
        }
        else
        {
            foreach (char ch in invalidChars)
            {
                if (ch == originalStringValue[0])
                {
                    var addN = bigN ? "N" : "n";
                    return addN + originalStringValue;
                }
            }

            return originalStringValue;
        }
    }
}

public static class NameHelperExtension
{
    static readonly char[] c_consonants = ['б', 'в', 'г', 'д', 'ж', 'з', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ'];

    /// <summary>
    /// Shorten a name
    /// </summary>
    /// <param name="thisName">Full name</param>
    /// <param name="maxSymbols">Optionally: max number of symbols</param>
    /// <returns></returns>
    public static string Shorten(this string thisName, int? maxSymbols = null)
    {
        char[] splitChars = [' ', '-', '.', ',', '(', ')', '?', '!', ':', ';'];

        #region First only shorten words, but do not remove them
        var result = string.Empty;
        var words = thisName.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
        var wordsWOEmpty = new List<string>();

        foreach (var w in words)
        {
            if (w.Trim() != string.Empty)
            {
                wordsWOEmpty.Add(w.Trim());
            }
        }

        words = wordsWOEmpty.ToArray();
        var nextChars = new char[words.Length - 1];
        var startIndex = 0;

        for (var i = 0; i < words.Length - 1; i++)
        {
            startIndex += words[i].Length;
            nextChars[i] = thisName[startIndex];
            startIndex++;
        }

        bool shortened;

        for (var i = 0; i < words.Length; i++)
        {
            shortened = false;
            words[i] = words[i].Trim();

            if (words[i].Length > 3)
            {
                for (var j = 2; j < words[i].Length - 2; j++)
                {
                    var thisChar = words[i][j].ToString().ToLower()[0];
                    var isConsonant = false;

                    foreach (var consChar in c_consonants)
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
                        shortened = true;
                        break;
                    }
                }
            }

            result += words[i];

            if (shortened)
            {
                if (i != words.Length - 1 && nextChars[i] != '.' && nextChars[i] != ' ')
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
        #endregion

        #region If the max chanrs value is set, and result is more than that value, try remove words, beginning from the end
        if (maxSymbols.HasValue && result.Length > maxSymbols.Value)
        {
            words = result.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            wordsWOEmpty = new List<string>();

            foreach (var w in words)
            {
                if (w.Trim() != string.Empty)
                {
                    wordsWOEmpty.Add(w.Trim());
                }
            }

            words = wordsWOEmpty.ToArray();
            nextChars = new char[words.Length - 1];
            startIndex = 0;

            for (var i = 0; i < words.Length - 1; i++)
            {
                startIndex += words[i].Length;
                nextChars[i] = result[startIndex];
                startIndex++;
            }

            result = string.Empty;

            for (var i = 0; i < words.Length; i++)
            {
                if (result.Length + (i != 0 ? 1 : 0) + words[i].Length <= maxSymbols.Value)
                {
                    if (i != 0)
                    {
                        result += nextChars[i - 1];
                    }

                    result += words[i];
                }
                else
                {
                    break;
                }
            }
        }
        #endregion

        return result;
    }
}
