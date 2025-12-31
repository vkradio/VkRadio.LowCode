using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Type;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Field;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using MetaModel.Names;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property
{
    /// <summary>
    /// Класс для пакетной генерации структуры полей и свойств класса
    /// </summary>
    public class FieldPropertyHelper
    {
        static void GetFieldNamesFromPropDef(PropertyDefinition propDef, out string out_fieldName, out string out_fieldIdName)
        {
            out_fieldName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
            out_fieldIdName = out_fieldName + "Id";
        }

        public static void GenerateFieldPropertyAndGetter(PropertyDefinition propDef, CSClass @class, out CSClassField out_field, out CSClassField out_fieldId, out CSProperty out_property, out CSProperty out_propertyId, out CSCollectionGetter out_collectionGetter, out CSCollectionCounter out_collectionCounter)
        {
            out_field = null;
            out_fieldId = null;
            out_property = null;
            out_propertyId = null;
            out_collectionGetter = null;
            out_collectionCounter = null;

            string fieldName, fieldIdName;
            GetFieldNamesFromPropDef(propDef, out fieldName, out fieldIdName);

            #region 1.1. Поля и свойства непосредственных значений.
            if (!(propDef.FunctionalType is PFTLink))
            {
                out_field = new CSClassField()
                {
                    Class = @class,
                    Visibility = ElementVisibilityClassic.Protected,
                    Name = fieldName,
                    DocComment = new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names)),
                    TypeKeyword = TypeHelper.PropertyDefinitionToCSType(propDef)
                };

                out_property = CSProperty.GenerateProperty(@class, propDef, out_field, out_field.DocComment as XmlComment);
            }
            #endregion
            else
            {
                #region 1.2. Поля ВК и соответствующие им поля кешированных объектов.
                if (propDef.FunctionalType is PFTReferenceValue ||
                    propDef.FunctionalType is PFTConnector      ||
                    propDef.FunctionalType is PFTTableOwner)
                {
                    // Ищем соответствующие ссылке определения ТОД и таблицы.
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

                    out_field = new CSClassField()
                    {
                        Class = @class,
                        Visibility = ElementVisibilityClassic.Protected,
                        Name = fieldName,
                        DocComment = new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names)),
                        TypeKeyword = TypeHelper.PropertyDefinitionToCSType(propDef)
                    };

                    out_fieldId = new CSClassField()
                    {
                        Class = @class,
                        Visibility = ElementVisibilityClassic.Protected,
                        Name = fieldIdName,
                        DocComment = new XmlComment("Внешний ключ на " + NameHelper.GetLocalNameUpperCase(propDef.Names)),
                        TypeKeyword = "Guid?"
                    };

                    out_property = CSProperty.GenerateProperty(@class, propDef, out_field, out_field.DocComment as XmlComment);
                    out_propertyId = CSProperty.GeneratePropertyId(@class, propDef, out_fieldId, out_fieldId.DocComment as XmlComment);
                }
                #endregion
                else if (propDef.FunctionalType is PFTTablePart || propDef.FunctionalType is PFTBackReferencedTable)
                #region 1.3. Поля коллекций (в таблице отсутствуют).
                {
                    out_property = CSProperty.GeneratePropertyCollection(@class, propDef);
                    out_collectionGetter = CSCollectionGetter.Instantiate(@class, propDef);
                    out_collectionCounter = CSCollectionCounter.Instantiate(@class, propDef);
                }
                #endregion
                else
                {
                    throw new GeneratorException($"Not recognized FunctionalType of PropertyDefinition {propDef.Id} {propDef.Names[HumanLanguageEnum.En]}: {propDef.FunctionalType.GetType().Name}.");
                }
            }
        }
    };
}
