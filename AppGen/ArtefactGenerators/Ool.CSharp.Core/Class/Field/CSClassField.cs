using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Field;

/// <summary>
/// C# class field
/// </summary>
public class CSClassField : ClassField, IPropertyOrField
{
    public CSClassField(
        CSharpTypeBase type,
        string name,
        CSClass? owningClass = null,
        XmlComment? docComment = null,
        ElementVisibilityAbstract? visibility = null,
        string? initialValue = null,
        bool isStatic = false
    ) : base(
        name,
        owningClass,
        docComment,
        visibility,
        initialValue,
        isStatic
    )
    {
        Type = Guard.Against.Null(type, nameof(type));
    }

    public CSharpTypeBase Type { get; private set; }

    public new XmlComment? DocComment { get => (XmlComment?)_docComment; set => _docComment = value; }

    protected override string GenerateTextConcrete()
    {
        var addKeywords = string.Empty;

        if (_isStatic)
        {
            addKeywords += "static ";
        }

        return string.Format(
            "{0}{1}{2}{3} {4}{5};",
            TabChars,
            _visibility.Value != ElementVisibilityEnum.Private ? _visibility.ToString() + " " : string.Empty,
            addKeywords,
            Type,
            _name,
            string.IsNullOrEmpty(_initialValue) ? string.Empty : " = " + _initialValue
        );
    }

    public override string TabChars => CSharpHelper.C_TAB;
}
