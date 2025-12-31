using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VkRadio.LowCode.AppGenerator.MetaModel.Names;

/// <summary>
/// Helper methods for work with names
/// </summary>
public static class NameDictionary
{
    /// <summary>
    /// Load name dictionary from XML node
    /// </summary>
    /// <param name="xel">XML node</param>
    /// <returns>Name dictionary</returns>
    public static Dictionary<HumanLanguageEnum, string> LoadNamesFromContainingXElement(XElement xel)
    {
        var result = new Dictionary<HumanLanguageEnum,string>();

        foreach (var childXel in xel.Elements("Name"))
        {
            result.Add(StringToEnumCode(childXel.Attribute("lang")!.Value), childXel.Value);
        }

        return result;
    }

    /// <summary>
    /// Parse name literal of a natural language
    /// </summary>
    /// <param name="language">Natural language literal</param>
    /// <returns>HumanLanguageEnum value</returns>
    public static HumanLanguageEnum StringToEnumCode(string language)
    {
        if (!Enum.TryParse<HumanLanguageEnum>(language, true, out var result))
        {
            throw new ArgumentException(string.Format("Unsupported language code: {0}.", language ?? "<NULL>"));
        }

        return result;
    }

    /// <summary>
    /// List of all supported natural languages (useful for testing for an existence of words in all languages)
    /// </summary>
    public static HumanLanguageEnum[] AllHumanLanguages => Enum.GetValues<HumanLanguageEnum>();

    /// <summary>
    /// Heuristic language detection
    /// </summary>
    /// <param name="stringValue"></param>
    /// <returns></returns>
    public static HumanLanguageEnum DetectLanguage(string stringValue)
    {
        return Regex.IsMatch(stringValue, "[абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ]")
            ? HumanLanguageEnum.Ru
            : HumanLanguageEnum.En;
    }

    /// <summary>
    /// Enrich language dictionary from other dictionary.
    /// If dictionary recipient has no word in a language of a source dictionary, this word is added there.
    /// </summary>
    /// <param name="dest">Dictionary recipient (being enriched)</param>
    /// <param name="src">Dictionary source</param>
    public static void EnrichNames(IDictionary<HumanLanguageEnum, string> dest, IDictionary<HumanLanguageEnum, string> src)
    {
        foreach (var lang in AllHumanLanguages)
        {
            if (!dest.ContainsKey(lang) && src.ContainsKey(lang))
            {
                dest.Add(lang, src[lang]);
            }
        }
    }
    /// <summary>
    /// Обогащение словаря имен из другого словаря.
    /// Если в словаре-приемнике отсутствует слово на языке, на котором это же слово
    /// имеется в словаре-источнике (доноре), это слово добавляется оттуда из источника.
    /// Метод отличается от аналогичного EnrichNames тем, что словарь-приемник ожидает
    /// модификацию слов из донора так, что для русских имен будет сформировано имя вида
    /// "коллекция объектов %оригинальный объект%", а для английских - "%original object%
    /// collection".
    /// </summary>
    /// <param name="in_dest">Словарь-приемник (обогащаемый словарь)</param>
    /// <param name="in_src">Словарь-источник (донор)</param>
    public static void EnrichNamesForCollection(IDictionary<HumanLanguageEnum, string> in_dest, IDictionary<HumanLanguageEnum, string> in_src)
    {
        foreach (HumanLanguageEnum lang in AllHumanLanguages)
        {
            if (!in_dest.ContainsKey(lang) && in_src.ContainsKey(lang))
            {
                string name = in_src[lang];
                switch (lang)
                {
                    case HumanLanguageEnum.Ru:
                        name = "коллекция объектов " + name;
                        break;
                    default:
                        name += " collection";
                        break;
                }
                in_dest.Add(lang, name);
            }
        }
    }
};
