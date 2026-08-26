using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - price
/// </summary>
public class PFTPrice : PFTMoney
{
    public PFTPrice()
    {
        _stringCode = C_STRING_CODE;

        _defaultNames.Clear();
        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "цена");
    }

    new public const string C_STRING_CODE = "price";
}
