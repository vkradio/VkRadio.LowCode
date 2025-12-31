using MetaModel.Names;
using MetaModel.Names.Translit.RU;
using System;
using System.Collections.Generic;

namespace VkRadio.LowCode.AppGenerator;

public class NameHelper
{
    static bool LastLetterIsSLike(string in_word)
    {
        char lastLetter = in_word[in_word.Length - 1];
        char prevLastLetter = in_word[in_word.Length - 2];

        return
            lastLetter == 's' ||
            (prevLastLetter == 's' && lastLetter == 'h') ||
            (prevLastLetter == 'c' && lastLetter == 'h');
    }

    public static string[] GetNameWords(string in_name)
    {
        string name = in_name
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
        return name.Split(new char[] { ' ', '-', '/' });
    }
    public static string[] GetNameWords(IDictionary<HumanLanguageEnum, string> in_names)
    {
        return GetNameWords(in_names[HumanLanguageEnum.En]);
    }

    /// <summary>
    /// Получение имени объекта в венгерской нотации (слова пишутся слитно, но
    /// выделяются заглавными буквами, при этом первое слово также опционально пишется
    /// с заглавной буквы)
    /// </summary>
    /// <param name="in_names">Пакет имен объекта на разных языках</param>
    /// <param name="in_firstLetterUpperCase">Делать ли заглавной первую букву имени</param>
    /// <returns>Имя объекта в венгерской нотации</returns>
    public static string NamesToHungarianName(IDictionary<HumanLanguageEnum, string> in_names, bool in_firstLetterUpperCase)
    {
        string[] words = GetNameWords(in_names);
        string name = string.Empty;
        for (int i = 0; i < words.Length; i++)
        {
            string nextWord = words[i].ToLower();

            if (i != 0 || in_firstLetterUpperCase)
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
    /// <summary>
    /// Получение имени объекта в венгерской нотации (слова пишутся слитно, но
    /// каждое из них пишется с заглавной буквы)
    /// </summary>
    /// <param name="in_names">Пакет имен объекта на разных языках</param>
    /// <returns>Имя объекта в венгерской нотации</returns>
    public static string NamesToHungarianName(IDictionary<HumanLanguageEnum, string> in_names)
    {
        return NamesToHungarianName(in_names, true);
    }

    public static string NamesToCamelCase(IDictionary<HumanLanguageEnum, string> in_names)
    {
        return NamesToHungarianName(in_names, false);
    }
    public static string NamesToCamelCasePlural(IDictionary<HumanLanguageEnum, string> in_names)
    {
        string result = NamesToCamelCase(in_names);

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
    /// Если ли следующие непустые слова
    /// </summary>
    /// <param name="in_words">Слова</param>
    /// <param name="in_thisIndex">Индекс (начиная с нуля) текущего слова, после которого ищутся непустые слова</param>
    /// <returns></returns>
    static bool NextWordsAreNotEmpty(string[] in_words, int in_thisIndex)
    {
        bool result = false;

        for (int i = in_thisIndex + 1; i < in_words.Length; i++)
        {
            if (in_words[i] != string.Empty)
            {
                result = true;
                break;
            }
        }

        return result;
    }
    /// <summary>
    /// Получение имени, слова в котором разделены знаком подчеркивания (_), из имени ТОД
    /// (определения его самого, его свойства и т.д.)
    /// </summary>
    /// <param name="in_dotName">Имя, которое берется за основу</param>
    public static string NameToUnderscoreSeparatedName(string in_dotName)
    {
        string[] words = GetNameWords(in_dotName);
        string name = string.Empty;
        for (int i = 0; i < words.Length; i++)
        {
            string nextWord = words[i].ToLower();

            if (nextWord != string.Empty)
            {
                name += nextWord;
                if (NextWordsAreNotEmpty(words, i))
                    name += "_";
            }
        }
        return name;
    }
    /// <summary>
    /// Получение имени, слова в котором разделены знаком подчеркивания (_), из имени ТОД
    /// (определения его самого, его свойства и т.д.)
    /// </summary>
    /// <param name="in_dotName">Имя, которое берется за основу</param>
    public static string NameToUnderscoreSeparatedName(IDictionary<HumanLanguageEnum, string> in_names)
    {
        return NameToUnderscoreSeparatedName(in_names[HumanLanguageEnum.En]);
    }

    /// <summary>
    /// Получение имени константы Id из имен объекта. Приоритет отдается англоязычному имени,
    /// но если его нет, транслитерируется локальное имя.
    /// </summary>
    /// <param name="in_names">Пакет имен объекта</param>
    /// <returns>Имя константы вида C_ID_WORD1_WORD2_WORDN</returns>
    public static string NameToConstantId(IDictionary<HumanLanguageEnum, string> in_names)
    {
        return NameToConstant(in_names, true, true);
    }
    /// <summary>
    /// Получение имени константы из имен объекта. Приоритет отдается англоязычному имени,
    /// но если его нет, транслитерируется локальное имя.
    /// </summary>
    /// <param name="in_names">Пакет имен объекта</param>
    /// <param name="in_idConstant">Является ли константа Id объекта (false по умолчанию)</param>
    /// <returns>Имя константы вида C_[ID_]WORD1_WORD2_WORDN</returns>
    public static string NameToConstant(IDictionary<HumanLanguageEnum, string> in_names, bool in_addCPrefix = true, bool in_idConstant = false)
    {
        string name = in_names.ContainsKey(HumanLanguageEnum.En) ?
            in_names[HumanLanguageEnum.En] :
            Transliteration.Front(in_names[HumanLanguageEnum.Ru]);

        string[] words = GetNameWords(name);
        string result = string.Empty;
        foreach (string word in words)
        {
            if (result != string.Empty)
                result += "_";
            result += word.ToUpper();
        }
        return string.Format("{0}{1}{2}", in_addCPrefix ? "C_" : string.Empty, in_idConstant ? "ID_" : string.Empty, result);
    }

    /// <summary>
    /// Формирование имени на локальном языке с заглавной буквы
    /// </summary>
    /// <param name="in_names">Словарь имен</param>
    /// <returns>Имя на локальном языке с заглавной буквы</returns>
    public static string GetLocalNameUpperCase(IDictionary<HumanLanguageEnum, string> in_names)
    {
        string localName = GetLocalNameLowerCase(in_names);
        if (localName.Length > 0)
            localName = char.ToUpper(localName[0]) + (localName.Length > 1 ? localName.Substring(1) : string.Empty);
        return localName;
    }
    /// <summary>
    /// Формирование имени на локальном языке с маленькой буквы
    /// </summary>
    /// <param name="in_names">Словарь имен</param>
    /// <returns>Имя на локальном языке с маленькой буквы</returns>
    public static string GetLocalNameLowerCase(IDictionary<HumanLanguageEnum, string> in_names)
    {
        string localName = in_names.ContainsKey(HumanLanguageEnum.Ru) ?
            in_names[HumanLanguageEnum.Ru] :
            in_names[HumanLanguageEnum.En];
        return localName;
    }

    /// <summary>
    /// "Забивание" строки эскейпами так, чтобы они представлялись нормальныим литералами C#
    /// </summary>
    /// <param name="in_string"></param>
    /// <returns></returns>
    public static string GetStringSuitableToCSharp(string in_string)
    {
        return in_string.Replace("\"", "\\\"");
    }
    /// <summary>
    /// "Забивание" строки эскейпами так, чтобы она представлялась нормальной текстовой
    /// строкой, пригодной для размещения между тегами XML.
    /// </summary>
    /// <param name="in_string"></param>
    /// <returns></returns>
    public static string GetStringSuitableToXmlText(string in_string)
    {
        return in_string
            .Replace("&", "&amp;")
            .Replace("\"", "&quot;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");
    }

    /// <summary>
    /// Если строка начинается с цифры (в общем случае в будущем должен поддерживаться
    /// любой недопустимый в начале наименований символ), в ее начало добавляется
    /// большая или маленькая N (в зависимости от значения in_bigN)
    /// </summary>
    /// <param name="in_original">Изначальное значение строки</param>
    /// <param name="in_bigN">Если true, то добавляемая N большая, иначе маленькая</param>
    /// <returns></returns>
    public static string AddBeginningNIfNeeded(string in_original, bool in_bigN = true)
    {
        char[] invalidChars = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        if (string.IsNullOrWhiteSpace(in_original))
        {
            return in_original;
        }
        else
        {
            foreach (char ch in invalidChars)
            {
                if (ch == in_original[0])
                {
                    string addN = in_bigN ? "N" : "n";
                    return addN + in_original;
                }
            }
            return in_original;
        }
    }
}

public static class NameHelperExtension
{
    static readonly char[] c_consonants = { 'б', 'в', 'г', 'д', 'ж', 'з', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ' };

    /// <summary>
    /// Сокращение наименования
    /// </summary>
    /// <param name="in_this">Полное наименование</param>
    /// <param name="in_maxSymbols">Опция: Максимальное количество символов</param>
    /// <returns></returns>
    public static string Shorten(this string in_this, int? in_maxSymbols = null)
    {
        char[] splitChars = new char[] { ' ', '-', '.', ',', '(', ')', '?', '!', ':', ';' };

        #region Сначала только сокращаем слова, но не удаляем их.
        string result = string.Empty;
        string[] words = in_this.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
        List<string> wordsWOEmpty = new List<string>();
        foreach (string w in words)
        {
            if (w.Trim() != string.Empty)
                wordsWOEmpty.Add(w.Trim());
        }
        words = wordsWOEmpty.ToArray();
        char[] nextChars = new char[words.Length - 1];
        int startIndex = 0;
        for (int i = 0; i < words.Length - 1; i++)
        {
            startIndex += words[i].Length;
            nextChars[i] = in_this[startIndex];
            startIndex++;
        }
        bool shortened;
        for (int i = 0; i < words.Length; i++)
        {
            shortened = false;
            words[i] = words[i].Trim();
            if (words[i].Length > 3)
            {
                for (int j = 2; j < words[i].Length - 2; j++)
                {
                    char thisChar = words[i][j].ToString().ToLower()[0];
                    bool isConsonant = false;
                    foreach (char consChar in c_consonants)
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
                    result += nextChars[i];
            }
            else
            {
                if (i != words.Length - 1)
                    result += nextChars[i];
            }
        }
        #endregion

        #region Если задано максимальное количество символов и результат его превышает, пытаемся удалить слова с конца.
        if (in_maxSymbols.HasValue && result.Length > in_maxSymbols.Value)
        {
            words = result.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            wordsWOEmpty = new List<string>();
            foreach (string w in words)
            {
                if (w.Trim() != string.Empty)
                    wordsWOEmpty.Add(w.Trim());
            }
            words = wordsWOEmpty.ToArray();
            nextChars = new char[words.Length - 1];
            startIndex = 0;
            for (int i = 0; i < words.Length - 1; i++)
            {
                startIndex += words[i].Length;
                nextChars[i] = result[startIndex];
                startIndex++;
            }
            result = string.Empty;
            for (int i = 0; i < words.Length; i++)
            {
                if (result.Length + (i != 0 ? 1 : 0) + words[i].Length <= in_maxSymbols.Value)
                {
                    if (i != 0)
                        result += nextChars[i - 1];
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
