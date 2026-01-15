namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Class;

public abstract class ClassConstant
{
    protected string _name;
    protected string _value;
    protected AbstractDocComment _docComment;
    protected Class _class;
    protected ElementVisibilityAbstract _visibility;

    protected abstract string GenerateTextConcrete();

    public string Name { get { return _name; } set { _name = value; } }
    public string Value { get { return _value; } set { _value = value; } }
    public AbstractDocComment DocComment { get { return _docComment; } set { _docComment = value; } }
    public Class Class { get { return _class; } set { _class = value; } }
    public ElementVisibilityAbstract Visibility { get { return _visibility; } set { _visibility = value; } }

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
