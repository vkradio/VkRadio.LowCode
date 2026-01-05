using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - time
/// </summary>
public class PFTTime: PFTDateTime
{
    public PFTTime()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _stringCode = C_STRING_CODE;
        _unique = false;

        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "время");
    }

    public const string C_STRING_CODE = "time";
}
