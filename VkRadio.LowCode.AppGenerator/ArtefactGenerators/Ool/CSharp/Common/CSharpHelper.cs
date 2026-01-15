using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common;

public static class CSharpHelper
{
    public const string C_TAB = "    ";

    public struct ClassNameDOTDefPair
    {
        public string ClassName;
        public DOTDefinition DOTDefinition;

        public static int Compare(ClassNameDOTDefPair in_1, ClassNameDOTDefPair in_2) => string.Compare(in_1.ClassName, in_2.ClassName);
    }

    public struct PropertyWidgetDescriptor
    {
        public PropertyDefinition PropertyDefinition;
        public string PropertyName;
        public string PropertyClass;
        public string WidgetName;
        public string WidgetType;
        public string WidgetCaption;
        public bool CanSelect;
    }

    public static string? StringToValidName(string? @string) => null;

    public static string GenerateMethodKey(CSMethod method)
    {
        var result = method is CSConstructor ? ".ctor" : method.Name;

        if (method.IsStatic || (method.AdditionalKeywords ?? string.Empty).Contains("static"))
        {
            result = "static:" + result;
        }

        if (method.Params.Count != 0)
        {
            result += "::" + string.Join(",", method.Params);
        }

        return result;
    }

    public static string GenerateDOTClassName(DOTDefinition dotDef) => NameHelper.NamesToPascalCase(dotDef.Names);

    /// <summary>
    /// Returns a class name for objects that corresponds to a reference property
    /// </summary>
    /// <param name="propDef">Reference property</param>
    /// <returns></returns>
    public static string? GetPropertyClassName(PropertyDefinition propDef)
    {
        if (!(propDef.FunctionalType is PFTLink))
        {
            return null;
        }

        DOTDefinition? otherEnd = null;

        var pftConn = propDef.FunctionalType as PFTConnector;

        if (pftConn != null)
        {
            otherEnd = pftConn.RelationshipConnector.End1.PropertyDefinition.Id == propDef.Id
                ? pftConn.RelationshipConnector.End2.PropertyDefinition.OwnerDefinition
                : pftConn.RelationshipConnector.End1.PropertyDefinition.OwnerDefinition;
        }
        else
        {
            var pftRefVal = propDef.FunctionalType as PFTReferenceValue;

            if (pftRefVal is not null)
            {
                otherEnd = pftRefVal.RelationshipReference.ReferenceDefinition;
            }
            else
            {
                var pftTblOwner = propDef.FunctionalType as PFTTableOwner;

                if (pftTblOwner is not null)
                {
                    otherEnd = pftTblOwner.RelationshipTable.PropertyDefinitionInOwner.OwnerDefinition;
                }
                else
                {
                    var pftTblPart = propDef.FunctionalType as PFTTablePart;

                    if (pftTblPart is not null)
                    {
                        otherEnd = pftTblPart.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition;
                    }
                    else
                    {
                        var pftBackRefTbl = propDef.FunctionalType as PFTBackReferencedTable;

                        if (pftBackRefTbl is not null)
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

        return NameHelper.NamesToPascalCase(otherEnd.Names);
    }

    /// <summary>
    /// Generate name of a widget for editing a property on a object edition card
    /// </summary>
    /// <param name="propDef"></param>
    /// <returns></returns>
    public static PropertyWidgetDescriptor GenerateWidgetDescForProperty(PropertyDefinition propDef)
    {
        var name = "_" + NameHelper.NamesToPascalCase(propDef.Names);
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
        {
            widgetClassName = "orm.Gui.StringField";
        }
        else if (propWidgetName.IndexOf("SEL_") == 0)
        {
            widgetClassName = "orm.Gui.SelectorField";
        }
        else if (propWidgetName.IndexOf("CHK_") == 0)
        {
            widgetClassName = "System.Windows.Forms.CheckBox";
        }
        else
        {
            throw new GeneratorException($"Unsupported widget type by name prefix: {propWidgetName}.)");
        }

        var propName = NameHelper.NamesToPascalCase(propDef.Names);

        if (isTable)
        {
            propName += "DataTable";
        }

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
    /// Analize a definition of a property and get its parameters for a purpose of constructing a method FillDOFromReader of storage
    /// </summary>
    /// <param name="propDef"></param>
    /// <param name="outIsNullable"></param>
    /// <param name="outIsId"></param>
    /// <param name="outType">C# type</param>
    public static void GetTypeInfoForFillDOFromReaderRow(PropertyDefinition propDef, out bool outIsNullable, out bool outIsId, out string outType)
    {
        outIsId = false;
        outIsNullable = false;
        outType = string.Empty;

        if (!(propDef.FunctionalType is PFTLink))
        {
            outIsNullable = propDef.FunctionalType.Nullable;

            if (propDef.FunctionalType is PFTBoolean)
            {
                outType = "bool" + (outIsNullable ? "?" : string.Empty);
            }
            else if (propDef.FunctionalType is PFTDateTime)
            {
                outType = "DateTime" + (outIsNullable ? "?" : string.Empty);
            }
            else if (propDef.FunctionalType is PFTDecimal)
            {
                outType = "decimal" + (outIsNullable ? "?" : string.Empty);
            }
            else if (propDef.FunctionalType is PFTInteger)
            {
                outType = "int" + (outIsNullable ? "?" : string.Empty);
            }
            else if (propDef.FunctionalType is PFTString)
            {
                outType = "string";
            }
            else if (propDef.FunctionalType is PFTUniqueCode)
            {
                outType = "Guid" + (outIsNullable ? "?" : string.Empty);
            }
            else
            {
                throw new GeneratorException($"Unexpected non-reference PropertyFunctionalType {propDef.FunctionalType.GetType().Name} in PropertyDefinition Id {propDef.Id} for constructing FillDOFromReader.");
            }
        }
        else
        {
            if (propDef.FunctionalType is PFTBackReferencedTable ||
                propDef.FunctionalType is PFTConnector ||
                propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTTableOwner)
            {
                outIsId = true;
                outIsNullable = true;
                outType = "Guid?";
            }
            else
            {
                throw new GeneratorException($"Unexpected reference type: {propDef.FunctionalType.GetType().Name} in PropertyDefinition Id {propDef.Id} when constructing values for FillDOFromReader.");
            }
        }
    }
}
