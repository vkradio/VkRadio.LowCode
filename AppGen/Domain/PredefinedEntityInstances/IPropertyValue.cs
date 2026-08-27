namespace VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

/// <summary>
/// Value of a predefined object
/// </summary>
public interface IPropertyValue
{
    /// <summary>
    /// Property definition
    /// </summary>
    PropertyDefinition.PropertyDefinition Definition { get; set; }

    /// <summary>
    /// Value, casted to an abstract object value
    /// </summary>
    object? ValueObject { get; set; }
}
