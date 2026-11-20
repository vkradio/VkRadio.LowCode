using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Getter;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Setter;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;

public class CSProperty : ICSProperty, IPropertyOrField
{
    protected CSharpTypeBase? _explicitType;

    protected virtual string GenerateNamePart()
    {
        var additionalKeywords = string.Empty;

        if (IsStatic)
        {
            additionalKeywords += " static";
        }

        if (!string.IsNullOrWhiteSpace(AdditionalKeywords))
        {
            additionalKeywords += " " + AdditionalKeywords;
        }

        return $"public{additionalKeywords} {GetPropertyType()} {Name}";
    }

    protected CSharpTypeBase GetPropertyType() => CorrespondingPropertyOrField?.Type ?? _explicitType ?? throw new InvalidOperationException($"No Type set for property {Name}");

    public CSProperty(
        string name,
        IPropertyOrField? correspondingPropertyOrField,
        //CSharpTypeBase type,
        //CSClass cSharpClass,
        string? entityClassName = null,
        ElementVisibilityAbstract? visibility = null,
        bool isStatic = false,
        CSPropertyGetter? getter = null,
        CSPropertySetter? setter = null,
        XmlComment? docComment = null,
        string? additionalKeywords = null,
        CSharpTypeBase? explicitType = null
    )
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        CorrespondingPropertyOrField = correspondingPropertyOrField;
        EntityClassName = entityClassName;
        Visibility = visibility ?? ElementVisibilityClassic.Public;
        IsStatic = isStatic;
        Getter = getter;
        Setter = setter;
        DocComment = docComment;
        AdditionalKeywords = additionalKeywords;
        _explicitType = explicitType;
    }

    /// <summary>
    ///  Constructor to build a property based on a property definition
    /// </summary>
    /// <param name="propDef"></param>
    /// <param name="correspondingPropertyOrField"></param>
    /// <param name="docComment"></param>
    /// <param name="modificationForIdField">Variant of property for getting/setting id instead of a reference of an object</param>
    public CSProperty(PropertyDefinition propDef, IPropertyOrField correspondingPropertyOrField, XmlComment? docComment, bool modificationForIdField)
        : this(
            NameHelper.NamesToPascalCase(propDef.Names, true) + (modificationForIdField ? "Id" : string.Empty),
            Guard.Against.Null(correspondingPropertyOrField, nameof(correspondingPropertyOrField)),
            null,
            ElementVisibilityClassic.Public,
            false,
            null,
            null,
            docComment,
            null
        )
    {
        Guard.Against.Null(propDef, nameof(propDef));

        if (!modificationForIdField)
        {
            if (propDef.FunctionalType is PFTConnector ||
                propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTTableOwner)
            {
                Getter = new CSPropertyGetterCachedObject(this);
                Setter = new CSPropertySetterCachedObject(this);
            }
            else
            {
                Getter = new CSPropertyGetter(this);
                Setter = new CSPropertySetter(this);
            }
        }
        else
        {
            if (!(propDef.FunctionalType is PFTConnector ||
                  propDef.FunctionalType is PFTReferenceValue ||
                  propDef.FunctionalType is PFTTableOwner))
            {
                throw new ArgumentException($"{nameof(propDef)}", $"{nameof(propDef)}.{nameof(propDef.FunctionalType)} is not reference value ({propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {propDef.Id}).");
            }

            Getter = new CSPropertyGetter(this);
            Setter = new CSPropertySetterCachedObjectId(this);
        }
    }


    public CSPropertyGetter? Getter { get; protected set; }

    public void SetGetter(CSPropertyGetter? getter) => Getter = getter;

    public CSPropertySetter? Setter { get; protected set; }

    public void SetSetter(CSPropertySetter? setter) => Setter = setter;

    public XmlComment? DocComment { get; protected set; }

    public string Name { get; protected set; }

    public IPropertyOrField? CorrespondingPropertyOrField { get; protected set; }

    public CSharpTypeBase Type => GetPropertyType();

    public string? EntityClassName { get; protected set; }

    public ElementVisibilityAbstract Visibility { get; protected set; }

    public bool IsStatic { get; protected set; }

    public string? AdditionalKeywords { get; protected set; }

    public bool SingleLine =>
        (Getter is null || Getter.SingleLineHint) &&
        (Setter is null || Setter.SingleLineHint);

    public virtual string[] GenerateText()
    {
        var text = new List<string>();

        if (DocComment is not null)
        {
            text.AddRange(DocComment.GenerateText());
        }

        var namePart = GenerateNamePart();

        var getterLines = Getter is not null ? Getter.GenerateText() : [string.Empty];
        var setterLines = Setter is not null ? Setter.GenerateText() : [string.Empty];

        if (SingleLine)
        {
            namePart += " {";

            if (Getter != null)
            {
                namePart += " " + getterLines[0];
            }

            if (Setter != null)
            {
                namePart += " " + setterLines[0];
            }

            namePart += " }";

            text.Add(namePart);
        }
        else
        {
            text.Add(namePart);
            text.Add("{");

            if (Getter != null)
            {
                text.AddRange(getterLines);
            }

            if (Setter != null)
            {
                text.AddRange(setterLines);
            }

            text.Add("}");
        }

        for (var i = 0; i < text.Count; i++)
        {
            text[i] = CSharpHelper.C_TAB + text[i];
        }

        return [.. text];
    }
}
