using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - text
/// </summary>
public class PFTText : PFTString
{
    public PFTText()
    {
        //_defaultValue = string.Empty;
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _stringCode = C_STRING_CODE;
        _unique = false;
        _defaultMaxLength = 8000;
        _defaultMinLength = 0;
        _maxLength = 8000;
        _minLength = 0;

        _defaultNames.Clear();
        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "текст");
    }

    public const string C_STRING_CODE = "text";
}
