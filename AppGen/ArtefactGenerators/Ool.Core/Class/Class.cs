using VkRadio.Patterns.Guards;
using CompNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Component;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

/// <summary>
/// Class
/// </summary>
public abstract class Class
{
    protected Dictionary<string, Method> _methods = [];
    protected Dictionary<string, ClassField> _fields = [];
    protected Dictionary<string, ClassConstant> _constants = [];

    protected virtual string[] GenerateClassDocComment() => DocComment?.GenerateText() ?? [];

    protected abstract string[] GenerateClassHeader();

    protected abstract string[] GenerateClassBodyLines();

    protected abstract string[] GenerateClassFooter();

    public Class(string name, CompNS.Component component, AbstractDocComment? docComment = null, string? inheritsFrom = null)
    {
        Name = GuardRule.Forbid.NullOrEmpty(name, nameof(name));
        Component = GuardRule.Forbid.Null(component, nameof(component));
        DocComment = docComment;
        InheritsFrom = inheritsFrom;
    }



    /// <summary>
    /// DocComment
    /// </summary>
    public AbstractDocComment? DocComment { get; protected set; }

    /// <summary>
    /// Component, containing this class
    /// </summary>
    public CompNS.Component Component { get; protected set; }

    /// <summary>
    /// Inheriting string - class and interfaces, from which this class is derived.
    /// Will be inserted in the generated code as defined here. If no value defined,
    /// this mean class is not derived from anything (explicitly).
    /// </summary>
    public string? InheritsFrom { get; protected set; }

    /// <summary>
    /// Class name
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Methods.
    /// Method key in the dictionary has a form &quot;name::param1,param2&quot;, and when there are no params,
    /// then &quot;name&quot;.
    /// </summary>
    public IReadOnlyDictionary<string, Method> Methods => _methods.AsReadOnly();

    public void AddMethod(Method method) => _methods.Add(GuardRule.Forbid.Null(method, nameof(method)).Name, method);

    public void AddMethod(string name, Method method) => _methods.Add(
        GuardRule.Forbid.NullOrEmpty(name, nameof(name)),
        GuardRule.Forbid.Null(method, nameof(method))
    );

    /// <summary>
    /// Fields
    /// </summary>
    public IReadOnlyDictionary<string, ClassField> Fields => _fields.AsReadOnly();

    public void AddField(ClassField field) => _fields.Add(GuardRule.Forbid.Null(field, nameof(field)).Name, field);

    /// <summary>
    /// Constants
    /// </summary>
    public IReadOnlyDictionary<string, ClassConstant> Constants => _constants.AsReadOnly();

    public void AddConstant(ClassConstant constant) => _constants.Add(GuardRule.Forbid.Null(constant, nameof(constant)).Name, constant);

    public virtual string[] GenerateText() => [
        .. GenerateClassDocComment(),
        .. GenerateClassHeader(),
        .. GenerateClassBodyLines(),
        .. GenerateClassFooter()
    ];
}
