using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

/// <summary>
/// Definition of a property
/// </summary>
public class PropertyDefinition : IUniqueNamed, IPropertyDefinition
{
    private readonly Dictionary<NaturalLanguageEnum, string> _names;

    /// <summary>
    /// Unique identifier of a property definition
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Dictionary with property names
    /// </summary>
    public IReadOnlyDictionary<NaturalLanguageEnum, string> Names => _names.AsReadOnly();

    /// <summary>
    /// Entity type definition that owns this property
    /// </summary>
    public EntityDefinition OwnerDefinition { get; private set; }

    /// <summary>
    /// Functional type of a property
    /// </summary>
    public PropertyFunctionalType FunctionalType { get; private set; }

    /// <summary>
    /// Default value of a property
    /// </summary>
    public object? DefaultValue { get; private set; }

    /// <summary>
    /// List order
    /// </summary>
    public ListOrderEnum? ListOrder { get; private set; }

    private static ListOrderEnum ParseListOrderValue(string stringValue)
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

    public void EnrichNames(IReadOnlyDictionary<NaturalLanguageEnum, string> nameSource) => NameDictionary.EnrichNames(_names, GuardRule.Forbid.Null(nameSource, nameof(nameSource)));

    public void EnrichNamesForCollection(IReadOnlyDictionary<NaturalLanguageEnum, string> nameSource) => NameDictionary.EnrichNamesForCollection(_names, GuardRule.Forbid.Null(nameSource, nameof(nameSource)));

    /// <summary>
    /// Loading of a property definition from a containing XML node
    /// </summary>
    /// <param name="containingXel">Containing XML node</param>
    /// <param name="metaModel">MetaModel</param>
    public PropertyDefinition(XElement containingXel, DomainModel metaModel, EntityDefinition entityDefinition)
    {
        OwnerDefinition = GuardRule.Forbid.Null(entityDefinition, nameof(entityDefinition));

        // 1. Load a IUniqueNamed properties
        Id = new Guid(containingXel.Element("Id")!.Value);
        var names = NameDictionary.LoadNamesFromContainingXElement(containingXel);

        // 2. Extract a functional property type
        FunctionalType = PropertyFunctionalType.LoadFromXElement(containingXel, metaModel);

        // 3. If a property definition has no all required names, extract them from a functional type,
        //    or for reference properties - from data object type definition, on that a reference is
        //    being pointed
        NameDictionary.EnrichNames(names, FunctionalType.DefaultNames);
        _names = new Dictionary<NaturalLanguageEnum, string>(names);

        // 4. Extract a default value. If set, than get it from a property definition, otherwise inherit
        //    from a functional type
        var xelDefaultValue = containingXel.Element("DefaultValue")!;
        DefaultValue = xelDefaultValue is not null
            ? FunctionalType.ParseValueFromXmlString(xelDefaultValue.Value)
            : FunctionalType.DefaultValue;

        // 5. Is there an ordering attribute for this field?
        var xelListOrder = containingXel.Element("ListOrder");
        ListOrder = xelListOrder is not null
            ? ParseListOrderValue(xelListOrder.Value)
            : (ListOrderEnum?)null;

        // 7. Delayed linking of a property definition with a functional type
        FunctionalType.PropertyDefinition = this;
    }
}
