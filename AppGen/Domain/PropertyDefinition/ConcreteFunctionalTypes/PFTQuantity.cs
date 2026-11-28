using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - quantity
/// </summary>
public class PFTQuantity : PFTInteger
{
    public override string StringCode => C_STRING_CODE;

    public PFTQuantity(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        //DefaultValue = 0;
        DefaultValue = null;
        Quantitative = true;

        _defaultNames.Clear();
        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "количество");
    }

    new public const string C_STRING_CODE = "quantity";
}
