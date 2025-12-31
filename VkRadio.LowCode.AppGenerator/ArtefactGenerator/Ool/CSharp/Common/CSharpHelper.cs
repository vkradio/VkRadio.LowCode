using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using MetaModel.DOTDefinition;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common
{
    public static class CSharpHelper
    {
        public const string C_TAB = "    ";

        public struct ClassNameDOTDefPair
        {
            public string ClassName;
            public DOTDefinition DOTDefinition;

            public static int Compare(ClassNameDOTDefPair in_1, ClassNameDOTDefPair in_2) =>
                string.Compare(in_1.ClassName, in_2.ClassName);
        };
        public struct PropertyWidgetDescriptor
        {
            public PropertyDefinition PropertyDefinition;
            public string PropertyName;
            public string PropertyClass;
            public string WidgetName;
            public string WidgetType;
            public string WidgetCaption;
            public bool CanSelect;
        };

        public static string StringToValidName(string @string) => null;

        public static string GenerateMethodKey(CSMethod method)
        {
            var result = method is CSConstructor ? ".ctor" : method.Name;

            if (method.IsStatic || (method.AdditionalKeywords ?? string.Empty).Contains("static"))
                result = "static:" + result;

            if (method.Params.Count != 0)
                result += "::" + string.Join(",", method.Params);

            return result;
        }

        public static string GenerateDOTClassName(DOTDefinition dotDef) =>
            NameHelper.NamesToHungarianName(dotDef.Names);

        /// <summary>
        /// Возвращает имя класса для объектов, соответствующих ссылочному свойству
        /// </summary>
        /// <param name="propDef">Ссылочное свойство</param>
        /// <returns>Имя класса для объектов ссылочного свойства</returns>
        public static string GetPropertyClassName(PropertyDefinition propDef)
        {
            if (!(propDef.FunctionalType is PFTLink))
                return null;

            DOTDefinition otherEnd = null;

            var pftConn = propDef.FunctionalType as PFTConnector;
            if (pftConn != null)
            {
                otherEnd = pftConn.RelationshipConnector.End1.PropertyDefinition.Id == propDef.Id ?
                    pftConn.RelationshipConnector.End2.PropertyDefinition.OwnerDefinition :
                    pftConn.RelationshipConnector.End1.PropertyDefinition.OwnerDefinition;
            }
            else
            {
                var pftRefVal = propDef.FunctionalType as PFTReferenceValue;
                if (pftRefVal != null)
                {
                    otherEnd = pftRefVal.RelationshipReference.ReferenceDefinition;
                }
                else
                {
                    var pftTblOwner = propDef.FunctionalType as PFTTableOwner;
                    if (pftTblOwner != null)
                    {
                        otherEnd = pftTblOwner.RelationshipTable.PropertyDefinitionInOwner.OwnerDefinition;
                    }
                    else
                    {
                        var pftTblPart = propDef.FunctionalType as PFTTablePart;
                        if (pftTblPart != null)
                        {
                            otherEnd = pftTblPart.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition;
                        }
                        else
                        {
                            var pftBackRefTbl = propDef.FunctionalType as PFTBackReferencedTable;
                            if (pftBackRefTbl != null)
                            {
                                otherEnd = ((PropertyDefinition)pftBackRefTbl.RelationshipReference.OwnerPropertyDefinition).OwnerDefinition;
                            }
                            else
                            {
                                throw new GeneratorException($"Unsupported reference property type {propDef.FunctionalType.GetType().Name} in PropertyDefinition Id {propDef.Id}.");
                            }
                        }
                    }
                }
            }

            return NameHelper.NamesToHungarianName(otherEnd.Names);
        }

        /// <summary>
        /// Генерирование имени элемента управления для редактирования свойства на
        /// панели редактирования объекта
        /// </summary>
        /// <param name="propDef">Определение свойства</param>
        /// <returns>Имя элемента управления</returns>
        public static PropertyWidgetDescriptor GenerateWidgetDescForProperty(PropertyDefinition propDef)
        {
            var name = "_" + NameHelper.NamesToHungarianName(propDef.Names);
            var prefix = string.Empty;
            var caption = NameHelper.GetLocalNameUpperCase(propDef.Names);
            var canSelect = false;
            var isTable = false;

            if (propDef.FunctionalType is PFTLink)
            {
                prefix = "SEL";
                if (!(propDef.FunctionalType is PFTBackReferencedTable ||
                    propDef.FunctionalType is PFTTablePart))
                {
                    canSelect = true;
                }
                else
                {
                    isTable = true;
                }
            }
            else if (propDef.FunctionalType is PFTBoolean)
            {
                prefix = "CHK";
            }
            else if (propDef.FunctionalType is PFTDate || propDef.FunctionalType is PFTDateAndTime)
            {
                //prefix = "D";
                prefix = "SF";
            }
            else if (propDef.FunctionalType is PFTDecimal    ||
                propDef.FunctionalType is PFTTime            ||
                propDef.FunctionalType is PFTInteger         ||
                propDef.FunctionalType is PFTString          ||
                propDef.FunctionalType is PFTUniqueCode)
            {
                //prefix = "T";
                prefix = "SF";
            }
            else
            {
                throw new GeneratorException($"Unknown PropertyFunctionalType {propDef.FunctionalType.GetType().Name} in PropertyDefinition Id {propDef.Id} when generating widget name.");
            }

            var propWidgetName = prefix + name;

            var widgetClassName = string.Empty;
            if (propWidgetName.IndexOf("SF_") == 0)
                widgetClassName = "orm.Gui.StringField";
            else if (propWidgetName.IndexOf("SEL_") == 0)
                widgetClassName = "orm.Gui.SelectorField";
            else if (propWidgetName.IndexOf("CHK_") == 0)
                widgetClassName = "System.Windows.Forms.CheckBox";
            else
                throw new GeneratorException($"Unsupported widget type by name prefix: {propWidgetName}.)");

            var propName = NameHelper.NamesToHungarianName(propDef.Names);
            if (isTable)
                propName += "DataTable";
            var desc = new PropertyWidgetDescriptor
            {
                PropertyDefinition = propDef,
                PropertyClass = GetPropertyClassName(propDef),
                WidgetName = propWidgetName,
                WidgetType = widgetClassName,
                WidgetCaption = caption,
                CanSelect = canSelect,
                PropertyName = propName
            };

            return desc;
        }

        /// <summary>
        /// Анализ определения свойства и выдача его параметров для целей
        /// конструирования метода хранилища FillDOFromReader.
        /// </summary>
        /// <param name="propDef">Определение анализируемого свойства</param>
        /// <param name="out_isNullable">Является ли nullable</param>
        /// <param name="out_isId">является ли Id</param>
        /// <param name="out_type">тип C#</param>
        public static void GetTypeInfoForFillDOFromReaderRow(PropertyDefinition propDef, out bool out_isNullable, out bool out_isId, out string out_type)
        {
            out_isId = false;
            out_isNullable = false;
            out_type = string.Empty;

            if (!(propDef.FunctionalType is PFTLink))
            {
                out_isNullable = propDef.FunctionalType.Nullable;

                if (propDef.FunctionalType is PFTBoolean)
                    out_type = "bool" + (out_isNullable ? "?" : string.Empty);
                else if (propDef.FunctionalType is PFTDateTime)
                    out_type = "DateTime" + (out_isNullable ? "?" : string.Empty);
                else if (propDef.FunctionalType is PFTDecimal)
                    out_type = "decimal" + (out_isNullable ? "?" : string.Empty);
                else if (propDef.FunctionalType is PFTInteger)
                    out_type = "int" + (out_isNullable ? "?" : string.Empty);
                else if (propDef.FunctionalType is PFTString)
                    out_type = "string";
                else if (propDef.FunctionalType is PFTUniqueCode)
                    out_type = "Guid" + (out_isNullable ? "?" : string.Empty);
                else
                    throw new GeneratorException($"Unexpected non-reference PropertyFunctionalType {propDef.FunctionalType.GetType().Name} in PropertyDefinition Id {propDef.Id} for constructing FillDOFromReader.");
            }
            else
            {
                if (propDef.FunctionalType is PFTBackReferencedTable ||
                    propDef.FunctionalType is PFTConnector ||
                    propDef.FunctionalType is PFTReferenceValue ||
                    propDef.FunctionalType is PFTTableOwner)
                {
                    out_isId = true;
                    out_isNullable = true;
                    out_type = "Guid?";
                }
                else
                {
                    throw new GeneratorException($"Unexpected reference type: {propDef.FunctionalType.GetType().Name} in PropertyDefinition Id {propDef.Id} when constructing values for FillDOFromReader.");
                }
            }
        }
    };
}
