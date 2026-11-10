using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Getter;

public class CSPropertyGetter
{
    protected string? _predefinedCode;

    protected string GetCorrespondingName() =>
        Property.CorrespondingPropertyOrField?.Name
        ?? throw new GeneratorException($"Neither {nameof(_predefinedCode)} set, not {nameof(Property.CorrespondingPropertyOrField)}, for a property {Property.Name}");

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

    public CSProperty Property { get; protected set; }

    public bool SingleLineHint { get; protected set; } = true;

    public virtual string[] GenerateText()
    {
        var text = new List<string>();

        if (SingleLineHint)
        {
            var line = _predefinedCode is not null
                ? _predefinedCode
                : $"get => {GetCorrespondingName()};";

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
                text.Add($"{CSharpHelper.C_TAB}return {GetCorrespondingName()};");
            }

            text.Add("}");
        }

        return [.. text];
    }
}
