using System;

using MetaModel.DOTDefinition;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Type
{
    /// <summary>
    /// Работа с типами данных C#
    /// </summary>
    public class TypeHelper
    {
        /// <summary>
        /// Получение ключевого слова типа C# или имени пользовательского типа
        /// из определения свойства метаданных
        /// </summary>
        /// <param name="propDef">Определение свойства</param>
        /// <returns></returns>
        public static string PropertyDefinitionToCSType(PropertyDefinition propDef)
        {
            string result;

            if (!(propDef.FunctionalType is PFTLink))
            {
                if (propDef.FunctionalType is PFTBoolean)
                    result = "bool";
                else if (propDef.FunctionalType is PFTDateTime)
                    result = "DateTime";
                else if (propDef.FunctionalType is PFTDecimal)
                    result = "decimal";
                else if (propDef.FunctionalType is PFTInteger) // TODO: Здесь мы не учитываем предельные величины целых чисел
                    result = "int";
                else if (propDef.FunctionalType is PFTString)
                    result = "string";
                else if (propDef.FunctionalType is PFTUniqueCode)
                    result = "Guid";
                else
                    throw new GeneratorException($"Unknown value type for property Id {propDef.Id}: {propDef.FunctionalType.GetType().Name}.");

                if (propDef.FunctionalType.Nullable && !(propDef.FunctionalType is PFTString))
                    result += "?";
            }
            else
            {
                var pftBackRefTable = propDef.FunctionalType as PFTBackReferencedTable;
                if (pftBackRefTable != null)
                {
                    //string className = GetClassNameForDOT(((PropertyDefinition)pftBackRefTable.RelationshipReference.OwnerPropertyDefinition).OwnerDefinition);
                    result = "DataTable";
                }
                else
                {
                    var pftConn = propDef.FunctionalType as PFTConnector;
                    if (pftConn != null)
                    {
                        var end = pftConn.RelationshipConnector.End1.PropertyDefinition.Id == propDef.Id ?
                            pftConn.RelationshipConnector.End2 :
                            pftConn.RelationshipConnector.End1;
                        result = GetClassNameForDOT(end.PropertyDefinition.OwnerDefinition);
                    }
                    else
                    {
                        var pftRefVal = propDef.FunctionalType as PFTReferenceValue;
                        if (pftRefVal != null)
                        {
                            result = GetClassNameForDOT(pftRefVal.RelationshipReference.ReferenceDefinition);
                        }
                        else
                        {
                            var pftTableOwner = propDef.FunctionalType as PFTTableOwner;
                            if (pftTableOwner != null)
                            {
                                result = GetClassNameForDOT(pftTableOwner.RelationshipTable.PropertyDefinitionInOwner.OwnerDefinition);
                            }
                            else
                            {
                                var pftTable = propDef.FunctionalType as PFTTablePart;
                                if (pftTable != null)
                                {
                                    //string className = GetClassNameForDOT(pftTable.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition);
                                    //result = string.Format("IDictionary<Guid, {0}>", className);
                                    result = "DataTable";
                                }
                                else
                                {
                                    throw new GeneratorException($"Unknown reference type for property Id {propDef.Id}: {propDef.FunctionalType.GetType().Name}.");
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Получение имени пользовательского класса ТОД C#, соответствующего определению
        /// свойства метаданных ссылочного типа
        /// </summary>
        /// <param name="propDef">Определение свойства</param>
        /// <returns>Имя класса</returns>
        public static string PropertyDefinitionToCSDOTType(PropertyDefinition propDef)
        {
            string result;

            if (!(propDef.FunctionalType is PFTLink))
            {
                throw new ArgumentException($"Cannot define C# class for non-reference PropertyDefinition Id {propDef.Id}, FunctionalType = {propDef.FunctionalType.GetType().Name}.");
            }
            else
            {
                if (propDef.FunctionalType is PFTBackReferencedTable ||
                    propDef.FunctionalType is PFTTablePart)
                {
                    var pftBackRefTable = propDef.FunctionalType as PFTBackReferencedTable;
                    if (pftBackRefTable != null)
                    {
                        result = GetClassNameForDOT(((PropertyDefinition)pftBackRefTable.RelationshipReference.OwnerPropertyDefinition).OwnerDefinition);
                    }
                    else
                    {
                        var pftTable = propDef.FunctionalType as PFTTablePart;
                        result = GetClassNameForDOT(pftTable.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition);
                    }
                }
                else
                {
                    result = PropertyDefinitionToCSType(propDef);
                }
            }

            return result;
        }
        /// <summary>
        /// Получение XmlComment для определения свойства табличного типа (коллекции)
        /// </summary>
        /// <param name="propDef">Определение свойства табличного типа</param>
        /// <returns>XmlComment</returns>
        public static XmlComment GetXmlCommentForTablePropDef(PropertyDefinition propDef) // TODO: Вынести это в обобщенный генератор XmlComment или DocComment
        {
            XmlComment result;

            if (!(propDef.FunctionalType is PFTLink))
            {
                throw new NotImplementedException($"Cannot define XmlComment for non-reference PropertyDefinition Id {propDef.Id}, FunctionalType = {propDef.FunctionalType.GetType().Name}.");
            }
            else
            {
                if (propDef.FunctionalType is PFTBackReferencedTable ||
                    propDef.FunctionalType is PFTTablePart)
                {
                    var pftBackRefTable = propDef.FunctionalType as PFTBackReferencedTable;
                    if (pftBackRefTable != null)
                    {
                        //result = new XmlComment(c_tableOfObjects + NameHelper.GetLocalNameUpperCase(((PropertyDefinition)pftBackRefTable.RelationshipReference.OwnerPropertyDefinition).OwnerDefinition.Names));
                        result = new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names));
                    }
                    else
                    {
                        //PFTTablePart pftTable = in_propDef.FunctionalType as PFTTablePart;
                        //result = new XmlComment(c_tableOfObjects + NameHelper.GetLocalNameUpperCase(pftTable.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition.Names));
                        result = new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names));
                    }
                }
                else
                {
                    throw new NotImplementedException($"Cannot define XmlComment for non-table PropertyDefinition Id {propDef.Id}, FunctionalType = {propDef.FunctionalType.GetType().Name}.");
                }
            }

            return result;
        }

        /// <summary>
        /// Получение имени класса для ТОД
        /// </summary>
        /// <param name="dotDef">Определение ТОД</param>
        /// <returns>Имя класса ТОД</returns>
        public static string GetClassNameForDOT(DOTDefinition dotDef) =>
            NameHelper.NamesToHungarianName(dotDef.Names);
    };
}
