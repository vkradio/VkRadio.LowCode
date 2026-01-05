using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

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
        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "порядковый номер");
    }

    new public const string C_STRING_CODE = "order number";
}
