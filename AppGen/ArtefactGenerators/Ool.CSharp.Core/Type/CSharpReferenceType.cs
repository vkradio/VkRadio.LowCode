namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public abstract class CSharpReferenceType : CSharpTypeBase
{
    protected readonly bool _supportNullableRefTypes;

    protected readonly bool _isNullableByDomainOrSpecification;

    protected string GenerateNullableLiteral(bool? explicitIsNullable = null) => $"{_typeName}{((explicitIsNullable ?? _isNullableByDomainOrSpecification) && _supportNullableRefTypes ? "?" : string.Empty)}";

    protected CSharpReferenceType(bool supportNullableRefTypes, string typeName, bool isNullableByDomainOrSpecification)
        : base(typeName)
    {
        _supportNullableRefTypes = supportNullableRefTypes;

        _isNullableByDomainOrSpecification = isNullableByDomainOrSpecification;
    }

    public bool SupportNullableRefTypes => _supportNullableRefTypes;

    public bool IsNullableByDomain => _isNullableByDomainOrSpecification;

    public override string GenerateLiteral(string? format)
    {
        var fmt = (format ?? "G").ToLower();

        string result;

        switch (fmt)
        {
            case "n":
            case "nullable":

                var isNullable = _supportNullableRefTypes && _isNullableByDomainOrSpecification;

                // "nullable" parameter overrides the non-nullable domain types to be represented as nullables
                if (!isNullable && _supportNullableRefTypes)
                {
                    isNullable = true;
                }

                result = GenerateNullableLiteral(explicitIsNullable: isNullable);

                break;

            case "nq": // no question for reference types

                result = GenerateNullableLiteral(explicitIsNullable: false);

                break;

            default:

                result = GenerateNullableLiteral();

                break;
        }

        return result;
    }
}
