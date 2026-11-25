using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;

public class ParameterTyped : ParameterSimple
{
    public ParameterTyped(string name, string type, string? value)
        : base(name, value)
    {
        _type = GuardRule.Forbid.NullOrEmpty(type);
    }

    protected string _type;

    public string Type { get => _type; protected set => _type = value; }
}
