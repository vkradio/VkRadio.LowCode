using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - email
/// </summary>
public class PFTEmail : PFTString
{
    public override string StringCode => C_STRING_CODE;

    public PFTEmail(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        DefaultValue = null;
        Quantitative = false;

        DefaultMaxLength = 100;
        DefaultMinLength = 0;
        MaxLength = ReadMaxLength(xElement) ?? 100;
        MinLength = ReadMinLength(xElement) ?? 0;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "адрес электронной почты");
    }

    public const string C_STRING_CODE = "e-mail";
}
