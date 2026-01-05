using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - password
/// </summary>
public class PFTPassword : PFTString
{
    PasswordStoreAs _storeAs;

    public PFTPassword()
    {
        _defaultValue = null;
        _nullable = false;
        _quantitative = false;
        _stringCode = C_STRING_CODE;
        _unique = false;
        _defaultMaxLength = 255;
        _defaultMinLength = 8;
        _maxLength = 255;
        _minLength = 8;
        _storeAs = PasswordStoreAs.Md5;

        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "пароль");
    }

    public const string C_STRING_CODE = "password";

    /// <summary>
    /// Storage method in a dababase
    /// </summary>
    public PasswordStoreAs StoreAs { get { return _storeAs; } set { _storeAs = value; } }

    protected override void InitExtendedParams(XElement xelPropertyDefinition)
    {
        base.InitExtendedParams(xelPropertyDefinition);

        var xel = xelPropertyDefinition.Element("StoreAs");

        if (xel is not null)
        {
            var strStoreAs = xel.Value;

            _storeAs = strStoreAs switch
            {
                "open" => PasswordStoreAs.Open,
                "md5" => PasswordStoreAs.Md5,
                _ => throw new ApplicationException(string.Format("Unsupported StoreAs code for password property Id {0}: \"{1}\".", xelPropertyDefinition.Element("Id")!.Value, strStoreAs)),
            };
        }
    }
}

/// <summary>
/// Storage method in a database
/// </summary>
public enum PasswordStoreAs
{
    /// <summary>
    /// Open store as a plain string
    /// </summary>
    Open,
    // MD5 hash
    Md5
}
