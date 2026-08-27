namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;

/// <summary>
/// C# property with fully predefined single-line code (actually a quick hack here)
/// </summary>
public class CSPropertyPredefined : CSProperty
{
    public override string[] GenerateText()
    {
        var text = new List<string>();

        if (DocComment is not null)
        {
            text.AddRange(DocComment.GenerateText());
        }

        text.Add(PredefinedValue);

        for (var i = 0; i < text.Count; i++)
        {
            text[i] = c_tab + text[i];
        }

        return [.. text];
    }

    public string PredefinedValue { get; set; }
}
