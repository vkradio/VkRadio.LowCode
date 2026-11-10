namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Setter;

public class CSPropertySetterCachedObject : CSPropertySetter
{
    public CSPropertySetterCachedObject(CSProperty property)
        : base(property)
    {
    }

    protected override string[] GenerateLinesSetValue()
    {
        var text = new List<string>
        {
            $"{GetCorrespondingName()} = value;",
            string.Format("{0}Id = {0} != null ? (Guid?){0}.Id : null;", GetCorrespondingName())
        };

        return [.. text];
    }
}
