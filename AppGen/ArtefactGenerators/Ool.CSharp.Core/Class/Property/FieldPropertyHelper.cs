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

    public static (
        CSClassField? MainField,
        CSProperty MainProperty,
        CSClassField? FieldForId,
        CSProperty? PropertyForId,
        ICSProperty? PropertyForFilter,
        CSCollectionReadMethod? CollectionReadMethod,
        CSCollectionCountMethod? CollectionCountMethod
    ) GenerateFieldPropertyAndGetter(
        PropertyDefinition propDef,
        CSClass cSharpClass,
        NaturalLanguageEnum preferNaturalLanguageForComments,
        bool supportNullableRefTypes //,
        //out CSClassField? outField,
        //out CSClassField? outFieldId,
        //out CSProperty? outProperty,
        //out CSProperty? outPropertyId,
        //out CSCollectionGetter? outCollectionGetter,
        //out CSCollectionCounter? outCollectionCounter
    )
    {
        //outField = null;
        //outFieldId = null;
        //outProperty = null;
        //outPropertyId = null;
        //outCollectionGetter = null;
        //outCollectionCounter = null;

        CSClassField? mainField = null;
        CSProperty mainProperty;
        CSClassField? fieldForId = null;
        CSProperty? propertyForId = null;
        ICSProperty? propertyForFilter = null;
        CSCollectionReadMethod? collectionReadMethod = null;
        CSCollectionCountMethod? collectionCountMethod = null;

        var (fieldName, fieldIdName) = GetFieldNamesFromPropDef(propDef);

        #region 1.1. Fields and properties for explicit (non-reference) values
        if (!(propDef.FunctionalType is PFTLink))
        {
            mainField = new CSClassField(
                CSharpTypeBase.DeriveFromPropertyFunctionalType(propDef, supportNullableRefTypes),
                fieldName,
                cSharpClass,
                new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names, preferNaturalLanguageForComments)),
                ElementVisibilityClassic.Protected
            );

            mainProperty = new CSProperty(propDef, mainField, mainField.DocComment, false);
        }
        #endregion
        else
        {
            #region 1.2. Fields of Foreign Keys, and their corresponding fields for cached objects
            if (propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTConnector      ||
                propDef.FunctionalType is PFTTableOwner)
            {
                mainField = new CSClassField(
                    CSharpTypeBase.DeriveFromPropertyFunctionalType(propDef, supportNullableRefTypes),
                    fieldName,
                    cSharpClass,
                    new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names, preferNaturalLanguageForComments)),
                    ElementVisibilityClassic.Protected
                );

                fieldForId = new CSClassField(
                    new CSharpValueType(TypeConstants.Value.C_GUID, true),
                    fieldIdName,
                    cSharpClass,
                    new XmlComment((preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Внешний ключ на " : "Foreign Key to ") + NameHelper.GetLocalNameUpperCase(propDef.Names, preferNaturalLanguageForComments)),
                    ElementVisibilityClassic.Protected
                );

                mainProperty = new CSProperty(propDef, mainField, mainField.DocComment, false);
                propertyForId = new CSProperty(propDef, fieldForId, fieldForId.DocComment, true);
            }
            #endregion
            else if (propDef.FunctionalType is PFTTablePart || propDef.FunctionalType is PFTBackReferencedTable)
            #region 1.3. Fields for collections (no tables)
            {
                propertyForFilter = new CSPropertyTableFilter(propDef, preferNaturalLanguageForComments);
                mainProperty = new CSProperty(propDef, (IPropertyOrField)propertyForFilter, null, false);
                collectionReadMethod = new CSCollectionReadMethod(cSharpClass, propDef, preferNaturalLanguageForComments, supportNullableRefTypes);
                collectionCountMethod = new CSCollectionCountMethod(cSharpClass, propDef, preferNaturalLanguageForComments, supportNullableRefTypes);
            }
            #endregion
            else
            {
                throw new GeneratorException($"Not recognized FunctionalType of PropertyDefinition {propDef.Id} {propDef.Names[NaturalLanguageEnum.En]}: {propDef.FunctionalType.GetType().Name}.");
            }
        }

        return (mainField, mainProperty, fieldForId, propertyForId, propertyForFilter, collectionReadMethod, collectionCountMethod);
    }
}
