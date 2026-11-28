using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - price
/// </summary>
public class PFTPrice : PFTMoney
{
    public override string StringCode => C_STRING_CODE;

    public PFTPrice(bool? nullable, bool? unique, XElement xElement)
        : base(nullable, unique, xElement)
    {
        _defaultNames.Clear();
        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "цена");
    }

    new public const string C_STRING_CODE = "price";
}
