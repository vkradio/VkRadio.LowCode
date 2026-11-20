using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Getter;

public class CSPropertyGetterCachedObject : CSPropertyGetter
{
    public CSPropertyGetterCachedObject(CSProperty property)
        : base(property, true)
    {
    }

    public override string[] GenerateText()
    {
        var text = new List<string>();

        if (SingleLineHint)
        {
            var type = (CSharpReferenceType)Property.Type;
            var notNullableExclamation =
                type.SupportNullableRefTypes &&
                !type.IsNullableByDomain;

            var line = string.Format(
                "get {{ if ({0} == null && {0}Id.HasValue) {0} = ({1})StorageRegistry.Instance.{2}Storage.Restore({0}Id.Value){3}; return {0}; }}",
                GetCorrespondingName(),
                Property.Type,
                Property.Type.TypeName,
                notNullableExclamation ? "!" : string.Empty
            );

            text.Add(line);
        }
        else
        {
            throw new NotImplementedException("Generating not-single-line getter for CSPropertyGetterCachedObject not implemented.");
        }

        return [.. text];
    }
}
