namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;

/// <summary>
/// Abstract comment in style of PHPDoc, XMLDoc, etc.
/// </summary>
public abstract class AbstractDocComment
{
    protected string _text;

    public AbstractDocComment(string text)
    {
        _text = text;
    }

    /// <summary>
    /// Comment text
    /// </summary>
    public string Text { get => _text; set => _text = value; }

    public abstract string[] GenerateText();
}
