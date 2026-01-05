using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - date
/// </summary>
public class PFTDate : PFTDateTime
{
    /// <summary>
    /// Constructor
    /// </summary>
    public PFTDate()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _stringCode = C_STRING_CODE;
        _unique = false;

        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "дата");
    }

    /// <summary>
    /// String code of a functional property type (used in MetaModel files)
    /// </summary>
    public const string C_STRING_CODE = "date";
}
