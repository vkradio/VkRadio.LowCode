using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - path to a file resource
/// </summary>
public class PFTFilePath : PFTString
{
    public PFTFilePath()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _stringCode = C_STRING_CODE;
        _unique = false;
        _defaultMaxLength = 255;
        _defaultMinLength = 0;
        _maxLength = 255;
        _minLength = 0;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "путь к файлу");
    }

    public const string C_STRING_CODE = "file path";
}
