using Ardalis.GuardClauses;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

public abstract class ClassConstant
{
    //protected string _name;
    //protected string _value;
    //protected AbstractDocComment _docComment;
    ////protected Class _class;
    //protected ElementVisibilityAbstract _visibility;

    protected ClassConstant(string name, string value, AbstractDocComment? docComment = null, ElementVisibilityAbstract? visibility = null)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Value = Guard.Against.Null(value, nameof(value));
        DocComment = docComment;
        Visibility = visibility ?? ElementVisibilityClassic.Protected;
    }

    protected abstract string GenerateTextConcrete();

    public string Name { get; protected set; } //{ get => _name; set => _name = value; }

    public string Value { get; protected set; } //{ get => _value; set => _value = value; }

    public AbstractDocComment? DocComment { get; protected set; } //{ get => _docComment; set => _docComment = value; }

    //public Class Class { get => _class; set => _class = value; }

    public ElementVisibilityAbstract Visibility { get; protected set; } // { get => _visibility; set => _visibility = value; }

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

        text.Add(GenerateTextConcrete());

        return [.. text];
    }

    public virtual string TabChars => "    ";
}
