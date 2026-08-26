using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

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
        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "текст");
    }

    public const string C_STRING_CODE = "text";
}
