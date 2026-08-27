using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Boolean functional property type
/// </summary>
public class PFTBoolean : PropertyFunctionalType
{
    public PFTBoolean()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _stringCode = C_STRING_CODE;
        _unique = false;
        _systemType = typeof(bool);

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "булево");
    }

    /// <summary>
    /// String code of a functional property type (used in MetaModel files)
    /// </summary>
    public const string C_STRING_CODE = "boolean";

    /// <summary>
    /// Parsing a value from an XML string
    /// </summary>
    /// <param name="xmlString">XML string containing a value</param>
    /// <returns>Typed value of a property</returns>
    public override object? ParseValueFromXmlString(string xmlString) { return bool.Parse(xmlString); }

    /// <summary>
    /// Creating of a typed prefab for storing a value
    /// </summary>
    /// <returns>Prefab for a property value</returns>
    public override IPropertyValue CreatePropertyValue() => new PropertyValue<bool?>
    {
        Definition = (PropertyDefinition)_propertyDefinition
    };
}
