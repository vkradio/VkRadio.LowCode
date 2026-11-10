using Ardalis.GuardClauses;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

/// <summary>
/// Abstract method of a class
/// </summary>
public abstract class Method
{
    protected Method(
        Class owningClass,
        string name,
        AbstractDocComment? docComment = null,
        ElementVisibilityAbstract? visibility = null,
        IEnumerable<string>? bodyStrings = null,
        IEnumerable<ParameterSimple>? parameters = null,
        bool isStatic = false,
        bool hintSingleLine = false
    )
    {
        Class = Guard.Against.Null(owningClass, nameof(owningClass));
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        DocComment = docComment;
        Visibility = visibility ?? ElementVisibilityClassic.Public;
        IsStatic = isStatic;
        Params = (parameters ?? []).ToDictionary(x => x.Name, x => x);
        BodyStrings = (bodyStrings ?? []).ToList();
        HintSingleLineBody = hintSingleLine;
    }

    protected abstract string[] GenerateTextConcrete();

    /// <summary>
    /// Owning class
    /// </summary>
    public Class Class { get; protected set; } //{ get => _class; set => _class = value; }

    /// <summary>
    /// DocComment
    /// </summary>
    public AbstractDocComment? DocComment { get; protected set; } // { get => _docComment; protected set => _docComment = value; }

    /// <summary>
    /// Method visibility
    /// </summary>
    public ElementVisibilityAbstract Visibility { get; protected set; } // { get => _visibility; protected set => _visibility = value; }

    /// <summary>
    /// Whether a method is static
    /// </summary>
    public bool IsStatic { get; protected set; } // { get => _isStatic; protected set => _isStatic = value; }

    /// <summary>
    /// Method name
    /// </summary>
    public string Name { get; protected set; } //{ get => _name; protected set => _name = value; }

    /// <summary>
    /// Parameters (key - name, value - the parameter)
    /// </summary>
    public IDictionary<string, ParameterSimple> Params { get; protected set; } // => _params;

    /// <summary>
    /// Method body strings
    /// </summary>
    public IList<string> BodyStrings { get; protected set; } //=> _bodyStrings;

    /// <summary>
    /// Should generate method body as a single line with its name
    /// </summary>
    public bool HintSingleLineBody { get; protected set; } // { get => _hintSingleLineBody; protected set => _hintSingleLineBody = value; }

    public virtual string TabChars => "    ";

    public virtual string[] GenerateText()
    {
        var text = new List<string>();

        if (DocComment is not null)
        {
            var commentStrings = DocComment.GenerateText();

            for (var i = 0; i < commentStrings.Length; i++)
            {
                commentStrings[i] = TabChars + commentStrings[i];
            }

            text.AddRange(commentStrings);
        }

        text.AddRange(GenerateTextConcrete());

        return [.. text];
    }
}
