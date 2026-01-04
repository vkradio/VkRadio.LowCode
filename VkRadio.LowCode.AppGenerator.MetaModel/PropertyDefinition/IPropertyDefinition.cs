using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;

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
    ListOrderEnum? ListOrder { get; set; }
}
