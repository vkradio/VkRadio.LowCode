using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - quantity
/// </summary>
public class PFTQuantity : PFTInteger
{
    public PFTQuantity()
    {
        //_defaultValue = 0;
        _defaultValue = null;
        _nullable = true;
        _quantitative = true;
        _stringCode = C_STRING_CODE;
        _unique = false;

        _defaultNames.Clear();
        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "количество");
    }

    new public const string C_STRING_CODE = "quantity";
}
