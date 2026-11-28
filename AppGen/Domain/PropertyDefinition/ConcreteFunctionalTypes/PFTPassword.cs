using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - password
/// </summary>
public class PFTPassword : PFTString
{
    public override string StringCode => C_STRING_CODE;

    public PFTPassword(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? false, unique ?? false, xElement)
    {
        DefaultValue = null;
        Quantitative = false;
        
        DefaultMaxLength = ReadDefaultMaxLength(xElement) ?? 255;
        DefaultMinLength = ReadDefaultMinLength(xElement) ?? 8;
        MaxLength = ReadMaxLength(xElement) ?? 255;
        MinLength = ReadMinLength(xElement) ?? 8;

        var xel = xElement.Element("StoreAs");

        if (xel is not null)
        {
            var strStoreAs = xel.Value;

            StoreAs = strStoreAs switch
            {
                "open" => PasswordStoreAs.Open,

                "md5" => PasswordStoreAs.Md5,

                _ => throw new ApplicationException(string.Format("Unsupported StoreAs code for password property Id {0}: \"{1}\".", xElement.Element("Id")!.Value, strStoreAs)),
            };
        }
        else
        {
            StoreAs = PasswordStoreAs.Md5;
        }

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "пароль");
    }

    public const string C_STRING_CODE = "password";

    /// <summary>
    /// Storage method in a dababase
    /// </summary>
    public PasswordStoreAs StoreAs { get; private set; }
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
