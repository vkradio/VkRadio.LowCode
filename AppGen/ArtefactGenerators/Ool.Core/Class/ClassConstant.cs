using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

public abstract class ClassConstant
{
    protected ClassConstant(string name, string value, AbstractDocComment? docComment = null, ElementVisibilityAbstract? visibility = null)
    {
        Name = GuardRule.Forbid.NullOrEmpty(name, nameof(name));
        Value = GuardRule.Forbid.Null(value, nameof(value));
        DocComment = docComment;
        Visibility = visibility ?? ElementVisibilityClassic.Protected;
    }

    protected abstract string GenerateTextConcrete();

    public string Name { get; protected set; }

    public string Value { get; protected set; }

    public AbstractDocComment? DocComment { get; protected set; }

    public ElementVisibilityAbstract Visibility { get; protected set; }

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
