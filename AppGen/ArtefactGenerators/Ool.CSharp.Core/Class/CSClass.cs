using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Constant;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;
using AbstClassNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class;

/// <summary>
/// C# class
/// </summary>
public class CSClass : AbstClassNS.Class
{
    protected Dictionary<string, ICSProperty> _properties = [];
    protected Dictionary<string, CSConstructor> _constructors = [];

    /// <summary>
    /// Constants
    /// </summary>
    public new IDictionary<string, CSClassConstant> Constants => _constants
        .Values
        .Cast<CSClassConstant>()
        .ToDictionary(x => x.Name);

    protected override string[] GenerateClassDocComment()
    {
        var text = base.GenerateClassDocComment();
        
        for (var i = 0; i < text.Length; i++)
        {
            text[i] = CSharpHelper.C_TAB + text[i];
        }

        return text;
    }

    protected override string[] GenerateClassHeader()
    {
        var text = new List<string>();

        var classNameString = $"public{(Partial ? " partial" : string.Empty)} class {_name}";

        if (!string.IsNullOrEmpty(_inheritsFrom))
        {
            classNameString += ": " + _inheritsFrom;
        }

        text.Add(classNameString);
        text.Add("{");

        for (var i = 0; i < text.Count; i++)
        {
            text[i] = CSharpHelper.C_TAB + text[i];
        }

        return [.. text];
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

        if (Constructors.Values.Count(x => x.IsStatic) == 0 &&
            Constants.Values.Count(x => !x.TrueConst) != 0)
        {
            var staticInitCtor = new CSConstructor(
                this,
                null,
                ElementVisibilityClassic.Private,
                null,
                new List<string>(),
                null,
                true,
                false
            );

            _constructors.Add(CSharpHelper.GenerateMethodKey(staticInitCtor), staticInitCtor);
        }

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
                text[i] = CSharpHelper.C_TAB + text[i];
            }
        }

        return [.. text];
    }

    protected override string[] GenerateClassFooter() => [CSharpHelper.C_TAB + "};"];

    /// <summary>
    /// Embedded class for fast extraction of a predefined data object
    /// </summary>
    public CSClassPredefinedDataObject? EmbeddedClassPredefined { get; set; }

    /// <summary>
    /// Is it a partial class representation
    /// </summary>
    public bool Partial { get; set; }

    /// <summary>
    /// Class properties
    /// </summary>
    public IDictionary<string, ICSProperty> Properties => _properties;

    /// <summary>
    /// Class constructors
    /// </summary>
    public IDictionary<string, CSConstructor> Constructors => _constructors;
}
