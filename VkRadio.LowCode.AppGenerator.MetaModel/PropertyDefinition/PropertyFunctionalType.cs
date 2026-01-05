using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;

/// <summary>
/// Functional type of a property
/// </summary>
public abstract class PropertyFunctionalType
{
    protected IPropertyDefinition _propertyDefinition;
    protected object? _defaultValue;
    protected bool _nullable;
    protected bool _quantitative;
    protected string _stringCode;
    protected bool _unique;
    protected Type _systemType;
    protected Dictionary<HumanLanguageEnum, string> _defaultNames = new();

    /// <summary>
    /// Property type definition of a data object type or a register, that own this functional type
    /// </summary>
    public IPropertyDefinition PropertyDefinition { get { return _propertyDefinition; } set { _propertyDefinition = value; } }
    /// <summary>
    /// Default value of a property
    /// </summary>
    public object? DefaultValue { get { return _defaultValue; } set { _defaultValue = value; } }
    /// <summary>
    /// Is it possible to have missing values
    /// </summary>
    public bool Nullable { get { return _nullable; } set { _nullable = value; } }
    /// <summary>
    /// Is it a qualitative type
    /// </summary>
    public bool Quantitative { get { return _quantitative; } }
    /// <summary>
    /// String code of a type
    /// </summary>
    public string StringCode { get { return _stringCode; } }
    /// <summary>
    /// Is it a unique value (among other properties withing a full list of owning objects)
    /// </summary>
    public bool Unique { get { return _unique; } set { _unique = value; } }
    /// <summary>
    /// System (.NET) type, containing a value (this value is not stored in a MetaModel, but only
    /// serves for its functioning withing a runtime environment)
    /// </summary>
    public Type SystemType { get { return _systemType; } }
    /// <summary>
    /// Default property names.
    /// If a property definition does not contain a name for a particular language, it is being extracted
    /// from this property. In this case a reference functional types have names, that match with names
    /// of referenced data object types.
    /// </summary>
    public IDictionary<HumanLanguageEnum, string> DefaultNames { get { return _defaultNames; } }

    /// <summary>
    /// Extracting property values from an XML string
    /// </summary>
    /// <param name="xmlString">XML string containing a value</param>
    /// <returns>Typed property value</returns>
    public abstract object? ParseValueFromXmlString(string xmlString);

    /// <summary>
    /// Initializing of an extended set of parameters for a functional type
    /// </summary>
    /// <param name="xelPropertyDefinition">XML node containing a property definition</param>
    protected virtual void InitExtendedParams(XElement xelPropertyDefinition)
    {
    }

    /// <summary>
    /// Extracting a function type of a property from an XML node that contains a property definition
    /// </summary>
    /// <param name="containingXel">XML node that contains a property definition</param>
    /// <param name="metaModel">MetaModel</param>
    /// <returns>Functional property type</returns>
    public static PropertyFunctionalType LoadFromXElement(XElement containingXel, MetaModel metaModel)
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
            PFTBackReferencedTable.C_STRING_CODE => new PFTBackReferencedTable(),
            PFTBoolean.C_STRING_CODE => new PFTBoolean(),
            PFTConnector.C_STRING_CODE => new PFTConnector(),
            PFTDate.C_STRING_CODE => new PFTDate(),
            PFTDateAndTime.C_STRING_CODE => new PFTDateAndTime(),
            PFTDecimal.C_STRING_CODE => new PFTDecimal(),
            PFTEmail.C_STRING_CODE => new PFTEmail(),
            PFTFilePath.C_STRING_CODE => new PFTFilePath(),
            PFTInteger.C_STRING_CODE => new PFTInteger(),
            PFTMoney.C_STRING_CODE => new PFTMoney(),
            PFTName.C_STRING_CODE => new PFTName(),
            PFTOrderNumber.C_STRING_CODE => new PFTOrderNumber(),
            PFTPassword.C_STRING_CODE => new PFTPassword(),
            PFTPrice.C_STRING_CODE => new PFTPrice(),
            PFTQuantity.C_STRING_CODE => new PFTQuantity(),
            PFTReferenceValue.C_STRING_CODE => new PFTReferenceValue(),
            PFTShortText.C_STRING_CODE => new PFTShortText(),
            PFTTableOwner.C_STRING_CODE => new PFTTableOwner(),
            PFTTablePart.C_STRING_CODE => new PFTTablePart(),
            PFTText.C_STRING_CODE => new PFTText(),
            PFTTime.C_STRING_CODE => new PFTTime(),
            PFTUniqueCode.C_STRING_CODE => new PFTUniqueCode(),
            _ => throw new ApplicationException(string.Format("Property functional type not supported: {0}.", ftName ?? "<NULL>")),
        };

        result.Nullable = nullable ?? false;
        result.Unique = unique ?? false;

        var dependentLink = result as IPFTDependentLink;

        if (dependentLink is not null)
        {
            xel = containingXel.Element("OnRefObjectDelete");

            if (xel is not null)
            {
                dependentLink.OnDeleteAction = xel.Value switch
                {
                    "ignore" => OnDeleteActionEnum.Ingnore,
                    "delete" => OnDeleteActionEnum.Delete,
                    "block" => OnDeleteActionEnum.CannotDelete,
                    "set default value" => OnDeleteActionEnum.ResetToDefault,
                    "set null" => OnDeleteActionEnum.ResetToNull,

                    _ => throw new ApplicationException(string.Format(
                        "Property {0} has unsupported OnRefObjectDelete value: {1}.",
                        containingXel.Element("Id") is not null
                            ? containingXel.Element("Id")!.Value
                            : "<NULL>",
                        xel.Value ?? "<NULL>"
                    )),
                };
            }
            else
            {
                if (metaModel.DefaultLinksStrict)
                {
                    if (result.Nullable)
                    {
                        dependentLink.OnDeleteAction = OnDeleteActionEnum.ResetToNull;
                    }
                    else
                    {
                        dependentLink.SetDefaultOnDeleteAction();
                    }
                }
            }
        }

        result.InitExtendedParams(containingXel);

        return result;
    }

    /// <summary>
    /// Creating a typed prefab for storing a value
    /// </summary>
    /// <returns>Prefab for a property value</returns>
    public abstract IPropertyValue CreatePropertyValue();
}
