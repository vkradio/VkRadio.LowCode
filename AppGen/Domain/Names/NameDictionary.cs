using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.Domain.Names;

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
    public static Dictionary<NaturalLanguageEnum, string> LoadNamesFromContainingXElement(XElement xel)
    {
        var result = new Dictionary<NaturalLanguageEnum,string>();

        foreach (var childXel in xel.Elements("Name"))
        {
            result.Add(StringToEnumCode(childXel.Attribute("lang")!.Value), childXel.Value);
        }

        return result;
    }

    /// <summary>
    /// Parse the name literal of a natural language
    /// </summary>
    /// <param name="language">Natural language literal</param>
    /// <returns>NaturalLanguageEnum value</returns>
    public static NaturalLanguageEnum StringToEnumCode(string language)
    {
        if (!Enum.TryParse<NaturalLanguageEnum>(language, true, out var result))
        {
            throw new ArgumentException(string.Format("Unsupported language code: {0}.", language ?? "<NULL>"));
        }

        return result;
    }

    /// <summary>
    /// List of all supported natural languages (useful for testing for an existence of words in all languages)
    /// </summary>
    public static NaturalLanguageEnum[] AllHumanLanguages => Enum.GetValues<NaturalLanguageEnum>();

    /// <summary>
    /// Heuristic language detection
    /// </summary>
    /// <param name="stringValue"></param>
    /// <returns></returns>
    public static NaturalLanguageEnum DetectLanguage(string stringValue)
    {
        return Regex.IsMatch(stringValue, "[абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ]")
            ? NaturalLanguageEnum.Ru
            : NaturalLanguageEnum.En;
    }

    /// <summary>
    /// Enrich the language dictionary from other dictionary.
    /// If a dictionary recipient has no word in a language of a source dictionary, this word is added there.
    /// </summary>
    /// <param name="dest">Dictionary recipient (being enriched)</param>
    /// <param name="src">Dictionary source</param>
    public static void EnrichNames(IDictionary<NaturalLanguageEnum, string> dest, IDictionary<NaturalLanguageEnum, string> src)
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
    /// Enrich a name dictionary from other dictionary.
    /// If a dictionary recipient has no word in a language of a source dictionary, this word is added there.
    /// The method differs from the similar EnrichNames in that it expects the added word will be modified,
    /// so for Russian names it will be formatted like &quot;коллекция объектов {оригинальный объект}&quot;, and
    /// for English names - like &quot;{original object} collection&quot;.
    /// </summary>
    /// <param name="dest">Destination dictionary (being enriched)</param>
    /// <param name="src">Source dictionary</param>
    public static void EnrichNamesForCollection(IDictionary<NaturalLanguageEnum, string> dest, IDictionary<NaturalLanguageEnum, string> src)
    {
        foreach (var lang in AllHumanLanguages)
        {
            if (!dest.ContainsKey(lang) && src.ContainsKey(lang))
            {
                var name = src[lang];

                switch (lang)
                {
                    case NaturalLanguageEnum.Ru:
                        name = "коллекция объектов " + name;
                        break;

                    default:
                        name += " collection";
                        break;
                }

                dest.Add(lang, name);
            }
        }
    }
}
