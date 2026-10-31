using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

namespace VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

/// <summary>
/// Value of a property
/// </summary>
/// <typeparam name="T">System (.NET) type of a value</typeparam>
public class PropertyValue<T> : IPropertyValue
{
    private T? _value;

    public PropertyValue(IPropertyDefinition propDefinition)
    {
        Guard.Against.Null(propDefinition, nameof(propDefinition));
        Definition = (PropertyDefinition.PropertyDefinition)propDefinition;
    }

    /// <summary>
    ///  Definition of a property
    /// </summary>
    public PropertyDefinition.PropertyDefinition Definition { get; private set; }

    /// <summary>
    ///  Typed value of a property
    /// </summary>
    public T? Value => _value;

    /// <summary>
    ///  Value of property being returned in a form of an abstract object
    /// </summary>
    public object? ValueObject => _value;

    /// <summary>
    ///  Set value object
    /// </summary>
    /// <param name="value"></param>
    public void SetValueObject(object? value) => _value = (T?)value;
}
