using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - short string of text
/// </summary>
public class PFTShortText : PFTString
{
    public override string StringCode => C_STRING_CODE;

    public PFTShortText(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        DefaultValue = null;
        Quantitative = false;
        
        DefaultMaxLength = ReadDefaultMaxLength(xElement) ?? 100;
        DefaultMinLength = ReadDefaultMinLength(xElement) ?? 0;
        MaxLength = ReadMaxLength(xElement) ?? 100;
        MinLength = ReadMinLength(xElement) ?? 0;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "короткий текст");
    }

    public const string C_STRING_CODE = "short text";
}
