namespace VkRadio.LowCode.AppGen.ArtefactGenerators.OolCore.Class;

public abstract class ClassConstant
{
    protected string _name;
    protected string _value;
    protected AbstractDocComment _docComment;
    protected Class _class;
    protected ElementVisibilityAbstract _visibility;

    protected abstract string GenerateTextConcrete();

    public string Name { get => _name; set => _name = value; }

    public string Value { get => _value; set => _value = value; }

    public AbstractDocComment DocComment { get => _docComment; set => _docComment = value; }

    public Class Class { get => _class; set => _class = value; }

    public ElementVisibilityAbstract Visibility { get => _visibility; set => _visibility = value; }

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
