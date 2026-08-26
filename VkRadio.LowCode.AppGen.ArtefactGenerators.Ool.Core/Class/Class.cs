using CompNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Component;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

/// <summary>
/// Class
/// </summary>
public abstract class Class
{
    protected AbstractDocComment _docComment;
    protected CompNS.Component _component;
    protected string _inheritsFrom;
    protected string _name;
    protected Dictionary<string, Method> _methods = [];
    protected Dictionary<string, ClassField> _fields = [];
    protected Dictionary<string, ClassConstant> _constants = [];

    protected virtual string[] GenerateClassDocComment() => _docComment?.GenerateText() ?? [];

    protected abstract string[] GenerateClassHeader();

    protected abstract string[] GenerateClassBodyLines();

    protected abstract string[] GenerateClassFooter();

    /// <summary>
    /// DocComment
    /// </summary>
    public AbstractDocComment DocComment { get => _docComment; set => _docComment = value; }

    /// <summary>
    /// Component, containing this class
    /// </summary>
    public CompNS.Component Component { get => _component; set => _component = value; }

    /// <summary>
    /// Inheriting string - class and interfaces, from which this class is derived.
    /// Will be inserted in the generated code as defined here. If no value defined,
    /// this mean class is not derived from anything (explicitly).
    /// </summary>
    public string InheritsFrom { get => _inheritsFrom; set => _inheritsFrom = value; }

    /// <summary>
    /// Class name
    /// </summary>
    public string Name { get => _name; set => _name = value; }

    /// <summary>
    /// Methods.
    /// Method key in the dictionary has a form &quot;name::param1,param2&quot;, and when there are no params,
    /// then &quot;name&quot;.
    /// </summary>
    public IDictionary<string, Method> Methods => _methods;

    /// <summary>
    /// Fields
    /// </summary>
    public IDictionary<string, ClassField> Fields => _fields;

    /// <summary>
    /// Constants
    /// </summary>
    public IDictionary<string, ClassConstant> Constants => _constants;

    public virtual string[] GenerateText() => [
        .. GenerateClassDocComment(),
        .. GenerateClassHeader(),
        .. GenerateClassBodyLines(),
        .. GenerateClassFooter()
    ];
}
