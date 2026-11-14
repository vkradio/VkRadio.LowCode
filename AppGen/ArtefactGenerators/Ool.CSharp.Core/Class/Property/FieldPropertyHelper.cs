using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Field;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;

/// <summary>
/// Class for a bulk generation of field and property structures in a class
/// </summary>
public class FieldPropertyHelper
{
    private static (string FieldName, string FieldIdName) GetFieldNamesFromPropDef(PropertyDefinition propDef)
    {
        var fieldName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
        return (fieldName, fieldName + "Id");
    }

    public static (List<CSClassField> fields, List<ICSProperty> properties, List<CSMethod> methods) GenerateFieldPropertyAndGetter(
        PropertyDefinition propDef,
        CSClass cSharpClass,
        NaturalLanguageEnum preferNaturalLanguageForComments,
        bool supportNullableRefTypes
    )
    {
        var fields = new List<CSClassField>();
        var properties = new List<ICSProperty>();
        var methods = new List<CSMethod>();

        var (fieldName, fieldIdName) = GetFieldNamesFromPropDef(propDef);

        #region 1.1. Fields and properties for explicit (non-reference) values
        if (!(propDef.FunctionalType is PFTLink))
        {
            var mainField = new CSClassField(
                CSharpTypeBase.DeriveFromPropertyFunctionalType(propDef, supportNullableRefTypes),
                fieldName,
                cSharpClass,
                new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names, preferNaturalLanguageForComments)),
                ElementVisibilityClassic.Protected
            );
            fields.Add(mainField);

            var mainProperty = new CSProperty(propDef, mainField, mainField.DocComment, false);
            properties.Add(mainProperty);
        }
        #endregion
        else
        {
            #region 1.2. Fields of Foreign Keys, and their corresponding fields for cached objects
            if (propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTConnector      ||
                propDef.FunctionalType is PFTTableOwner)
            {
                var mainField = new CSClassField(
                    CSharpTypeBase.DeriveFromPropertyFunctionalType(propDef, supportNullableRefTypes),
                    fieldName,
                    cSharpClass,
                    new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names, preferNaturalLanguageForComments)),
                    ElementVisibilityClassic.Protected
                );
                fields.Add(mainField);

                var fieldForId = new CSClassField(
                    new CSharpValueType(TypeConstants.Value.C_GUID, true),
                    fieldIdName,
                    cSharpClass,
                    new XmlComment((preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Внешний ключ на " : "Foreign Key to ") + NameHelper.GetLocalNameUpperCase(propDef.Names, preferNaturalLanguageForComments)),
                    ElementVisibilityClassic.Protected
                );
                fields.Add(fieldForId);

                var mainProperty = new CSProperty(propDef, mainField, mainField.DocComment, false);
                properties.Add(mainProperty);

                var propertyForId = new CSProperty(propDef, fieldForId, fieldForId.DocComment, true);
                properties.Add(propertyForId);
            }
            #endregion
            else if (propDef.FunctionalType is PFTTablePart || propDef.FunctionalType is PFTBackReferencedTable)
            #region 1.3. Fields for collections (no tables)
            {
                var propertyForFilter = new CSPropertyTableFilter(propDef, preferNaturalLanguageForComments);
                properties.Add(propertyForFilter);

                var propertyForDataTable = new CSPropertyDataTable(propDef, preferNaturalLanguageForComments);
                properties.Add(propertyForDataTable);

                var collectionReadMethod = new CSCollectionReadMethod(cSharpClass, propDef, preferNaturalLanguageForComments, supportNullableRefTypes);
                methods.Add(collectionReadMethod);

                var collectionCountMethod = new CSCollectionCountMethod(cSharpClass, propDef, preferNaturalLanguageForComments, supportNullableRefTypes);
                methods.Add(collectionCountMethod);
            }
            #endregion
            else
            {
                throw new GeneratorException($"Not recognized FunctionalType of PropertyDefinition {propDef.Id} {propDef.Names[NaturalLanguageEnum.En]}: {propDef.FunctionalType.GetType().Name}.");
            }
        }

        return (fields, properties, methods);
    }
}
