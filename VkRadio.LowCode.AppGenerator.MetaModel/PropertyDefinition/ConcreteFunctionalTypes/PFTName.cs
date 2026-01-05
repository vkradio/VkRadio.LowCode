using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - name
/// </summary>
public class PFTName : PFTString
{
    public PFTName()
    {
        //_defaultValue = string.Empty;
        _defaultValue = null;
        _nullable = false;
        _quantitative = false;
        _stringCode = C_STRING_CODE;
        _unique = true;
        _defaultMaxLength = 255;
        _defaultMinLength = 1;
        _maxLength = 255;
        _minLength = 1;

        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "наименование");
    }

    public const string C_STRING_CODE = "name";
}
