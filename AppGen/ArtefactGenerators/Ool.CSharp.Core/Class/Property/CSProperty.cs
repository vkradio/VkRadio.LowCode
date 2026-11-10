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
        //Type = Guard.Against.Null(type, nameof(type));
        //Class = Guard.Against.Null(cSharpClass, nameof(cSharpClass));
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
    /// <param name="cSharpClass"></param>
    /// <param name="propDef"></param>
    /// <param name="correspondingPropertyOrField"></param>
    /// <param name="docComment"></param>
    /// <param name="modificationForIdField">Variant of property for getting/setting id instead of a reference of an object</param>
    //public static CSProperty GenerateProperty(CSClass cSharpClass, PropertyDefinition propDef, CSClassField correspondingField, XmlComment docComment)
    public CSProperty(/*CSClass cSharpClass,*/ PropertyDefinition propDef, IPropertyOrField correspondingPropertyOrField, XmlComment? docComment, bool modificationForIdField)
        : this(
            NameHelper.NamesToPascalCase(propDef.Names, true) + (modificationForIdField ? "Id" : string.Empty),
            Guard.Against.Null(correspondingPropertyOrField, nameof(correspondingPropertyOrField)),
            //cSharpClass,
            null,
            ElementVisibilityClassic.Public,
            false,
            null,
            null,
            docComment,
            null
        )
    {
        //Guard.Against.Null(cSharpClass, nameof(cSharpClass));
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

    /*
    /// <summary>
    /// Build a collection property based on a property definition
    /// </summary>
    /// <param name="cSharpClass"></param>
    /// <param name="propDef"></param>
    public static CSProperty GeneratePropertyCollection(CSClass cSharpClass, PropertyDefinition propDef, IPropertyOrField correspondingField, NaturalLanguageEnum preferNaturalLanguageForComments)
    {
        if (!(propDef.FunctionalType is PFTBackReferencedTable || propDef.FunctionalType is PFTTablePart))
        {
            throw new ArgumentException($"in_propDef.FunctionalType is not table/collection ({propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {propDef.Id}).");
        }

        var filterPropName = NameHelper.NamesToPascalCase(propDef.Names, true) + "Filter";

        #region Extract a property name
        string fieldName;
        var refTable = propDef.FunctionalType as PFTBackReferencedTable;

        if (refTable is not null)
        {
            fieldName = NameHelper.NameToUnderscoreSeparatedName(refTable.RelationshipReference.OwnerPropertyDefinition.Names) + "_id";
        }
        else
        {
            if (propDef.FunctionalType is PFTTablePart tblPart)
            {
                fieldName = NameHelper.NameToUnderscoreSeparatedName(tblPart.RelationshipTable.PropertyDefinitionInTable.Names) + "_id";
            }
            else
            {
                throw new GeneratorException($"Unsupported {nameof(PropertyFunctionalType)} ({propDef.FunctionalType.GetType().Name}) for collection property Id {propDef.Id}.");
            }
        }
        #endregion

        var filterProp = new CSPropertyPredefined(
            filterPropName,
            $"public virtual FilterSimple {filterPropName} {{ get {{ return FilterSimple.CreateTableFilter(_id, \"{fieldName}\", VkRadio.LowCode.Libs.Orm.DbProviderFactory.Default); }} }}",
            new XmlComment((preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Фильтр для свойства " : "Filter for the property ") + NameHelper.GetLocalNameLowerCase(propDef.Names, preferNaturalLanguageForComments))
        );
        cSharpClass.Properties.Add(filterProp.Name, filterProp);

        var result = new CSProperty(
            NameHelper.NamesToPascalCase(propDef.Names, true) + "DataTable",
            correspondingField,
            //cSharpClass,
            TypeHelper.PropertyDefinitionToCSEntityType(propDef),
            ElementVisibilityClassic.Public,
            false,
            null,
            null,
            TypeHelper.GetXmlCommentForTablePropDef(propDef, preferNaturalLanguageForComments),
            "virtual"
        );
        result.DocComment!.Text += preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? " (чтение таблицы)" : " (read table)";
        result.Getter = new CSPropertyGetterCollection(result, propDef);

        return result;
    }
    */

    //public CSClass Class { get; protected set; }

    public CSPropertyGetter? Getter { get; protected set; }

    public void SetGetter(CSPropertyGetter? getter) => Getter = getter;

    public CSPropertySetter? Setter { get; protected set; }

    public void SetSetter(CSPropertySetter? setter) => Setter = setter;

    public XmlComment? DocComment { get; protected set; }

    public string Name { get; protected set; }

    public IPropertyOrField? CorrespondingPropertyOrField { get; protected set; }

    //public string NameFieldCorresponding { get; set; }

    public CSharpTypeBase Type => GetPropertyType();

    public string? EntityClassName { get; protected set; }

    //public string DOTType { get; set; }

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
