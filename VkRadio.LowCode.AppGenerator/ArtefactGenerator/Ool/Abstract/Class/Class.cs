using CompNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Class;

/// <summary>
/// Class
/// </summary>
public abstract class Class
{
    protected AbstractDocComment _docComment;
    protected CompNS.Component _component;
    protected string _inheritsFrom;
    protected string _name;
    protected Dictionary<string, Method> _methods = new Dictionary<string, Method>();
    protected Dictionary<string, ClassField> _fields = new Dictionary<string, ClassField>();
    protected Dictionary<string, ClassConstant> _constants = new Dictionary<string, ClassConstant>();

    protected virtual string[] GenerateClassDocComment()
    {
        return _docComment != null ? _docComment.GenerateText() : new string[0];
    }
    protected abstract string[] GenerateClassHeader();
    protected abstract string[] GenerateClassBodyLines();
    protected abstract string[] GenerateClassFooter();

    /// <summary>
    /// DocComment
    /// </summary>
    public AbstractDocComment DocComment { get { return _docComment; } set { _docComment = value; } }
    /// <summary>
    /// Component, containing this class
    /// </summary>
    public CompNS.Component Component { get { return _component; } set { _component = value; } }
    /// <summary>
    /// Inheriting string - class and interfaces, from which this class is derived.
    /// Will be inserted in the generated code as defined here. If no value defined,
    /// this mean class is not derived from anything (explicitly).
    /// </summary>
    public string InheritsFrom { get { return _inheritsFrom; } set { _inheritsFrom = value; } }
    /// <summary>
    /// Имя класса
    /// </summary>
    public string Name { get { return _name; } set { _name = value; } }
    /// <summary>
    /// Methods.
    /// Method key in the dictionary has a form &quot;name::param1,param2&quot;, and when there are no params,
    /// then &quot;name&quot;.
    /// </summary>
    public IDictionary<string, Method> Methods { get { return _methods; } }
    /// <summary>
    /// Fields
    /// </summary>
    public IDictionary<string, ClassField> Fields { get { return _fields; } }
    /// <summary>
    /// Constants
    /// </summary>
    public IDictionary<string, ClassConstant> Constants { get { return _constants; } }

    public virtual string[] GenerateText()
    {
        List<string> text = new List<string>();

        text.AddRange(GenerateClassDocComment());
        text.AddRange(GenerateClassHeader());
        text.AddRange(GenerateClassBodyLines());
        text.AddRange(GenerateClassFooter());

        return text.ToArray();
    }
}
