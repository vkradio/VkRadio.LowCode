using VkRadio.Patterns.Guards;

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
        Class = GuardRule.Forbid.Null(owningClass, nameof(owningClass));
        Name = GuardRule.Forbid.NullOrEmpty(name, nameof(name));
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
    public Class Class { get; protected set; }

    /// <summary>
    /// DocComment
    /// </summary>
    public AbstractDocComment? DocComment { get; protected set; }

    /// <summary>
    /// Method visibility
    /// </summary>
    public ElementVisibilityAbstract Visibility { get; protected set; }

    /// <summary>
    /// Whether a method is static
    /// </summary>
    public bool IsStatic { get; protected set; }

    /// <summary>
    /// Method name
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Parameters (key - name, value - the parameter)
    /// </summary>
    public IDictionary<string, ParameterSimple> Params { get; protected set; }

    /// <summary>
    /// Method body strings
    /// </summary>
    public IList<string> BodyStrings { get; protected set; }

    /// <summary>
    /// Should generate method body as a single line with its name
    /// </summary>
    public bool HintSingleLineBody { get; protected set; }

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
