namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;

/// <summary>
/// System (.NET) type for storing a unique (GUID) code.
/// Differst from a standard Guid in that it allows to select whether to store
/// a fixed GUID value, or to generate a new random value at runtime
/// </summary>
public class SGuid
{
    Guid? _fixedValue;
    bool _generateValueAtRunTime;

    /// <summary>
    /// Constructor variant for a fixed GUID value
    /// </summary>
    /// <param name="fixedValue">Фиксированное значение даты и времени</param>
    public SGuid(Guid fixedValue)
    {
        _fixedValue = fixedValue;
    }

    /// <summary>
    /// Constructor variant for a case with generating a random value at runtime
    /// </summary>
    public SGuid()
    {
        _fixedValue = null;
        _generateValueAtRunTime = true;
    }

    /// <summary>
    /// Fixed GUID value.
    /// Note: In case a fixed value is not set (requiring a random GUID generation), an attempt
    /// of getting a fixed value will throw an exception
    /// </summary>
    public Guid FixedValue
    {
        get
        {
            if (GenerateValueAtRunTime)
            {
                throw new InvalidOperationException("Unable to get a fixed GUID value for a random generation variant.");
            }

            return _fixedValue!.Value;
        }
        set
        {
            _fixedValue = value;
            _generateValueAtRunTime = false;
        }
    }

    /// <summary>
    /// Do we need to generate a random value every time
    /// </summary>
    public bool GenerateValueAtRunTime
    {
        get
        {
            return _generateValueAtRunTime;
        }
        set
        {
            _generateValueAtRunTime = value;
            _fixedValue = _generateValueAtRunTime
                ? null
                : Guid.NewGuid();
        }
    }
}
