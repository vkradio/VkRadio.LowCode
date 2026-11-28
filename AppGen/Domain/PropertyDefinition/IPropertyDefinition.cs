using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

/// <summary>
/// Definition of a property
/// </summary>
public interface IPropertyDefinition : IUniqueNamed
{
    /// <summary>
    /// Functional property type
    /// </summary>
    PropertyFunctionalType FunctionalType { get; }

    /// <summary>
    /// Ordering attribute for objects in a list
    /// </summary>
    ListOrderEnum? ListOrder { get; }

    /// <summary>
    /// Entity type definition that owns this property
    /// </summary>
    public EntityDefinition OwnerDefinition { get; }

    void EnrichNames(IReadOnlyDictionary<NaturalLanguageEnum, string> nameSource);
}
