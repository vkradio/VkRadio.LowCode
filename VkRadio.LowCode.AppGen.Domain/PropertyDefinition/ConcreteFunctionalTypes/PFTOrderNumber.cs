using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - order number
/// </summary>
public class PFTOrderNumber : PFTInteger
{
    public PFTOrderNumber()
    {
        _defaultValue = null;
        _nullable = false;
        _quantitative = true;
        _stringCode = C_STRING_CODE;
        _unique = true;

        _defaultNames.Clear();
        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "порядковый номер");
    }

    new public const string C_STRING_CODE = "order number";
}
