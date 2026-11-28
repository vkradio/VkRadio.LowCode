using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - order number
/// </summary>
public class PFTOrderNumber : PFTInteger
{
    public override string StringCode => C_STRING_CODE;

    public PFTOrderNumber(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? false, unique ?? true, xElement)
    {
        DefaultValue = null;
        Quantitative = true;

        _defaultNames.Clear();
        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "порядковый номер");
    }

    new public const string C_STRING_CODE = "order number";
}
