using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - email
/// </summary>
public class PFTEmail : PFTString
{
    public PFTEmail()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _stringCode = C_STRING_CODE;
        _unique = false;
        _defaultMaxLength = 100;
        _defaultMinLength = 0;
        _maxLength = 100;
        _minLength = 0;

        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "адрес электронной почты");
    }

    public const string C_STRING_CODE = "e-mail";
}
