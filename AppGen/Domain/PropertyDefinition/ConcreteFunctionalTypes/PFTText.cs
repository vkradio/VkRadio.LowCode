using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - text
/// </summary>
public class PFTText : PFTString
{
    public override string StringCode => C_STRING_CODE;

    public PFTText(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        //DefaultValue = string.Empty;
        DefaultValue = null;
        Quantitative = false;
        
        DefaultMaxLength = ReadDefaultMaxLength(xElement) ?? 8000;
        DefaultMinLength = ReadDefaultMinLength(xElement) ?? 0;
        MaxLength = ReadMaxLength(xElement) ?? 8000;
        MinLength = ReadMinLength(xElement) ?? 0;

        _defaultNames.Clear();
        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "текст");
    }

    public const string C_STRING_CODE = "text";
}
