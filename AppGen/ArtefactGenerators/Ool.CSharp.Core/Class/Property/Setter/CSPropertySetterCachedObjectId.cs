namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Setter;

public class CSPropertySetterCachedObjectId : CSPropertySetter
{
    public CSPropertySetterCachedObjectId(CSProperty property)
        : base(property)
    {
    }

    protected override string[] GenerateLinesSetValue()
    {
        var text = new List<string>
        {
            $"{GetCorrespondingName()} = value;"
        };

        var objectFieldName = GetCorrespondingName().Substring(0, GetCorrespondingName().Length - 2);

        text.Add($"{objectFieldName} = null;");

        return [.. text];
    }
}
