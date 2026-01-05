namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;

/// <summary>
/// Reference type.
/// When defined contains only a reference (Guid), but then a delayed linking will assing
/// a value of a real data object
/// </summary>
public class SRefObject
{
    Guid _key;
    object? _value;

    /// <summary>
    /// Constructor for initializing only a reference (Guid)
    /// </summary>
    /// <param name="key">Reference</param>
    public SRefObject(Guid key)
    {
        _key = key;
    }

    /// <summary>
    /// Reference (key) of a value
    /// </summary>
    public Guid Key { get { return _key; } }
    /// <summary>
    /// Value of an object
    /// </summary>
    public object? Value { get { return _value; } set { _value = value; } }
}
