namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;

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
    public string Text { get { return _text; } set { _text = value; } }

    public abstract string[] GenerateText();
}
