using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpTypeHardCoded : CSharpTypeBase
{
    private readonly string _hardCodedValue;

    public CSharpTypeHardCoded(string hardCodedValue)
        : base(hardCodedValue)
    {
        _hardCodedValue = GuardRule.Forbid.NullOrEmpty(hardCodedValue, nameof(hardCodedValue));
    }

    public override string GenerateLiteral(string? format) => _hardCodedValue;

    public override string? DefaultValue() => null;
}
