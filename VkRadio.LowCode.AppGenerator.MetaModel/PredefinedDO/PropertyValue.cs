namespace VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;

/// <summary>
/// Value of a property
/// </summary>
/// <typeparam name="T">System (.NET) type of a value</typeparam>
public class PropertyValue<T>: IPropertyValue
{
    PropertyDefinition.PropertyDefinition _definition;
    T _value;

    /// <summary>
    /// Definition of a property
    /// </summary>
    public PropertyDefinition.PropertyDefinition Definition { get { return _definition; } set { _definition = value; } }
    /// <summary>
    /// Typed value of a property
    /// </summary>
    public T Value { get { return _value; } }
    /// <summary>
    /// Value of property being returned in a form of an abstract object
    /// </summary>
    public object ValueObject { get { return _value; } set { _value = (T)value; } }
}
