using System.Xml.Linq;

using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;

/// <summary>
/// Definition of a property
/// </summary>
public class PropertyDefinition : IUniqueNamed, IPropertyDefinition
{
    Guid _id;
    Dictionary<HumanLanguageEnum, string> _names;
    DOTDefinition.DOTDefinition _ownerDefinition;
    PropertyFunctionalType _functionalType;
    object _defaultValue;

    /// <summary>
    /// Unique identifier of a property definition
    /// </summary>
    public Guid Id { get { return _id; } set { _id = value; } }
    /// <summary>
    /// Dictionary with property names
    /// </summary>
    public IDictionary<HumanLanguageEnum, string> Names { get { return _names; } }
    /// <summary>
    /// Data object type definition that owns this property
    /// </summary>
    public DOTDefinition.DOTDefinition OwnerDefinition { get { return _ownerDefinition; } set { _ownerDefinition = value; } }
    /// <summary>
    /// Functional type of a property
    /// </summary>
    public PropertyFunctionalType FunctionalType { get { return _functionalType; } }
    /// <summary>
    /// Default value of a property
    /// </summary>
    public object? DefaultValue { get { return _defaultValue; } }
    /// <summary>
    /// List order
    /// </summary>
    public ListOrderEnum? ListOrder { get; set; }

    static ListOrderEnum ParseListOrderValue(string stringValue)
    {
        if (string.IsNullOrEmpty(stringValue))
        {
            return ListOrderEnum.Default;
        }

        return stringValue switch
        {
            "asc" => ListOrderEnum.Asc,
            "desc" => ListOrderEnum.Desc,
            _ => throw new FormatException(string.Format("Invalid value of ListOrder: \"{0}\" (allowed values: \"asc\", \"desc\" or empty).", stringValue ?? "<NULL>")),
        };
    }

    /// <summary>
    /// Loading of a property definition from a containing XML node
    /// </summary>
    /// <param name="containingXel">Containing XML node</param>
    /// <param name="metaModel">MetaModel</param>
    /// <returns>Property definition</returns>
    public static PropertyDefinition LoadFromXElement(XElement containingXel, MetaModel metaModel)
    {
        // 1. Load a IUniqueNamed properties
        var id = new Guid(containingXel.Element("Id")!.Value);
        var names = NameDictionary.LoadNamesFromContainingXElement(containingXel);

        // 2. Extract a functional property type
        var ft = PropertyFunctionalType.LoadFromXElement(containingXel, metaModel);

        // 3. If a property definition has no all required names, extract them from a functional type,
        //    or for reference properties - from data object type definition, on that a reference is
        //    being pointed
        NameDictionary.EnrichNames(names, ft.DefaultNames);

        // 4. Extract a default value. If set, than get it from a property definition, otherwise inherit
        //    from a functional type
        var xelDefaultValue = containingXel.Element("DefaultValue")!;
        var defaultValue = xelDefaultValue is not null
            ? ft.ParseValueFromXmlString(xelDefaultValue.Value)
            : ft.DefaultValue;

        // 5. Is there an ordering attribute for this field?
        var xelListOrder = containingXel.Element("ListOrder");
        var listOrder = xelListOrder is not null
            ? ParseListOrderValue(xelListOrder.Value)
            : (ListOrderEnum?)null;

        // 6. Create a property definition
        var pd = new PropertyDefinition()
        {
            _id = id,
            _names = names,
            _functionalType = ft,
            _defaultValue = defaultValue,
            ListOrder = listOrder
        };

        // 7. Delayed linking of a property definition with a functional type
        pd.FunctionalType.PropertyDefinition = pd;

        return pd;
    }
}
