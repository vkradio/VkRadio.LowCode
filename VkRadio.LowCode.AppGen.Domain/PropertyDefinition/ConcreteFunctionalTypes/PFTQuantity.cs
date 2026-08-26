using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

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
        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "количество");
    }

    new public const string C_STRING_CODE = "quantity";
}
