namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter;

public class CSPropertyGetter
{
    protected string? _predefinedCode;

    public CSPropertyGetter(CSProperty property, bool singleLineHint, string? predefinedCode)
    {
        _predefinedCode = predefinedCode;
        Property = property;
        SingleLineHint = singleLineHint;
    }

    public CSPropertyGetter(CSProperty property, bool singleLineHint)
        : this(property, singleLineHint, null)
    {
    }

    public CSPropertyGetter(CSProperty property)
        : this(property, true)
    {
    }

    public CSProperty Property { get; set; }
    public bool SingleLineHint { get; set; } = true;

    public virtual string[] GenerateText()
    {
        var text = new List<string>();

        if (SingleLineHint)
        {
            var line = _predefinedCode != null
                ? _predefinedCode
                : $"get => {Property.NameFieldCorresponding};";

            text.Add(line);
        }
        else
        {
            text.Add("get");
            text.Add("{");

            if (_predefinedCode != null)
            {
                text.Add(_predefinedCode);
            }
            else
            {
                text.Add($"    return {Property.NameFieldCorresponding};");
            }

            text.Add("}");
        }

        return text.ToArray();
    }
}
