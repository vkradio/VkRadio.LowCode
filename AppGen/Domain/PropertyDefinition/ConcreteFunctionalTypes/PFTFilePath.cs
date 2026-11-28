using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - path to a file resource
/// </summary>
public class PFTFilePath : PFTString
{
    public override string StringCode => C_STRING_CODE;

    public PFTFilePath(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        DefaultValue = null;
        Quantitative = false;
        
        DefaultMaxLength = ReadDefaultMaxLength(xElement) ?? 255;
        DefaultMinLength = ReadDefaultMinLength(xElement) ?? 0;
        MaxLength = ReadMaxLength(xElement) ?? 255;
        MinLength = ReadMinLength(xElement) ?? 0;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "путь к файлу");
    }

    public const string C_STRING_CODE = "file path";
}
