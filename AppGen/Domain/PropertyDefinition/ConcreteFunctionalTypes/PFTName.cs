using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - name
/// </summary>
public class PFTName : PFTString
{
    public override string StringCode => C_STRING_CODE;

    public PFTName(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? false, unique ?? true, xElement)
    {
        //DefaultValue = string.Empty;
        DefaultValue = null;
        Quantitative = false;
        
        DefaultMaxLength = ReadDefaultMaxLength(xElement) ?? 255;
        DefaultMinLength = ReadDefaultMinLength(xElement) ?? 1;
        MaxLength = ReadMaxLength(xElement) ?? 255;
        MinLength = ReadMinLength(xElement) ?? 1;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "наименование");
    }

    public const string C_STRING_CODE = "name";
}
