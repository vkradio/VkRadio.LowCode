using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core;

/// <summary>
/// Comment in XML format for C#
/// </summary>
public class XmlComment : AbstractDocComment
{
    /// <summary>
    /// Constructor with text content initialization
    /// </summary>
    /// <param name="text">Comment text</param>
    public XmlComment(string text)
        : base(text)
    {
    }

    // TODO: Extend XmlDoc for an ability to comment params and return values
    public override string[] GenerateText()
    {
        var encodedText = NameHelper.EncodeXmlText(_text ?? string.Empty);

        return
        [
            "/// <summary>",
            "/// " + encodedText,
            "/// </summary>"
        ];
    }
}
