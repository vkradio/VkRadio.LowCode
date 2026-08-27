using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;

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
        var xel = new XElement("root", _text);
        var encodedText = xel.ToString();
        encodedText = encodedText.Substring(6, encodedText.Length - 13);

        return
        [
            "/// <summary>",
            "/// " + encodedText,
            "/// </summary>"
        ];
    }
}
