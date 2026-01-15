using System.Globalization;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Constant;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Field;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter;
using VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;
using PackNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Package.Model;

public class DOTPackage : PackNS.Package
{
    #region InitNew method
    private static void GenerateInitVariablesForMethod(DOTDefinition dotDef, CSMethod method)
    {
        foreach (PropertyDefinition propDef in dotDef.PropertyDefinitions.Values)
        {
            var initVarString = GenerateInitVariableForProp(propDef);

            if (initVarString is not null)
            {
                method.BodyStrings.Add(initVarString);
            }
        }
    }

    private static string? GenerateInitVariableForProp(PropertyDefinition propDef)
    {
        var varName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
        var initVar = false;
        var varValue = string.Empty;

        #region Explicit (not-reference) values
        if (!(propDef.FunctionalType is PFTLink))
        {
            var pftBool = propDef.FunctionalType as PFTBoolean;

            if (pftBool is not null)
            {
                varValue = GenerateInitVarValueForBool(propDef, pftBool);
                initVar = varValue != null;
            }
            else
            {
                var pftDateTime = propDef.FunctionalType as PFTDateTime;

                if (pftDateTime is not null)
                {
                    varValue = GenerateInitVarValueForDateTime(
                        propDef,
                        pftDateTime,
                        pftDateTime is PFTDateAndTime || pftDateTime is PFTDate,
                        pftDateTime is PFTDateAndTime || pftDateTime is PFTTime);

                    initVar = varValue is not null;
                }
                else
                {
                    var pftDecimal = propDef.FunctionalType as PFTDecimal;

                    if (pftDecimal is not null)
                    {
                        varValue = GenerateInitVarValueForDecimal(propDef, pftDecimal);
                        initVar = varValue != null;
                    }
                    else
                    {
                        var pftInteger = propDef.FunctionalType as PFTInteger;

                        if (pftInteger is not null)
                        {
                            varValue = GenerateInitVarValueForInt(propDef, pftInteger);
                            initVar = varValue != null;
                        }
                        else
                        {
                            var pftString = propDef.FunctionalType as PFTString;

                            if (pftString is not null)
                            {
                                varValue = GenerateInitVarValueForString(propDef, pftString);
                                initVar = varValue != null;
                            }
                            else
                            {
                                var pftUniqueCode = propDef.FunctionalType as PFTUniqueCode;

                                if (pftUniqueCode is not null)
                                {
                                    varValue = GenerateInitVarValueForUniqueCode(propDef, pftUniqueCode);
                                    initVar = varValue != null;
                                }
                                else
                                {
                                    throw new ApplicationException(string.Format("Unsupported non-reference value initialization for PropertyFunctionalType {0} for PropertyDefinition Id {1}.", propDef.FunctionalType.GetType().Name, propDef.Id));
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
        else
        #region Reference values
        {
            if (propDef.FunctionalType is PFTBackReferencedTable || propDef.FunctionalType is PFTTablePart)
            {
                return null;
            }

            varName += "Id";

            if (propDef.FunctionalType is PFTConnector ||
                propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTTableOwner)
            {
                var pftRef = (PFTLink)propDef.FunctionalType;
                varValue = GenerateInitVarValueForRef(propDef, pftRef);
                initVar = varValue != null;
            }
            else
            {
                throw new ApplicationException(string.Format("Unknown single-reference type {0} for PropertyDefinition Id {1}.", propDef.FunctionalType.GetType().Name, propDef.Id));
            }
        }
        #endregion

        return initVar ? string.Format("{0} = {1};", varName, varValue) : null;
    }

    private static string? GenerateInitVarValueForBool(PropertyDefinition propDef, PFTBoolean pftBool)
    {
        string? result = null;

        var defaultValue = propDef.DefaultValue as bool?;

        if (!defaultValue.HasValue)
        {
            defaultValue = pftBool.DefaultValue as bool?;
        }

        if (!defaultValue.HasValue)
        {
            //if (in_pftBool.Nullable)
            //    result = "null";
            //else
            //    result = "false";
        }
        else
        {
            result = defaultValue.Value ? "true" : null; // "false";
        }

        return result;
    }

    private static string? GenerateInitVarValueForDateTime(PropertyDefinition propDef, PFTDateTime pftDateTime, bool useDatePart, bool useTimePart)
    {
        string? result = null;
        
        var defaultValue = propDef.DefaultValue as SDateTime;

        if (defaultValue is null)
        {
            defaultValue = pftDateTime.DefaultValue as SDateTime;
        }

        var defSysValue = string.Empty;

        if (useDatePart && useTimePart)
        {
            defSysValue = "DateTime.Now";
        }
        else if (useDatePart)
        {
            defSysValue = "DateTime.Today";
        }
        else
        {
            defSysValue = "new DateTime(1900, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)";
        }

        if (defaultValue is null)
        {
            result = pftDateTime.Nullable
                ? null
                : // "null" :
                  defSysValue;
        }
        else
        {
            result = defaultValue.UseModelRuntimeValue
                ? defSysValue
                : string.Format("new DateTime({0}, {1}, {2}, {3}, {4}, {5})",
                    defaultValue.FixedValue.Year,
                    defaultValue.FixedValue.Month,
                    defaultValue.FixedValue.Date,
                    defaultValue.FixedValue.Hour,
                    defaultValue.FixedValue.Minute,
                    defaultValue.FixedValue.Second
                );
        }

        return result;
    }

    private static string? GenerateInitVarValueForDecimal(PropertyDefinition propDef, PFTDecimal pftDecimal)
    {
        string? result = null;

        var defaultValue = propDef.DefaultValue as decimal?;

        if (!defaultValue.HasValue)
        {
            defaultValue = pftDecimal.DefaultValue as decimal?;
        }
        
        if (!defaultValue.HasValue)
        {
            //if (in_pftDecimal.Nullable)
            //    result = "null";
            //else
            //    result = "0.0m";
        }
        else
        {
            var defIntPart = (int)decimal.Truncate(defaultValue.Value);
            var defDecPart = decimal.Remainder(defaultValue.Value, 1m);
            result = defIntPart.ToString() + ".";
            var decPartStr = defDecPart.ToString(CultureInfo.InvariantCulture);

            result += decPartStr.Contains(".")
                ? decPartStr.Substring(decPartStr.IndexOf('.') + 1)
                : "0";

            result += "m";
        }

        return result;
    }

    private static string? GenerateInitVarValueForInt(PropertyDefinition propDef, PFTInteger pftInteger)
    {
        string? result = null;

        var defaultValue = propDef.DefaultValue as int?;

        if (!defaultValue.HasValue)
        {
            defaultValue = pftInteger.DefaultValue as int?;
        }
        
        if (!defaultValue.HasValue)
        {
            //if (in_pftInteger.Nullable)
            //    result = "null";
            //else
            //    result = "0";
        }
        else
        {
            result = defaultValue.Value.ToString(CultureInfo.InvariantCulture);
        }

        return result;
    }

    private static string? GenerateInitVarValueForString(PropertyDefinition propDef, PFTString pftString)
    {
        string? result = null;

        var defaultValue = propDef.DefaultValue as string;

        if (defaultValue is null)
        {
            defaultValue = pftString.DefaultValue as string;
        }
        
        if (defaultValue is null)
        {
            if (pftString.Nullable)
            {
                result = null; // "null";
            }
            else
            {
                result = "string.Empty";
            }
        }
        else
        {
            result = "\"" + defaultValue + "\"";
        }

        return result;
    }

    private static string? GenerateInitVarValueForUniqueCode(PropertyDefinition propDef, PFTUniqueCode pftUniqueCode)
    {
        string? result = null;

        var defaultValue = propDef.DefaultValue as SGuid;

        if (defaultValue is null)
        {
            defaultValue = pftUniqueCode.DefaultValue as SGuid;
        }
        
        if (defaultValue is null)
        {
            if (pftUniqueCode.Nullable)
            {
                result = null; // "null";
            }
            else
            {
                result = "Guid.Empty";
            }
        }
        else
        {
            result = defaultValue.GenerateValueAtRunTime
                ? "Guid.NewGuid()"
                : string.Format("new Guid(\"{0}\")", defaultValue.FixedValue);
        }

        return result;
    }

    private static string? GenerateInitVarValueForRef(PropertyDefinition propDef, PFTLink pftRef)
    {
        if (!(pftRef is PFTConnector || pftRef is PFTReferenceValue || pftRef is PFTTableOwner))
        {
            throw new ArgumentException(string.Format("in_pftRef is not foreign key (single reference) type ({0}) for PropertyDefinition Id {1}.", pftRef.GetType().Name, propDef.Id));
        }

        string? result = null;

        var defaultValue = propDef.DefaultValue as SRefObject;

        if (defaultValue is null)
        {
            defaultValue = pftRef.DefaultValue as SRefObject;
        }
        
        if (defaultValue is null)
        {
            //if (in_pftRef.Nullable)
            //    result = "null";
            //else
            //    throw new ApplicationException(string.Format("Unable to init reference variable in DOT method InitNew, because property is not nullable but there is not default value defined (PropertyDefinition Id {0}).", in_propDef.Id));
        }
        else
        {
            var pdo = propDef.OwnerDefinition.MetaModel.AllPredefinedDOs[defaultValue.Key];
            var constName = NameHelper.NameToConstantId(pdo.Names);
            var className = CSharpHelper.GenerateDOTClassName(pdo.DOTDefinition);
            result = string.Format("{0}.Predefined.{1}", className, constName);
        }

        return result;
    }
    #endregion

    #region Clone method
    private static void GenerateCloneVariablesForMethod(DOTDefinition dotDef, CSMethod method)
    {
        method.BodyStrings.Add($"var clone = new {method.Class.Name}();");
        method.BodyStrings.Add("CloneBase(this, clone);");

        foreach (var propDef in dotDef.PropertyDefinitions.Values)
        {
            var cloneVarString = GenerateCloneVariableForProp(propDef);

            if (cloneVarString is not null)
            {
                method.BodyStrings.Add(cloneVarString);
            }
        }

        method.BodyStrings.Add("return clone;");
    }

    private static string? GenerateCloneVariableForProp(PropertyDefinition propDef)
    {
        var varName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
        var cloneVar = false;
        var varValue = string.Empty;

        #region Explicit (non-reference) values
        if (!(propDef.FunctionalType is PFTLink))
        {
            cloneVar = true;
            varValue = varName;
        }
        #endregion
        else
        #region Reference values
        {
            if (propDef.FunctionalType is PFTBackReferencedTable || propDef.FunctionalType is PFTTablePart)
            {
                return null;
            }                

            if (propDef.FunctionalType is PFTConnector ||
                propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTTableOwner)
            {
                cloneVar = true;
                varName += "Id";
                varValue = varName;
            }
            else
            {
                throw new ApplicationException(string.Format("Unknown single-reference type {0} for PropertyDefinition Id {1}.", propDef.FunctionalType.GetType().Name, propDef.Id));
            }
        }
        #endregion

        return cloneVar ? string.Format("clone.{0} = {1};", varName, varValue) : null;
    }
    #endregion

    #region Validate method
    static void GenerateValidateForMethod(DOTDefinition dotDef, CSMethod method)
    {
        method.BodyStrings.Add("var baseResult = base.Validate();");
        method.BodyStrings.Add("if (baseResult != null)");
        method.BodyStrings.Add("    return baseResult;");
        method.BodyStrings.Add(string.Empty);
        method.BodyStrings.Add("baseResult = ValidateInner();");
        method.BodyStrings.Add("if (baseResult != null)");
        method.BodyStrings.Add("    return baseResult;");
        method.BodyStrings.Add(string.Empty);

        var propValidateStrings = new List<string>();

        foreach (var propDef in dotDef.PropertyDefinitions.Values)
        {
            propValidateStrings.AddRange(GenerateValidatesStringsForPropDef(propDef));
        }

        if (propValidateStrings.Count != 0)
        {
            foreach (var str in propValidateStrings)
            {
                method.BodyStrings.Add(str);
            }

            method.BodyStrings.Add(string.Empty);
        }

        method.BodyStrings.Add("return null;");
    }

    private static string[] GenerateValidatesStringsForPropDef(PropertyDefinition propDef)
    {
        var text = new List<string>();

        var varName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
        var propLocalName = NameHelper.GetLocalNameUpperCase(propDef.Names);

        #region Explicit (non-reference) values
        if (!(propDef.FunctionalType is PFTLink))
        {
            var pftBool = propDef.FunctionalType as PFTBoolean;

            if (pftBool is not null)
            {
                text.AddRange(GenerateValidateVarValueForBool(propDef, pftBool));
            }
            else
            {
                var pftDateTime = propDef.FunctionalType as PFTDateTime;

                if (pftDateTime is not null)
                {
                    // Simplified check - we treat data types "only date" and "only time" as "date and time"
                    text.AddRange(GenerateValidateVarValueForDateTime(propDef, pftDateTime, varName, propLocalName));
                }
                else
                {
                    var pftDecimal = propDef.FunctionalType as PFTDecimal;

                    if (pftDecimal is not null)
                    {
                        //varValue = GenerateInitVarValueForDecimal(in_propDef, pftDecimal);
                        //initVar = varValue != null;
                    }
                    else
                    {
                        var pftInteger = propDef.FunctionalType as PFTInteger;

                        if (pftInteger is not null)
                        {
                            //varValue = GenerateInitVarValueForInt(in_propDef, pftInteger);
                            //initVar = varValue != null;
                        }
                        else
                        {
                            var pftString = propDef.FunctionalType as PFTString;

                            if (pftString is not null)
                            {
                                text.AddRange(GenerateValidateVarValueForString(propDef, pftString, varName, propLocalName));
                            }
                            else
                            {
                                var pftUniqueCode = propDef.FunctionalType as PFTUniqueCode;

                                if (pftUniqueCode is not null)
                                {
                                    //varValue = GenerateInitVarValueForUniqueCode(in_propDef, pftUniqueCode);
                                    //initVar = varValue != null;
                                }
                                else
                                {
                                    throw new ApplicationException($"Unsupported non-reference value initialization for PropertyFunctionalType {propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {propDef.Id}.");
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
        else
        #region Reference values
        {
            if (propDef.FunctionalType is PFTBackReferencedTable || propDef.FunctionalType is PFTTablePart)
            {
                return [];
            }

            var varNameWOId = varName;
            varName += "Id";

            if (propDef.FunctionalType is PFTConnector ||
                propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTTableOwner)
            {
                var pftRef = (PFTLink)propDef.FunctionalType;
                text.AddRange(GenerateValidateVarValueForRefValue(propDef, pftRef, varName, varNameWOId, propLocalName));
            }
            else
            {
                throw new ApplicationException($"Unknown single-reference type {propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {propDef.Id}.");
            }
        }
        #endregion

        return text.ToArray();
    }

    private static string[] GenerateValidateVarValueForBool(PropertyDefinition propDef, PFTBoolean pftBool)
    {
        var text = new List<string>();

        return text.ToArray();
    }

    private static string[] GenerateValidateVarValueForString(PropertyDefinition propDef, PFTString pftString, string varName, string propLocalName)
    {
        var text = new List<string>();

        if (!pftString.Nullable)
        {
            text.Add($"if (string.IsNullOrWhiteSpace({varName}))");
            text.Add($"    return string.Format(c_propertyValueNotSet, \"{propLocalName}\");");
        }

        var minLength = pftString.MinLength;

        if (minLength == 0 && !pftString.Nullable)
        {
            minLength = 1;
        }            

        var nullableEquation = string.Empty;

        if (pftString.Nullable)
        {
            nullableEquation = $"{varName} != null && ";
        }            

        var minEquation = string.Empty;

        if (minLength > 0)
        {
            minEquation = $"{(pftString.Nullable && minLength > 0 ? "(" : string.Empty)}{varName}.Length < {minLength} || ";
        }

        var equation = $"if ({nullableEquation}{minEquation}{varName}.Length > {pftString.MaxLength})";

        if (pftString.Nullable && minLength > 0)
        {
            equation += ")";
        }

        text.Add(equation);
        text.Add($"    return string.Format(c_invalidPropertyLength, \"{propLocalName}\", {minLength}, {pftString.MaxLength});");

        return text.ToArray();
    }

    private static string[] GenerateValidateVarValueForDateTime(PropertyDefinition propDef, PFTDateTime pftDateTime, string varName, string propLocalName)
    {
        var text = new List<string>();

        if (pftDateTime.Nullable)
        {
            text.Add($"if ({varName}.HasValue && ({varName}.Value < C_MIN_SQL_DATE_TIME || {varName}.Value > C_MAX_SQL_DATE_TIME))");
        }
        else
        {
            text.Add($"if ({varName} < C_MIN_SQL_DATE_TIME || {varName} > C_MAX_SQL_DATE_TIME)");
        }

        text.Add($"    return string.Format(c_invalidPropertyDateTime, \"{propLocalName}\");");

        return text.ToArray();
    }

    private static string[] GenerateValidateVarValueForRefValue(PropertyDefinition propDef, PFTLink pftRef, string varName, string varNameWOId, string propLocalName)
    {
        if (!(propDef.FunctionalType is PFTConnector || propDef.FunctionalType is PFTReferenceValue || propDef.FunctionalType is PFTTableOwner))
        {
            throw new ArgumentException("GenerateValidateVarValueForRefValue in_pftRef");
        }

        var text = new List<string>();

        if (!pftRef.Nullable)
        {
            text.Add($"if (!{varNameWOId}Id.HasValue && {varNameWOId} == null)");
            text.Add($"    return string.Format(c_propertyValueNotSet, \"{propLocalName}\");");
        }

        return text.ToArray();
    }
    #endregion

    #region ResetCachedRefProperties method
    private static void GenerateResetCachedRefPropertiesMethod(CSClass @class, DOTDefinition dotDef)
    {
        var refProps = new List<PropertyDefinition>();

        foreach (var propDef in dotDef.PropertyDefinitions.Values)
        {
            if (propDef.FunctionalType is PFTConnector ||
                propDef.FunctionalType is PFTReferenceValue ||
                propDef.FunctionalType is PFTTableOwner)
            {
                refProps.Add(propDef);
            }
        }

        if (refProps.Count > 0)
        {
            var method = new CSMethod
            {
                AdditionalKeywords = "override",
                Class = @class,
                DocComment = new XmlComment("Сброс закешированных ссылочных объектов"),
                Name = "ResetCachedRefProperties",
                ReturnType = "void",
                Visibility = ElementVisibilityClassic.Public
            };
            @class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

            foreach (var propDef in refProps)
            {
                method.BodyStrings.Add($"_{NameHelper.NamesToCamelCase(propDef.Names)} = null;");
            }
        }
    }
    #endregion

    private static void CreateMethodInitNew(CSClass @class, DOTDefinition dotDef)
    {
        var method = new CSMethod
        {
            Class = @class,
            DocComment = new XmlComment("Create a new object and initialize it with default values"),
            IsStatic = false,
            HintSingleLineBody = false,
            Name = "InitNew",
            ReturnType = "void",
            Visibility = ElementVisibilityClassic.Public,
            AdditionalKeywords = "override"
        };
        @class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
        GenerateInitVariablesForMethod(dotDef, method);
    }

    private static void CreateMethodClone(CSClass @class, DOTDefinition dotDef)
    {
        var method = new CSMethod
        {
            Class = @class,
            DocComment = new XmlComment("Create an isolated instance of the object (clone)"),
            IsStatic = false,
            HintSingleLineBody = false,
            Name = "Clone",
            ReturnType = "object",
            Visibility = ElementVisibilityClassic.Public,
            AdditionalKeywords = "override"
        };
        @class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
        GenerateCloneVariablesForMethod(dotDef, method);
    }

    private static void CreateMethodToString(CSClass @class, DOTDefinition dotDef)
    {
        var method = new CSMethod
        {
            Class = @class,
            DocComment = new XmlComment("Default string representation of an object"),
            IsStatic = false,
            HintSingleLineBody = true,
            Name = "ToString",
            ReturnType = "string",
            Visibility = ElementVisibilityClassic.Public,
            AdditionalKeywords = "override"
        };
        @class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
        
        var varName = string.Empty;

        #region Heuristically search for a name-like or a unique property
        // 1. Search for a name-like property
        var found = false;
        var isString = true;
        var nullableValueType = false;

        foreach (var propDef in dotDef.PropertyDefinitions.Values)
        {
            if (propDef.FunctionalType is PFTName)
            {
                varName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
                found = true;
                break;
            }
        }

        // 2. Then search for a unique-value property
        if (!found)
        {
            foreach (var propDef in dotDef.PropertyDefinitions.Values)
            {
                if (propDef.FunctionalType.Unique)
                {
                    varName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
                    found = true;
                    isString = propDef.FunctionalType is PFTString;

                    if (!isString)
                    {
                        nullableValueType = propDef.FunctionalType.Nullable;
                    }

                    break;
                }
            }
        }

        // 3. Then try get the first string value
        if (!found)
        {
            foreach (var propDef in dotDef.PropertyDefinitions.Values)
            {
                if (propDef.FunctionalType is PFTString)
                {
                    varName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
                    found = true;
                    break;
                }
            }
        }

        // 4. If nothing found, use value of _id
        if (!found)
        {
            varName = "id";
            isString = false;
        }
        #endregion

        var getting = isString
            ? $"{varName} ?? string.Empty"
            : (nullableValueType
                ? $"{varName}.HasValue ? {varName}.Value.ToString() : string.Empty"
                : $"{varName}.ToString()"
            );

        var gettingWOverrider = $"overrider != null && overrider.OverrideToString ? (overrider.ToStringOverride()) : ({getting})";

        method.BodyStrings.Add(gettingWOverrider + ";");
    }

    private static void CreateMethodValidate(CSClass @class, DOTDefinition dotDef)
    {
        var method = new CSMethod
        {
            Class = @class,
            DocComment = new XmlComment("Validate an object state before saving it to a database"),
            IsStatic = false,
            HintSingleLineBody = false,
            Name = "Validate",
            ReturnType = "string",
            Visibility = ElementVisibilityClassic.Public,
            AdditionalKeywords = "override"
        };
        @class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
        GenerateValidateForMethod(dotDef, method);
    }

    public static CSClass CreateDOTClass(Component.CSComponent component, DOTDefinition dotDef)
    {
        var name = CSharpHelper.GenerateDOTClassName(dotDef);

        var result = new CSClass
        {
            Component = component,
            DocComment = new XmlComment(NameHelper.GetLocalNameUpperCase(dotDef.Names)),
            Name = name,
            InheritsFrom = "DbMappedDOT",
            Partial = true
        };
        component.Classes.Add(result.Name, result);

        // Create class fields for different table fields:

        foreach (var propDef in dotDef.PropertyDefinitions.Values)
        {
            FieldPropertyHelper.GenerateFieldPropertyAndGetter(
                propDef,
                result,
                out CSClassField field,
                out CSClassField fieldId,
                out CSProperty prop,
                out CSProperty propId,
                out CSCollectionGetter getter,
                out CSCollectionCounter counter
            );

            if (field is not null)
            {
                result.Fields.Add(field.Name, field);
            }

            if (fieldId is not null)
            {
                result.Fields.Add(fieldId.Name, fieldId);
            }

            if (prop is not null)
            {
                result.Properties.Add(prop.Name, prop);
            }

            if (propId is not null)
            {
                result.Properties.Add(propId.Name, propId);
            }

            if (getter is not null)
            {
                result.Methods.Add(getter.Name, getter);
            }

            if (counter is not null)
            {
                result.Methods.Add(counter.Name, counter);
            }
        }

        #region Insert methods
        CreateMethodInitNew(result, dotDef);
        CreateMethodClone(result, dotDef);
        CreateMethodToString(result, dotDef);
        CreateMethodValidate(result, dotDef);
        GenerateResetCachedRefPropertiesMethod(result, dotDef);
        #endregion

        #region Insert constants and helpers for retrieving predefined objects
        if (dotDef.PredefinedDOs.Count != 0)
        {
            var predefsClass = new CSClassPredefined
            {
                DocComment = new XmlComment("Quick retrieve of predefined objects"),
                Name = "Predefined",
                ParentClass = result
            };
            result.EmbeddedClassPredefined = predefsClass;

            foreach (var pdo in dotDef.PredefinedDOs)
            {
                var idConst = new CSClassConstant("Guid", ElementVisibilityClassic.Public, false)
                {
                    Class = predefsClass,
                    DocComment = new XmlComment("id of object " + NameHelper.GetLocalNameUpperCase(pdo.Names)),
                    Name = NameHelper.NameToConstantId(pdo.Names),
                    Value = $"new Guid(\"{pdo.Id}\")"
                };
                predefsClass.Constants.Add(idConst.Name, idConst);

                var predefProp = new CSProperty
                {
                    Class = predefsClass,
                    DocComment = new XmlComment(NameHelper.GetLocalNameUpperCase(pdo.Names)),
                    Name = NameHelper.AddBeginningNIfNeeded(NameHelper.NamesToPascalCase(pdo.Names)),
                    Type = CSharpHelper.GenerateDOTClassName(pdo.DOTDefinition),
                    IsStatic = true
                };
                var predefGetter = new CSPropertyGetterPredefinedObject(predefProp)
                {
                    CorrespondingPDO = pdo,
                    IdConstName = NameHelper.NameToConstantId(pdo.Names)
                };
                predefProp.Getter = predefGetter;
                predefsClass.Properties.Add(predefProp.Name, predefProp);
            }
        }
        #endregion

        return result;
    }

    public DOTPackage(ModelPackage parentPackage)
        : base(parentPackage, "DOT")
    {
        var model = ParentPackage.ParentPackage.ParentPackage.DomainModel;
        var dbModel = ParentPackage.ParentPackage.ParentPackage.DBbSchemaModel;

        // For each of data object type definition create a component with a corresponding class
        var dotDefs = model.AllDOTDefinitions.Values;

        foreach (var dotDef in dotDefs)
        {
            var name = CSharpHelper.GenerateDOTClassName(dotDef);

            var modelComponent = new CSComponentWMainClass
            {
                Package = this,
                Name = name + ".cs",
                DOTDefinition = dotDef,
                Namespace = $"{ParentPackage.ParentPackage.RootNamespace}.Model.DOT"
            };
            _components.Add(modelComponent.Name, modelComponent);
            modelComponent.SystemUsings.Add("System");
            modelComponent.SystemUsings.Add("System.Collections.Generic");
            modelComponent.SystemUsings.Add("System.Data");
            modelComponent.SystemUsings.Add("System.Data.Common");
            modelComponent.UserUsings.Add("orm.Db");
            //modelComponent.UserUsings.Add("orm.Util");
            modelComponent.UserUsings.Add($"{ParentPackage.ParentPackage.RootNamespace}.Model.Storage");

            var modelClass = CreateDOTClass(modelComponent, dotDef);
            modelComponent.MainClass = modelClass;
        }
    }

    public new ModelPackage ParentPackage { get { return (ModelPackage)_parentPackage; } }
}
