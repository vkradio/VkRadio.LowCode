namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Setter;

public class CSPropertySetter
{
    protected bool _useModify = true;

    public CSPropertySetter(CSProperty property)
        : this(property, true, true)
    {
    }

    public CSPropertySetter(CSProperty property, bool singleLineHint, bool useModify)
    {
        Property = property;
        SingleLineHint = singleLineHint;
        _useModify = useModify;
    }

    protected virtual string GenerateLineModify() => "Modify();";
    protected virtual string[] GenerateLinesSetValue() => [$"{Property.NameFieldCorresponding} = value;"];

    public CSProperty Property { get; set; }
    public bool SingleLineHint { get; set; } = true;

    public virtual string[] GenerateText()
    {
        var text = new List<string>();

        var modify = _useModify ? (GenerateLineModify() + " ") : string.Empty;

        if (SingleLineHint)
        {
            var line = $"set {{ {modify}{string.Join(" ", GenerateLinesSetValue())} }}";
            text.Add(line);
        }
        else
        {
            text.Add("set");
            text.Add("{");
            text.Add("    " + modify.Substring(0, modify.Length - 1));

            var setLines = GenerateLinesSetValue();

            foreach (var setLine in setLines)
            {
                text.Add("    " + setLine);
            }

            text.Add("}");
        }

        return text.ToArray();
    }
}
