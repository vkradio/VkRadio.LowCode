using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - price
/// </summary>
public class PFTPrice : PFTMoney
{
    public PFTPrice()
    {
        _stringCode = C_STRING_CODE;

        _defaultNames.Clear();
        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "цена");
    }

    new public const string C_STRING_CODE = "price";
}
