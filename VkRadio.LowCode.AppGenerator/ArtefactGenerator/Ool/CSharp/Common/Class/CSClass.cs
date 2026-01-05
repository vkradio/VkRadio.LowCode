using AbstClassNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Class;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class;

/// <summary>
/// C# class
/// </summary>
public class CSClass : AbstClassNS.Class
{
    const string c_tab = "    ";

    protected Dictionary<string, CSProperty> _properties = [];
    protected Dictionary<string, CSConstructor> _constructors = [];

    protected override string[] GenerateClassDocComment()
    {
        var text = base.GenerateClassDocComment();
        
        for (var i = 0; i < text.Length; i++)
        {
            text[i] = c_tab + text[i];
        }

        return text;
    }

    protected override string[] GenerateClassHeader()
    {
        var text = new List<string>();

        var classNameString = $"public{(Partial ? " partial" : string.Empty)} class {_name}";

        if (!string.IsNullOrEmpty(_inheritsFrom))
        {
            classNameString += (": " + _inheritsFrom);
        }

        text.Add(classNameString);
        text.Add("{");

        for (var i = 0; i < text.Count; i++)
        {
            text[i] = c_tab + text[i];
        }

        return text.ToArray();
    }

    protected override string[] GenerateClassBodyLines()
    {
        var text = new List<string>();

        var textEmbeddedClass = new List<string>();

        if (EmbeddedClassPredefined != null)
        {
            textEmbeddedClass.AddRange(EmbeddedClassPredefined.GenerateText());
        }

        var textFields = new List<string>();

        foreach (var field in _fields.Values)
        {
            textFields.AddRange(field.GenerateText());
        }

        var textConstructors = new List<string>();

        foreach (var ctor in _constructors.Values)
        {
            textConstructors.AddRange(ctor.GenerateText());
        }

        var textMethods = new List<string>();

        foreach (var method in _methods.Values)
        {
            textMethods.AddRange(method.GenerateText());
        }

        var textConstants = new List<string>();

        foreach (var constant in _constants.Values)
        {
            textConstants.AddRange(constant.GenerateText());
        }

        var textProperties = new List<string>();

        foreach (var prop in _properties.Values)
        {
            textProperties.AddRange(prop.GenerateText());
        }

        if (textEmbeddedClass.Count != 0)
        {
            text.AddRange(textEmbeddedClass);

            if (textFields.Count != 0 || textMethods.Count != 0 || textConstants.Count != 0 || textProperties.Count != 0)
            {
                text.Add(string.Empty);
            }
        }

        if (textFields.Count != 0)
        {
            text.AddRange(textFields);

            if (textConstructors.Count != 0 || textMethods.Count != 0 || textConstants.Count != 0 || textProperties.Count != 0)
            {
                text.Add(string.Empty);
            }
        }

        if (textConstructors.Count != 0)
        {
            text.AddRange(textConstructors);

            if (textMethods.Count != 0 || textConstants.Count != 0 || textProperties.Count != 0)
            {
                text.Add(string.Empty);
            }
        }

        if (textMethods.Count != 0)
        {
            text.AddRange(textMethods);

            if (textConstants.Count != 0 || textProperties.Count != 0)
            {
                text.Add(string.Empty);
            }
        }

        if (textConstants.Count != 0)
        {
            text.AddRange(textConstants);

            if (textProperties.Count != 0)
            {
                text.Add(string.Empty);
            }
        }

        if (textProperties.Count != 0)
        {
            text.AddRange(textProperties);
        }

        for (var i = 0; i < text.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(text[i]))
            {
                text[i] = c_tab + text[i];
            }
        }

        return text.ToArray();
    }

    protected override string[] GenerateClassFooter() { return [c_tab + "};"]; }

    /// <summary>
    /// Embedded class for fast extraction of a predefined data object
    /// </summary>
    public CSClassPredefined EmbeddedClassPredefined { get; set; }
    /// <summary>
    /// Is it a partial class representation
    /// </summary>
    public bool Partial { get; set; }
    /// <summary>
    /// Class properties
    /// </summary>
    public IDictionary<string, CSProperty> Properties { get { return _properties; } }
    /// <summary>
    /// Class constructors
    /// </summary>
    public IDictionary<string, CSConstructor> Constructors { get { return _constructors; } }
}
