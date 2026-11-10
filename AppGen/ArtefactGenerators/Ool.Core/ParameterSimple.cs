using Ardalis.GuardClauses;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;

public class ParameterSimple
{
    protected string _name;
    protected string? _value;

    public ParameterSimple(string name, string? value)
    {
        _name = Guard.Against.NullOrEmpty(name, nameof(name));
        _value = value;
    }

    public string Name { get => _name; protected set => _name = value; }

    public string? Value { get => _value; protected set => _value = value; }
}
