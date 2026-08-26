using VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Type;

/// <summary>
/// Work with C# data types
/// </summary>
public class TypeHelper
{
    /// <summary>
    /// Get a C# type keyword or name of user defined type from a MetaData definition
    /// </summary>
    /// <param name="propDef">Property definition</param>
    /// <returns></returns>
    public static string PropertyDefinitionToCSType(PropertyDefinition propDef)
    {
        string result;

        if (!(propDef.FunctionalType is PFTLink))
        {
            if (propDef.FunctionalType is PFTBoolean)
            {
                result = "bool";
            }
            else if (propDef.FunctionalType is PFTDateTime)
            {
                result = "DateTime";
            }
            else if (propDef.FunctionalType is PFTDecimal)
            {
                result = "decimal";
            }
            else if (propDef.FunctionalType is PFTInteger) // TODO: Here we do not consider a size of int
            {
                result = "int";
            }
            else if (propDef.FunctionalType is PFTString)
            {
                result = "string";
            }
            else if (propDef.FunctionalType is PFTUniqueCode)
            {
                result = "Guid";
            }
            else
            {
                throw new GeneratorException($"Unknown value type for property Id {propDef.Id}: {propDef.FunctionalType.GetType().Name}.");
            }

            if (propDef.FunctionalType.Nullable && !(propDef.FunctionalType is PFTString))
            {
                result += "?";
            }
        }
        else
        {
            var pftBackRefTable = propDef.FunctionalType as PFTBackReferencedTable;

            if (pftBackRefTable is not null)
            {
                //var className = GetClassNameForDOT(((PropertyDefinition)pftBackRefTable.RelationshipReference.OwnerPropertyDefinition).OwnerDefinition);
                result = "DataTable";
            }
            else
            {
                var pftConn = propDef.FunctionalType as PFTConnector;

                if (pftConn is not null)
                {
                    var end = pftConn.RelationshipConnector.End1.PropertyDefinition.Id == propDef.Id
                        ? pftConn.RelationshipConnector.End2
                        : pftConn.RelationshipConnector.End1;

                    result = GetClassNameForDOT(end.PropertyDefinition.OwnerDefinition);
                }
                else
                {
                    var pftRefVal = propDef.FunctionalType as PFTReferenceValue;

                    if (pftRefVal is not null)
                    {
                        result = GetClassNameForDOT(pftRefVal.RelationshipReference.ReferenceDefinition);
                    }
                    else
                    {
                        var pftTableOwner = propDef.FunctionalType as PFTTableOwner;

                        if (pftTableOwner is not null)
                        {
                            result = GetClassNameForDOT(pftTableOwner.RelationshipTable.PropertyDefinitionInOwner.OwnerDefinition);
                        }
                        else
                        {
                            var pftTable = propDef.FunctionalType as PFTTablePart;

                            if (pftTable is not null)
                            {
                                //var className = GetClassNameForDOT(pftTable.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition);
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
    /// Get a C# name for a user class of data object type, that corresponds for metadata property of reference type
    /// </summary>
    /// <param name="propDef"></param>
    /// <returns>Class name</returns>
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

                if (pftBackRefTable is not null)
                {
                    result = GetClassNameForDOT(((PropertyDefinition)pftBackRefTable.RelationshipReference.OwnerPropertyDefinition).OwnerDefinition);
                }
                else
                {
                    var pftTable = propDef.FunctionalType as PFTTablePart;
                    result = GetClassNameForDOT(pftTable!.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition);
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
    /// Get XmlComment for a property definition of table type (collection)
    /// </summary>
    /// <param name="propDef"></param>
    /// <returns>XmlComment</returns>
    public static XmlComment GetXmlCommentForTablePropDef(PropertyDefinition propDef) // TODO: Move it to a generalized generator of XmlComment or DocComment
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

                if (pftBackRefTable is not null)
                {
                    //result = new XmlComment(c_tableOfObjects + NameHelper.GetLocalNameUpperCase(((PropertyDefinition)pftBackRefTable.RelationshipReference.OwnerPropertyDefinition).OwnerDefinition.Names));
                    result = new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names));
                }
                else
                {
                    //var pftTable = in_propDef.FunctionalType as PFTTablePart;
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
    /// Get class name for a data object type
    /// </summary>
    /// <param name="dotDef">Data object type definition</param>
    /// <returns>Class name for data object type</returns>
    public static string GetClassNameForDOT(DOTDefinition dotDef) => NameHelper.NamesToPascalCase(dotDef.Names);
}
