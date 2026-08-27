namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

/// <summary>
/// Abstract method of a class
/// </summary>
public abstract class Method
{
    protected Class _class;
    protected AbstractDocComment _docComment;
    protected ElementVisibilityAbstract _visibility;
    protected bool _isStatic;
    protected string _name;
    protected Dictionary<string, ParameterSimple> _params = [];
    protected List<string> _bodyStrings = [];
    protected bool _hintSingleLineBody;

    protected abstract string[] GenerateTextConcrete();

    /// <summary>
    /// Owning class
    /// </summary>
    public Class Class { get => _class; set => _class = value; }

    /// <summary>
    /// DocComment
    /// </summary>
    public AbstractDocComment DocComment { get => _docComment; set => _docComment = value; }

    /// <summary>
    /// Method visibility
    /// </summary>
    public ElementVisibilityAbstract Visibility { get => _visibility; set => _visibility = value; }

    /// <summary>
    /// Whether a method is static
    /// </summary>
    public bool IsStatic { get => _isStatic; set => _isStatic = value; }

    /// <summary>
    /// Method name
    /// </summary>
    public string Name { get => _name; set => _name = value; }

    /// <summary>
    /// Parameters (key - name, value - the parameter)
    /// </summary>
    public IDictionary<string, ParameterSimple> Params => _params;

    /// <summary>
    /// Method body strings
    /// </summary>
    public IList<string> BodyStrings => _bodyStrings;

    /// <summary>
    /// Should generate method body as a single line with its name
    /// </summary>
    public bool HintSingleLineBody { get => _hintSingleLineBody; set => _hintSingleLineBody = value; }

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

        text.AddRange(GenerateTextConcrete());

        return [.. text];
    }
}
