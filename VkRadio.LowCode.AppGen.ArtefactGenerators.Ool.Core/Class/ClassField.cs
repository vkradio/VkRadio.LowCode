namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

/// <summary>
/// Field of an abstract class
/// </summary>
public abstract class ClassField
{
    protected Class _class;
    protected AbstractDocComment _docComment;
    protected ElementVisibilityAbstract _visibility;
    protected string _initialValue;
    protected string _name;
    protected bool _isStatic;

    protected abstract string GenerateTextConcrete();

    /// <summary>
    /// Owning class
    /// </summary>
    public Class Class { get => _class; set => _class = value; }

    /// <summary>
    /// Comment
    /// </summary>
    public AbstractDocComment DocComment { get => _docComment; set => _docComment = value; }

    /// <summary>
    /// Field visibility
    /// </summary>
    public ElementVisibilityAbstract Visibility { get => _visibility; set => _visibility = value; }

    /// <summary>
    /// Initial value
    /// </summary>
    public string InitialValue { get => _initialValue; set => _initialValue = value; }

    /// <summary>
    /// Field name
    /// </summary>
    public string Name { get => _name; set => _name = value; }

    /// <summary>
    /// Whether field is static
    /// </summary>
    public bool IsStatic { get => _isStatic; set => _isStatic = value; }

    public virtual string[] GenerateText()
    {
        var text = new List<string>();

        if (_docComment is not null)
        {
            var commentStrings = _docComment.GenerateText();

            for (var i = 0; i < commentStrings.Length; i++)
            {
                commentStrings[i] = "    " + commentStrings[i];
            }

            text.AddRange(commentStrings);
        }

        text.Add(GenerateTextConcrete());

        return [.. text];
    }
}
