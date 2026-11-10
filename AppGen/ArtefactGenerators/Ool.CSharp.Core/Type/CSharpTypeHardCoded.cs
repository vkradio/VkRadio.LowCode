using Ardalis.GuardClauses;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpTypeHardCoded : CSharpTypeBase
{
    private readonly string _hardCodedValue;

    public CSharpTypeHardCoded(string hardCodedValue)
        : base(hardCodedValue)
    {
        _hardCodedValue = Guard.Against.NullOrEmpty(hardCodedValue, nameof(hardCodedValue));
    }

    public override string GenerateLiteral() => _hardCodedValue;
}
