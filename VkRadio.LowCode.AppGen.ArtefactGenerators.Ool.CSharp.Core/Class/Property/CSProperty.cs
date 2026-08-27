using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Field;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Getter;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Setter;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;

public class CSProperty
{
    protected const string c_tab = "    ";

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

        return $"public{additionalKeywords} {Type} {Name}";
    }

    /// <summary>
    /// Build a property based on a property definition
    /// </summary>
    /// <param name="cSharpClass"></param>
    /// <param name="propDef"></param>
    /// <param name="correspondingField"></param>
    /// <param name="docComment"></param>
    public static CSProperty GenerateProperty(CSClass cSharpClass, PropertyDefinition propDef, CSClassField correspondingField, XmlComment docComment)
    {
        CSProperty? result = null;

        if (!(propDef.FunctionalType is PFTLink))
        {
            result = new CSProperty()
            {
                Class = cSharpClass,
                DocComment = docComment,
                Name = NameHelper.NamesToPascalCase(propDef.Names, true),
                NameFieldCorresponding = correspondingField.Name,
                Type = correspondingField.TypeKeyword
            };

            result.Getter = new CSPropertyGetter(result);
            result.Setter = new CSPropertySetter(result);
        }
        else
        {
            if (propDef.FunctionalType is PFTConnector ||
                propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTTableOwner)
            {
                result = new CSProperty
                {
                    Class = cSharpClass,
                    DocComment = docComment,
                    Name = NameHelper.NamesToPascalCase(propDef.Names, true),
                    NameFieldCorresponding = correspondingField.Name,
                    Type = correspondingField.TypeKeyword
                };
                result.Getter = new CSPropertyGetterCachedObject(result);
                result.Setter = new CSPropertySetterCachedObject(result);
            }
        }

        return result;
    }
    /// <summary>
    /// Build an id-property, corresponding to a reference property, based on a property definition
    /// </summary>
    /// <param name="cSharpClass"></param>
    /// <param name="propDef"></param>
    /// <param name="correspondingField"></param>
    /// <param name="docComment"></param>
    public static CSProperty GeneratePropertyId(CSClass cSharpClass, PropertyDefinition propDef, CSClassField correspondingField, XmlComment docComment)
    {
        if (!(propDef.FunctionalType is PFTConnector || propDef.FunctionalType is PFTReferenceValue || propDef.FunctionalType is PFTTableOwner))
        {
            throw new ArgumentException($"in_propDef.FunctionalType is not reference value ({propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {propDef.Id}).");
        }

        var result = new CSProperty
        {
            Class = cSharpClass,
            DocComment = docComment,
            Name = NameHelper.NamesToPascalCase(propDef.Names, true) + "Id",
            NameFieldCorresponding = correspondingField.Name,
            Type = correspondingField.TypeKeyword
        };

        result.Getter = new CSPropertyGetter(result);
        result.Setter = new CSPropertySetterCachedObjectId(result);

        return result;
    }

    /// <summary>
    /// Build a collection property based on a property definition
    /// </summary>
    /// <param name="cSharpClass"></param>
    /// <param name="propDef"></param>
    public static CSProperty GeneratePropertyCollection(CSClass cSharpClass, PropertyDefinition propDef)
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
                throw new GeneratorException($"Unsupported PropertyFunctionalType ({propDef.FunctionalType.GetType().Name}) for collection property Id {propDef.Id}.");
            }
        }
        #endregion

        var filterProp = new CSPropertyPredefined
        {
            Class = cSharpClass,
            DocComment = new XmlComment("Filter for the property " + NameHelper.GetLocalNameLowerCase(propDef.Names)),
            Name = filterPropName,
            PredefinedValue = $"public virtual FilterSimple {filterPropName} {{ get {{ return FilterSimple.CreateTableFilter(id, \"{fieldName}\"); }} }}"
        };
        cSharpClass.Properties.Add(filterProp.Name, filterProp);

        var result = new CSProperty
        {
            Class = cSharpClass,
            DocComment = TypeHelper.GetXmlCommentForTablePropDef(propDef),
            Name = NameHelper.NamesToPascalCase(propDef.Names, true) + "DataTable",
            Type = TypeHelper.PropertyDefinitionToCSType(propDef),
            DOTType = TypeHelper.PropertyDefinitionToCSEntityType(propDef),
            AdditionalKeywords = "virtual"
        };
        result.DocComment.Text += " (read table)";
        result.Getter = new CSPropertyGetterCollection(result, propDef);

        return result;
    }

    public CSClass Class { get; set; }

    public CSPropertyGetter Getter { get; set; }

    public CSPropertySetter Setter { get; set; }

    public AbstractDocComment DocComment { get; set; }

    public string Name { get; set; }

    public string NameFieldCorresponding { get; set; }

    public string Type { get; set; }

    public string DOTType { get; set; }

    public ElementVisibilityAbstract Visibility { get; set; }

    public bool IsStatic { get; set; }

    public string AdditionalKeywords { get; set; }

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
            text[i] = c_tab + text[i];
        }

        return [.. text];
    }
}
