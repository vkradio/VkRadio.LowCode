using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Type;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Field;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property;

/// <summary>
/// Class for a bulk generation of field and property structures in a class
/// </summary>
public class FieldPropertyHelper
{
    private static void GetFieldNamesFromPropDef(PropertyDefinition propDef, out string out_fieldName, out string out_fieldIdName)
    {
        out_fieldName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
        out_fieldIdName = out_fieldName + "Id";
    }

    public static void GenerateFieldPropertyAndGetter(
        PropertyDefinition propDef,
        CSClass @class,
        out CSClassField? outField,
        out CSClassField? outFieldId,
        out CSProperty? outProperty,
        out CSProperty? outPropertyId,
        out CSCollectionGetter? outCollectionGetter,
        out CSCollectionCounter? outCollectionCounter
    )
    {
        outField = null;
        outFieldId = null;
        outProperty = null;
        outPropertyId = null;
        outCollectionGetter = null;
        outCollectionCounter = null;

        string fieldName, fieldIdName;
        GetFieldNamesFromPropDef(propDef, out fieldName, out fieldIdName);

        #region 1.1. Fields and properties for explicit (non-reference) values
        if (!(propDef.FunctionalType is PFTLink))
        {
            outField = new CSClassField()
            {
                Class = @class,
                Visibility = ElementVisibilityClassic.Protected,
                Name = fieldName,
                DocComment = new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names)),
                TypeKeyword = TypeHelper.PropertyDefinitionToCSType(propDef)
            };

            outProperty = CSProperty.GenerateProperty(@class, propDef, outField, outField.DocComment as XmlComment);
        }
        #endregion
        else
        {
            #region 1.2. Fields of Foreign Keys, and their corresponding fields for cached objects
            if (propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTConnector      ||
                propDef.FunctionalType is PFTTableOwner)
            {
                // Search for corresponding definitions of data object types and tables
                //DOTDefinition dotRef = null;
                //PFTReferenceValue   ftRef       = in_propDef.FunctionalType as PFTReferenceValue;
                //PFTConnector        ftConn      = in_propDef.FunctionalType as PFTConnector;
                //PFTTableOwner       ftTblOwner  = in_propDef.FunctionalType as PFTTableOwner;
                //if (ftRef != null)
                //    dotRef = ftRef.RelationshipReference.ReferenceDefinition;
                //else if (ftConn != null)
                //    dotRef = ftConn.RelationshipConnector.End1.PropertyDefinition.Id == in_propDef.Id ? ftConn.RelationshipConnector.End2.PropertyDefinition.OwnerDefinition : ftConn.RelationshipConnector.End1.PropertyDefinition.OwnerDefinition;
                //else if (ftTblOwner != null)
                //    dotRef = ftTblOwner.RelationshipTable.PropertyDefinitionInOwner.OwnerDefinition;
                //else
                //    throw new ApplicationException(string.Format("Invalid PropertyDefinition reference FunctionalType: {0}.", in_propDef.FunctionalType.GetType().Name));

                outField = new CSClassField
                {
                    Class = @class,
                    Visibility = ElementVisibilityClassic.Protected,
                    Name = fieldName,
                    DocComment = new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names)),
                    TypeKeyword = TypeHelper.PropertyDefinitionToCSType(propDef)
                };

                outFieldId = new CSClassField()
                {
                    Class = @class,
                    Visibility = ElementVisibilityClassic.Protected,
                    Name = fieldIdName,
                    DocComment = new XmlComment("Foreign Key to " + NameHelper.GetLocalNameUpperCase(propDef.Names)),
                    TypeKeyword = "Guid?"
                };

                outProperty = CSProperty.GenerateProperty(@class, propDef, outField, outField.DocComment as XmlComment);
                outPropertyId = CSProperty.GeneratePropertyId(@class, propDef, outFieldId, outFieldId.DocComment as XmlComment);
            }
            #endregion
            else if (propDef.FunctionalType is PFTTablePart || propDef.FunctionalType is PFTBackReferencedTable)
            #region 1.3. Fields for collections (no tables)
            {
                outProperty = CSProperty.GeneratePropertyCollection(@class, propDef);
                outCollectionGetter = CSCollectionGetter.Instantiate(@class, propDef);
                outCollectionCounter = CSCollectionCounter.Instantiate(@class, propDef);
            }
            #endregion
            else
            {
                throw new GeneratorException($"Not recognized FunctionalType of PropertyDefinition {propDef.Id} {propDef.Names[HumanLanguageEnum.En]}: {propDef.FunctionalType.GetType().Name}.");
            }
        }
    }
}
