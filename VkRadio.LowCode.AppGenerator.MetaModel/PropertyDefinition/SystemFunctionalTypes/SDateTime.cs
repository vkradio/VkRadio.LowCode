namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;

/// <summary>
/// System type for storing a date and time.
/// Differs from a standard DateTime in that it allow to select whether to return a fixed
/// DateTime value or return a current runtime DateTime value.
/// </summary>
public class SDateTime
{
    DateTime? _fixedValue;
    bool _useModelRuntimeValue;

    /// <summary>
    /// Constructor variant for a fixed date and time value
    /// </summary>
    /// <param name="fixedValue">Fixed value of date and time</param>
    public SDateTime(DateTime fixedValue)
    {
        _fixedValue = fixedValue;
    }
    /// <summary>
    /// Constructor variant for a runtime date and time value
    /// </summary>
    public SDateTime()
    {
        _fixedValue = null;
        _useModelRuntimeValue = true;
    }

    /// <summary>
    /// Fixed date and time value.
    /// Note: If the fixed value is not set (requring getting a current runtime value),
    /// an attempt to get a fixed value will throw an exception.
    /// </summary>
    public DateTime FixedValue
    {
        get
        {
            if (UseModelRuntimeValue)
            {
                throw new InvalidOperationException("Unable to get a fixed value for a current runtime date and time variant.");
            }

            return _fixedValue!.Value;
        }
        set
        {
            _fixedValue = value;
            _useModelRuntimeValue = false;
        }
    }

    /// <summary>
    /// Do we need to extract a current runtime date and time value
    /// </summary>
    public bool UseModelRuntimeValue
    {
        get
        {
            return _useModelRuntimeValue;
        }
        set
        {
            _useModelRuntimeValue = value;
            _fixedValue = _useModelRuntimeValue
                ? null
                : DateTime.Now;
        }
    }
}
