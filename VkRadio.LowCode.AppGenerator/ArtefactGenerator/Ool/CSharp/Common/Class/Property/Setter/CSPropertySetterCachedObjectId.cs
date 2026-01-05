namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Setter;

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
            $"{Property.NameFieldCorresponding} = value;"
        };

        var objectFieldName = Property.NameFieldCorresponding.Substring(0, Property.NameFieldCorresponding.Length - 2);

        text.Add($"{objectFieldName} = null;");

        return text.ToArray();
    }
}
