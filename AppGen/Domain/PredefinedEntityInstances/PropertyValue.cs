namespace VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

/// <summary>
/// Value of a property
/// </summary>
/// <typeparam name="T">System (.NET) type of a value</typeparam>
public class PropertyValue<T> : IPropertyValue
{
    PropertyDefinition.PropertyDefinition _definition;
    T? _value;

    /// <summary>
    /// Definition of a property
    /// </summary>
    public PropertyDefinition.PropertyDefinition Definition { get => _definition; set => _definition = value; }

    /// <summary>
    /// Typed value of a property
    /// </summary>
    public T? Value => _value;

    /// <summary>
    /// Value of property being returned in a form of an abstract object
    /// </summary>
    public object? ValueObject { get => _value; set => _value = (T?)value; }
}
