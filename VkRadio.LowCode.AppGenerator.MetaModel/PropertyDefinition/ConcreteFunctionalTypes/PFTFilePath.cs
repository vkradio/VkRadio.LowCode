using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

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

        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "путь к файлу");
    }

    public const string C_STRING_CODE = "file path";
}
