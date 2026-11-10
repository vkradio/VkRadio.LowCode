using Ardalis.GuardClauses;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;

/// <summary>
/// C# property with fully predefined single-line code (actually a quick hack here)
/// </summary>
public class CSPropertyPredefined : ICSProperty
{
    public CSPropertyPredefined(string name, string predefinedValue, XmlComment? docComment = null)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        PredefinedValue = Guard.Against.NullOrEmpty(predefinedValue, nameof(predefinedValue));
        DocComment = docComment;
    }

    public string Name { get; private set; }

    public XmlComment? DocComment { get; private set; }

    public string PredefinedValue { get; private set; }

    public string[] GenerateText()
    {
        var text = new List<string>();

        if (DocComment is not null)
        {
            text.AddRange(DocComment.GenerateText());
        }

        text.Add(PredefinedValue);

        for (var i = 0; i < text.Count; i++)
        {
            text[i] = CSharpHelper.C_TAB + text[i];
        }

        return [.. text];
    }
}
