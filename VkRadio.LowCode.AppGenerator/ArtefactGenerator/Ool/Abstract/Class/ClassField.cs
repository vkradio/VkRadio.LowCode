namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Class;

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
    public Class Class { get { return _class; } set { _class = value; } }
    /// <summary>
    /// Comment
    /// </summary>
    public AbstractDocComment DocComment { get { return _docComment; } set { _docComment = value; } }
    /// <summary>
    /// Field visibility
    /// </summary>
    public ElementVisibilityAbstract Visibility { get { return _visibility; } set { _visibility = value; } }
    /// <summary>
    /// Initial value
    /// </summary>
    public string InitialValue { get { return _initialValue; } set { _initialValue = value; } }
    /// <summary>
    /// Field name
    /// </summary>
    public string Name { get { return _name; } set { _name = value; } }
    /// <summary>
    /// Whether field is static
    /// </summary>
    public bool IsStatic { get { return _isStatic; } set { _isStatic = value; } }

    public virtual string[] GenerateText()
    {
        var text = new List<string>();

        if (_docComment != null)
        {
            var commentStrings = _docComment.GenerateText();

            for (var i = 0; i < commentStrings.Length; i++)
            {
                commentStrings[i] = "    " + commentStrings[i];
            }

            text.AddRange(commentStrings);
        }

        text.Add(GenerateTextConcrete());

        return text.ToArray();
    }
}
