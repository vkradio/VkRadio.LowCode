using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

/// <summary>
/// Functional type of a property
/// </summary>
public abstract class PropertyFunctionalType
{
    protected readonly Dictionary<NaturalLanguageEnum, string> _defaultNames = [];

    /// <summary>
    /// Property type definition of a data object type or a register, that own this functional type
    /// </summary>
    public IPropertyDefinition PropertyDefinition { get; set; } = default!;

    /// <summary>
    /// Default value of a property
    /// </summary>
    public object? DefaultValue { get; protected set; }

    /// <summary>
    /// Is it possible to have missing values
    /// </summary>
    public bool Nullable { get; protected set; }

    /// <summary>
    /// Is it a qualitative type
    /// </summary>
    public bool Quantitative { get; protected set; }

    /// <summary>
    /// String code of a type
    /// </summary>
    public abstract string StringCode { get; }

    /// <summary>
    /// Is it a unique value (among other properties withing a full list of owning objects)
    /// </summary>
    public bool Unique { get; protected set; }

    /// <summary>
    /// System (.NET) type, containing a value (this value is not stored in a MetaModel, but only
    /// serves for its functioning withing a runtime environment)
    /// </summary>
    public Type? SystemType { get; protected set; }

    /// <summary>
    /// Default property names.
    /// If a property definition does not contain a name for a particular language, it is being extracted
    /// from this property. In this case a reference functional types have names, that match with names
    /// of referenced data object types.
    /// </summary>
    public IReadOnlyDictionary<NaturalLanguageEnum, string> DefaultNames => _defaultNames.AsReadOnly();

    /// <summary>
    /// Extracting property values from an XML string
    /// </summary>
    /// <param name="xmlString">XML string containing a value</param>
    /// <returns>Typed property value</returns>
    public abstract object? ParseValueFromXmlString(string xmlString);

    protected PropertyFunctionalType(bool? nullable, bool? unique, XElement xElement)
    {
        GuardRule.Forbid.Null(xElement, nameof(xElement));

        if (nullable.HasValue)
        {
            Nullable = nullable.Value;
        }

        if (unique.HasValue)
        {
            Unique = unique.Value;
        }
    }

    /// <summary>
    /// Extracting a function type of a property from an XML node that contains a property definition
    /// </summary>
    /// <param name="containingXel">XML node that contains a property definition</param>
    /// <param name="domainModel">DomainModel</param>
    /// <returns>Functional property type</returns>
    public static PropertyFunctionalType LoadFromXElement(XElement containingXel, DomainModel domainModel)
    {
        var xel = containingXel.Element("Nullable");
        var nullable = xel is not null
            ? bool.Parse(xel.Value)
            : (bool?)null;

        xel = containingXel.Element("Unique");
        var unique = xel is not null
            ? bool.Parse(xel.Value)
            : (bool?)null;

        var ftName = containingXel.Element("FunctionalType")!.Value;

        PropertyFunctionalType result = ftName switch
        {
            PFTBackReferencedTable.C_STRING_CODE => new PFTBackReferencedTable(nullable, unique, containingXel),
            PFTBoolean.C_STRING_CODE => new PFTBoolean(nullable, unique, containingXel),
            PFTConnector.C_STRING_CODE => new PFTConnector(nullable, unique, containingXel, domainModel),
            PFTDate.C_STRING_CODE => new PFTDate(nullable, unique, containingXel),
            PFTDateAndTime.C_STRING_CODE => new PFTDateAndTime(nullable, unique, containingXel),
            PFTDecimal.C_STRING_CODE => new PFTDecimal(nullable, unique, containingXel),
            PFTEmail.C_STRING_CODE => new PFTEmail(nullable, unique, containingXel),
            PFTFilePath.C_STRING_CODE => new PFTFilePath(nullable, unique, containingXel),
            PFTInteger.C_STRING_CODE => new PFTInteger(nullable, unique, containingXel),
            PFTMoney.C_STRING_CODE => new PFTMoney(nullable, unique, containingXel),
            PFTName.C_STRING_CODE => new PFTName(nullable, unique, containingXel),
            PFTOrderNumber.C_STRING_CODE => new PFTOrderNumber(nullable, unique, containingXel),
            PFTPassword.C_STRING_CODE => new PFTPassword(nullable, unique, containingXel),
            PFTPrice.C_STRING_CODE => new PFTPrice(nullable, unique, containingXel),
            PFTQuantity.C_STRING_CODE => new PFTQuantity(nullable, unique, containingXel),
            PFTReferenceValue.C_STRING_CODE => new PFTReferenceValue(nullable, unique, containingXel, domainModel),
            PFTShortText.C_STRING_CODE => new PFTShortText(nullable, unique, containingXel),
            PFTTableOwner.C_STRING_CODE => new PFTTableOwner(nullable, unique, containingXel, domainModel),
            PFTTablePart.C_STRING_CODE => new PFTTablePart(nullable, unique, containingXel),
            PFTText.C_STRING_CODE => new PFTText(nullable, unique, containingXel),
            PFTTime.C_STRING_CODE => new PFTTime(nullable, unique, containingXel),
            PFTUniqueCode.C_STRING_CODE => new PFTUniqueCode(nullable, unique, containingXel),
            _ => throw new ApplicationException(string.Format("Property functional type not supported: {0}.", ftName ?? "<NULL>")),
        };

        return result;
    }

    /// <summary>
    /// Creating a typed prefab for storing a value
    /// </summary>
    /// <returns>Prefab for a property value</returns>
    public abstract IPropertyValue CreatePropertyValue();
}
