using System;
using System.Collections.Generic;
using System.Globalization;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Constant;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Field;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter;
using MetaModel.DOTDefinition;
using MetaModel.PredefinedDO;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;
using PackNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Package.Model
{
    public class DOTPackage: PackNS.Package
    {
        #region Метод InitNew
        static void GenerateInitVariablesForMethod(DOTDefinition in_dotDef, CSMethod in_method)
        {
            foreach (PropertyDefinition propDef in in_dotDef.PropertyDefinitions.Values)
            {
                string initVarString = GenerateInitVariableForProp(propDef);
                if (initVarString != null)
                    in_method.BodyStrings.Add(initVarString);
            }
        }
        static string GenerateInitVariableForProp(PropertyDefinition in_propDef)
        {
            string varName = "_" + NameHelper.NamesToCamelCase(in_propDef.Names);
            bool initVar = false;
            string varValue = string.Empty;

            #region Непосредственные значения.
            if (!(in_propDef.FunctionalType is PFTLink))
            {
                PFTBoolean pftBool = in_propDef.FunctionalType as PFTBoolean;
                if (pftBool != null)
                {
                    varValue = GenerateInitVarValueForBool(in_propDef, pftBool);
                    initVar = varValue != null;
                }
                else
                {
                    PFTDateTime pftDateTime = in_propDef.FunctionalType as PFTDateTime;
                    if (pftDateTime != null)
                    {
                        varValue = GenerateInitVarValueForDateTime(
                            in_propDef,
                            pftDateTime,
                            pftDateTime is PFTDateAndTime || pftDateTime is PFTDate,
                            pftDateTime is PFTDateAndTime || pftDateTime is PFTTime);
                        initVar = varValue != null;
                    }
                    else
                    {
                        PFTDecimal pftDecimal = in_propDef.FunctionalType as PFTDecimal;
                        if (pftDecimal != null)
                        {
                            varValue = GenerateInitVarValueForDecimal(in_propDef, pftDecimal);
                            initVar = varValue != null;
                        }
                        else
                        {
                            PFTInteger pftInteger = in_propDef.FunctionalType as PFTInteger;
                            if (pftInteger != null)
                            {
                                varValue = GenerateInitVarValueForInt(in_propDef, pftInteger);
                                initVar = varValue != null;
                            }
                            else
                            {
                                PFTString pftString = in_propDef.FunctionalType as PFTString;
                                if (pftString != null)
                                {
                                    varValue = GenerateInitVarValueForString(in_propDef, pftString);
                                    initVar = varValue != null;
                                }
                                else
                                {
                                    PFTUniqueCode pftUniqueCode = in_propDef.FunctionalType as PFTUniqueCode;
                                    if (pftUniqueCode != null)
                                    {
                                        varValue = GenerateInitVarValueForUniqueCode(in_propDef, pftUniqueCode);
                                        initVar = varValue != null;
                                    }
                                    else
                                    {
                                        throw new ApplicationException(string.Format("Unsupported non-reference value initialization for PropertyFunctionalType {0} for PropertyDefinition Id {1}.", in_propDef.FunctionalType.GetType().Name, in_propDef.Id));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            else
            #region Ссылочные значения.
            {
                if (in_propDef.FunctionalType is PFTBackReferencedTable || in_propDef.FunctionalType is PFTTablePart)
                    return null;

                varName += "Id";

                if (in_propDef.FunctionalType is PFTConnector ||
                    in_propDef.FunctionalType is PFTReferenceValue ||
                    in_propDef.FunctionalType is PFTTableOwner)
                {
                    PFTLink pftRef = (PFTLink)in_propDef.FunctionalType;
                    varValue = GenerateInitVarValueForRef(in_propDef, pftRef);
                    initVar = varValue != null;
                }
                else
                {
                    throw new ApplicationException(string.Format("Unknown single-reference type {0} for PropertyDefinition Id {1}.", in_propDef.FunctionalType.GetType().Name, in_propDef.Id));
                }
            }
            #endregion

            return initVar ? string.Format("{0} = {1};", varName, varValue) : null;
        }
        static string GenerateInitVarValueForBool(PropertyDefinition in_propDef, PFTBoolean in_pftBool)
        {
            string result = null;

            bool? defaultValue = in_propDef.DefaultValue as bool?;
            if (!defaultValue.HasValue)
                defaultValue = in_pftBool.DefaultValue as bool?;

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
        static string GenerateInitVarValueForDateTime(PropertyDefinition in_propDef, PFTDateTime in_pftDateTime, bool in_useDatePart, bool in_useTimePart)
        {
            string result = null;
            
            SDateTime defaultValue = in_propDef.DefaultValue as SDateTime;
            if (defaultValue == null)
                defaultValue = in_pftDateTime.DefaultValue as SDateTime;

            string defSysValue = string.Empty;
            if (in_useDatePart && in_useTimePart)
                defSysValue = "DateTime.Now";
            else if (in_useDatePart)
                defSysValue = "DateTime.Today";
            else
                defSysValue = "new DateTime(1900, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)";

            if (defaultValue == null)
            {
                result = in_pftDateTime.Nullable ?
                    null : // "null" :
                    defSysValue;
            }
            else
            {
                result = defaultValue.UseModelRuntimeValue ?
                    defSysValue :
                    string.Format("new DateTime({0}, {1}, {2}, {3}, {4}, {5})",
                        defaultValue.FixedValue.Year,
                        defaultValue.FixedValue.Month,
                        defaultValue.FixedValue.Date,
                        defaultValue.FixedValue.Hour,
                        defaultValue.FixedValue.Minute,
                        defaultValue.FixedValue.Second);
            }

            return result;
        }
        static string GenerateInitVarValueForDecimal(PropertyDefinition in_propDef, PFTDecimal in_pftDecimal)
        {
            string result = null;

            decimal? defaultValue = in_propDef.DefaultValue as decimal?;
            if (!defaultValue.HasValue)
                defaultValue = in_pftDecimal.DefaultValue as decimal?;
            
            if (!defaultValue.HasValue)
            {
                //if (in_pftDecimal.Nullable)
                //    result = "null";
                //else
                //    result = "0.0m";
            }
            else
            {
                int defIntPart = (int)decimal.Truncate(defaultValue.Value);
                decimal defDecPart = decimal.Remainder(defaultValue.Value, 1m);
                result = defIntPart.ToString() + ".";
                string decPartStr = defDecPart.ToString(CultureInfo.InvariantCulture);

                result += decPartStr.Contains(".") ?
                    decPartStr.Substring(decPartStr.IndexOf('.') + 1) :
                    "0";
                result += "m";
            }

            return result;
        }
        static string GenerateInitVarValueForInt(PropertyDefinition in_propDef, PFTInteger in_pftInteger)
        {
            string result = null;

            int? defaultValue = in_propDef.DefaultValue as int?;
            if (!defaultValue.HasValue)
                defaultValue = in_pftInteger.DefaultValue as int?;
            
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
        static string GenerateInitVarValueForString(PropertyDefinition in_propDef, PFTString in_pftString)
        {
            string result = null;

            string defaultValue = in_propDef.DefaultValue as string;
            if (defaultValue == null)
                defaultValue = in_pftString.DefaultValue as string;
            
            if (defaultValue == null)
            {
                if (in_pftString.Nullable)
                    result = null; // "null";
                else
                    result = "string.Empty";
            }
            else
            {
                result = "\"" + defaultValue + "\"";
            }

            return result;
        }
        static string GenerateInitVarValueForUniqueCode(PropertyDefinition in_propDef, PFTUniqueCode in_pftUniqueCode)
        {
            string result = null;

            SGuid defaultValue = in_propDef.DefaultValue as SGuid;
            if (defaultValue == null)
                defaultValue = in_pftUniqueCode.DefaultValue as SGuid;
            
            if (defaultValue == null)
            {
                if (in_pftUniqueCode.Nullable)
                    result = null; // "null";
                else
                    result = "Guid.Empty";
            }
            else
            {
                result = defaultValue.GenerateValueAtRunTime ?
                    "Guid.NewGuid()" :
                    string.Format("new Guid(\"{0}\")", defaultValue.FixedValue);
            }

            return result;
        }
        static string GenerateInitVarValueForRef(PropertyDefinition in_propDef, PFTLink in_pftRef)
        {
            if (!(in_pftRef is PFTConnector || in_pftRef is PFTReferenceValue || in_pftRef is PFTTableOwner))
                throw new ArgumentException(string.Format("in_pftRef is not foreign key (single reference) type ({0}) for PropertyDefinition Id {1}.", in_pftRef.GetType().Name, in_propDef.Id));

            string result = null;

            SRefObject defaultValue = in_propDef.DefaultValue as SRefObject;
            if (defaultValue == null)
                defaultValue = in_pftRef.DefaultValue as SRefObject;
            
            if (defaultValue == null)
            {
                //if (in_pftRef.Nullable)
                //    result = "null";
                //else
                //    throw new ApplicationException(string.Format("Unable to init reference variable in DOT method InitNew, because property is not nullable but there is not default value defined (PropertyDefinition Id {0}).", in_propDef.Id));
            }
            else
            {
                PredefinedDO pdo = in_propDef.OwnerDefinition.MetaModel.AllPredefinedDOs[defaultValue.Key];
                string constName = NameHelper.NameToConstantId(pdo.Names);
                string className = CSharpHelper.GenerateDOTClassName(pdo.DOTDefinition);
                result = string.Format("{0}.Predefined.{1}", className, constName);
            }

            return result;
        }
        #endregion

        #region Метод Clone
        static void GenerateCloneVariablesForMethod(DOTDefinition in_dotDef, CSMethod in_method)
        {
            in_method.BodyStrings.Add($"var clone = new {in_method.Class.Name}();");
            in_method.BodyStrings.Add("CloneBase(this, clone);");

            foreach (var propDef in in_dotDef.PropertyDefinitions.Values)
            {
                var cloneVarString = GenerateCloneVariableForProp(propDef);
                if (cloneVarString != null)
                    in_method.BodyStrings.Add(cloneVarString);
            }

            in_method.BodyStrings.Add("return clone;");
        }
        static string GenerateCloneVariableForProp(PropertyDefinition in_propDef)
        {
            string varName = "_" + NameHelper.NamesToCamelCase(in_propDef.Names);
            bool cloneVar = false;
            string varValue = string.Empty;

            #region Непосредственные значения.
            if (!(in_propDef.FunctionalType is PFTLink))
            {
                cloneVar = true;
                varValue = varName;
            }
            #endregion
            else
            #region Ссылочные значения.
            {
                if (in_propDef.FunctionalType is PFTBackReferencedTable || in_propDef.FunctionalType is PFTTablePart)
                    return null;

                if (in_propDef.FunctionalType is PFTConnector ||
                    in_propDef.FunctionalType is PFTReferenceValue ||
                    in_propDef.FunctionalType is PFTTableOwner)
                {
                    cloneVar = true;
                    varName += "Id";
                    varValue = varName;
                }
                else
                {
                    throw new ApplicationException(string.Format("Unknown single-reference type {0} for PropertyDefinition Id {1}.", in_propDef.FunctionalType.GetType().Name, in_propDef.Id));
                }
            }
            #endregion

            return cloneVar ? string.Format("clone.{0} = {1};", varName, varValue) : null;
        }
        #endregion

        #region Метод Validate
        static void GenerateValidateForMethod(DOTDefinition in_dotDef, CSMethod in_method)
        {
            in_method.BodyStrings.Add("var baseResult = base.Validate();");
            in_method.BodyStrings.Add("if (baseResult != null)");
            in_method.BodyStrings.Add("    return baseResult;");
            in_method.BodyStrings.Add(string.Empty);
            in_method.BodyStrings.Add("baseResult = ValidateInner();");
            in_method.BodyStrings.Add("if (baseResult != null)");
            in_method.BodyStrings.Add("    return baseResult;");
            in_method.BodyStrings.Add(string.Empty);

            var propValidateStrings = new List<string>();
            foreach (var propDef in in_dotDef.PropertyDefinitions.Values)
                propValidateStrings.AddRange(GenerateValidatesStringsForPropDef(propDef));

            if (propValidateStrings.Count != 0)
            {
                foreach (var str in propValidateStrings)
                    in_method.BodyStrings.Add(str);
                in_method.BodyStrings.Add(string.Empty);
            }

            in_method.BodyStrings.Add("return null;");
        }
        static string[] GenerateValidatesStringsForPropDef(PropertyDefinition in_propDef)
        {
            var text = new List<string>();

            var varName = "_" + NameHelper.NamesToCamelCase(in_propDef.Names);
            var propLocalName = NameHelper.GetLocalNameUpperCase(in_propDef.Names);

            #region Непосредственные значения.
            if (!(in_propDef.FunctionalType is PFTLink))
            {
                var pftBool = in_propDef.FunctionalType as PFTBoolean;
                if (pftBool != null)
                {
                    text.AddRange(GenerateValidateVarValueForBool(in_propDef, pftBool));
                }
                else
                {
                    var pftDateTime = in_propDef.FunctionalType as PFTDateTime;
                    if (pftDateTime != null)
                    {
                        // Упрощенная проверка - трактуем типы данных "только дата" и "только время" как "дата и время".
                        text.AddRange(GenerateValidateVarValueForDateTime(in_propDef, pftDateTime, varName, propLocalName));
                    }
                    else
                    {
                        var pftDecimal = in_propDef.FunctionalType as PFTDecimal;
                        if (pftDecimal != null)
                        {
                            //varValue = GenerateInitVarValueForDecimal(in_propDef, pftDecimal);
                            //initVar = varValue != null;
                        }
                        else
                        {
                            var pftInteger = in_propDef.FunctionalType as PFTInteger;
                            if (pftInteger != null)
                            {
                                //varValue = GenerateInitVarValueForInt(in_propDef, pftInteger);
                                //initVar = varValue != null;
                            }
                            else
                            {
                                var pftString = in_propDef.FunctionalType as PFTString;
                                if (pftString != null)
                                {
                                    text.AddRange(GenerateValidateVarValueForString(in_propDef, pftString, varName, propLocalName));
                                }
                                else
                                {
                                    var pftUniqueCode = in_propDef.FunctionalType as PFTUniqueCode;
                                    if (pftUniqueCode != null)
                                    {
                                        //varValue = GenerateInitVarValueForUniqueCode(in_propDef, pftUniqueCode);
                                        //initVar = varValue != null;
                                    }
                                    else
                                    {
                                        throw new ApplicationException($"Unsupported non-reference value initialization for PropertyFunctionalType {in_propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {in_propDef.Id}.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            else
            #region Ссылочные значения.
            {
                if (in_propDef.FunctionalType is PFTBackReferencedTable || in_propDef.FunctionalType is PFTTablePart)
                    return new string[0];

                var varNameWOId = varName;
                varName += "Id";

                if (in_propDef.FunctionalType is PFTConnector ||
                    in_propDef.FunctionalType is PFTReferenceValue ||
                    in_propDef.FunctionalType is PFTTableOwner)
                {
                    var pftRef = (PFTLink)in_propDef.FunctionalType;
                    text.AddRange(GenerateValidateVarValueForRefValue(in_propDef, pftRef, varName, varNameWOId, propLocalName));
                }
                else
                {
                    throw new ApplicationException($"Unknown single-reference type {in_propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {in_propDef.Id}.");
                }
            }
            #endregion

            return text.ToArray();
        }
        static string[] GenerateValidateVarValueForBool(PropertyDefinition in_propDef, PFTBoolean in_pftBool)
        {
            var text = new List<string>();

            return text.ToArray();
        }
        static string[] GenerateValidateVarValueForString(PropertyDefinition in_propDef, PFTString in_pftString, string in_varName, string in_propLocalName)
        {
            var text = new List<string>();

            if (!in_pftString.Nullable)
            {
                text.Add($"if (string.IsNullOrWhiteSpace({in_varName}))");
                text.Add($"    return string.Format(c_propertyValueNotSet, \"{in_propLocalName}\");");
            }

            var minLength = in_pftString.MinLength;
            if (minLength == 0 && !in_pftString.Nullable)
                minLength = 1;

            var nullableEquation = string.Empty;
            if (in_pftString.Nullable)
                nullableEquation = $"{in_varName} != null && ";

            var minEquation = string.Empty;
            if (minLength > 0)
                minEquation = $"{(in_pftString.Nullable && minLength > 0 ? "(" : string.Empty)}{in_varName}.Length < {minLength} || ";

            var equation = $"if ({nullableEquation}{minEquation}{in_varName}.Length > {in_pftString.MaxLength})";
            if (in_pftString.Nullable && minLength > 0)
                equation += ")";
            text.Add(equation);
            text.Add($"    return string.Format(c_invalidPropertyLength, \"{in_propLocalName}\", {minLength}, {in_pftString.MaxLength});");

            return text.ToArray();
        }
        static string[] GenerateValidateVarValueForDateTime(PropertyDefinition in_propDef, PFTDateTime in_pftDateTime, string in_varName, string in_propLocalName)
        {
            var text = new List<string>();

            if (in_pftDateTime.Nullable)
                text.Add($"if ({in_varName}.HasValue && ({in_varName}.Value < C_MIN_SQL_DATE_TIME || {in_varName}.Value > C_MAX_SQL_DATE_TIME))");
            else
                text.Add($"if ({in_varName} < C_MIN_SQL_DATE_TIME || {in_varName} > C_MAX_SQL_DATE_TIME)");
            text.Add($"    return string.Format(c_invalidPropertyDateTime, \"{in_propLocalName}\");");

            return text.ToArray();
        }
        static string[] GenerateValidateVarValueForRefValue(PropertyDefinition in_propDef, PFTLink in_pftRef, string in_varName, string in_varNameWOId, string in_propLocalName)
        {
            if (!(in_propDef.FunctionalType is PFTConnector || in_propDef.FunctionalType is PFTReferenceValue || in_propDef.FunctionalType is PFTTableOwner))
                throw new ArgumentException("GenerateValidateVarValueForRefValue in_pftRef");

            var text = new List<string>();

            if (!in_pftRef.Nullable)
            {
                text.Add($"if (!{in_varNameWOId}Id.HasValue && {in_varNameWOId} == null)");
                text.Add($"    return string.Format(c_propertyValueNotSet, \"{in_propLocalName}\");");
            }

            return text.ToArray();
        }
        #endregion

        #region Метод ResetCachedRefProperties
        static void GenerateResetCachedRefPropertiesMethod(CSClass in_class, DOTDefinition in_dotDef)
        {
            var refProps = new List<PropertyDefinition>();

            foreach (var propDef in in_dotDef.PropertyDefinitions.Values)
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
                    Class = in_class,
                    DocComment = new XmlComment("Сброс закешированных ссылочных объектов"),
                    Name = "ResetCachedRefProperties",
                    ReturnType = "void",
                    Visibility = ElementVisibilityClassic.Public
                };
                in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

                foreach (var propDef in refProps)
                    method.BodyStrings.Add($"_{NameHelper.NamesToCamelCase(propDef.Names)} = null;");
            }
        }
        #endregion

        /// <summary>
        /// Создание и добавление в класс логического описания метода InitNew,
        /// создающего и инициализирующего умолчаниями новый объект ТОД.
        /// </summary>
        /// <param name="in_class">Класс</param>
        /// <param name="in_dotDef">Определение ТОД</param>
        static void CreateMethodInitNew(CSClass in_class, DOTDefinition in_dotDef)
        {
            var method = new CSMethod
            {
                Class = in_class,
                DocComment = new XmlComment("Создание нового объекта и инициализация значений по умолчанию"),
                IsStatic = false,
                HintSingleLineBody = false,
                Name = "InitNew",
                ReturnType = "void",
                Visibility = ElementVisibilityClassic.Public,
                AdditionalKeywords = "override"
            };
            in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
            GenerateInitVariablesForMethod(in_dotDef, method);
        }
        /// <summary>
        /// Создание и добавление в класс логического описания метода Clone,
        /// клонирующего объект ТОД.
        /// </summary>
        /// <param name="in_class">Класс</param>
        /// <param name="in_dotDef">Определение ТОД</param>
        static void CreateMethodClone(CSClass in_class, DOTDefinition in_dotDef)
        {
            var method = new CSMethod
            {
                Class = in_class,
                DocComment = new XmlComment("Создание независимой копии (клона) объекта"),
                IsStatic = false,
                HintSingleLineBody = false,
                Name = "Clone",
                ReturnType = "object",
                Visibility = ElementVisibilityClassic.Public,
                AdditionalKeywords = "override"
            };
            in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
            GenerateCloneVariablesForMethod(in_dotDef, method);
        }
        /// <summary>
        /// Создание и добавление в класс логического описания метода ToString,
        /// выдающего строковое представление объекта ТОД по умолчанию. В объекте
        /// берется иго наименование или эвристически ищется другое похожее
        /// поле. Если ничего не найдено, то в строку выдается Id.
        /// </summary>
        /// <param name="in_class">Класс</param>
        /// <param name="in_dotDef">Определение ТОД</param>
        static void CreateMethodToString(CSClass in_class, DOTDefinition in_dotDef)
        {
            var method = new CSMethod
            {
                Class = in_class,
                DocComment = new XmlComment("Строковое представление объекта по умолчанию"),
                IsStatic = false,
                HintSingleLineBody = true,
                Name = "ToString",
                ReturnType = "string",
                Visibility = ElementVisibilityClassic.Public,
                AdditionalKeywords = "override"
            };
            in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
            
            var varName = string.Empty;

            #region Эвристический поиск именующей или уникальной переменной.
            // 1. Сначала ищем свойство типа name.
            var found = false;
            var isString = true;
            var nullableValueType = false;
            foreach (var propDef in in_dotDef.PropertyDefinitions.Values)
            {
                if (propDef.FunctionalType is PFTName)
                {
                    varName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
                    found = true;
                    break;
                }
            }

            // 2. Во вторую очередь ищем свойство с уникальным значением.
            if (!found)
            {
                foreach (var propDef in in_dotDef.PropertyDefinitions.Values)
                {
                    if (propDef.FunctionalType.Unique)
                    {
                        varName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
                        found = true;
                        isString = propDef.FunctionalType is PFTString;
                        if (!isString)
                            nullableValueType = propDef.FunctionalType.Nullable;
                        break;
                    }
                }
            }

            // 3. В третью очередь ищем первое строковое значение.
            if (!found)
            {
                foreach (var propDef in in_dotDef.PropertyDefinitions.Values)
                {
                    if (propDef.FunctionalType is PFTString)
                    {
                        varName = "_" + NameHelper.NamesToCamelCase(propDef.Names);
                        found = true;
                        break;
                    }
                }
            }

            // 4. Если ничего не найдено, то используем _id.
            if (!found)
            {
                varName = "id";
                isString = false;
            }
            #endregion

            var getting = isString ?
                $"{varName} ?? string.Empty" :
                (nullableValueType ?
                    $"{varName}.HasValue ? {varName}.Value.ToString() : string.Empty" :
                    $"{varName}.ToString()");

            string gettingWOverrider = $"overrider != null && overrider.OverrideToString ? (overrider.ToStringOverride()) : ({getting})";

            method.BodyStrings.Add(gettingWOverrider + ";");
        }
        /// <summary>
        /// Создание и добавление в класс логического описания метода Validate,
        /// который вызывается перед сохранением изменений объекта ТОД в БД.
        /// </summary>
        /// <param name="in_class">Класс</param>
        /// <param name="in_dotDef">Определение ТОД</param>
        static void CreateMethodValidate(CSClass in_class, DOTDefinition in_dotDef)
        {
            var method = new CSMethod
            {
                Class = in_class,
                DocComment = new XmlComment("Проверка состояния объекта на допустимость к сохранению в БД"),
                IsStatic = false,
                HintSingleLineBody = false,
                Name = "Validate",
                ReturnType = "string",
                Visibility = ElementVisibilityClassic.Public,
                AdditionalKeywords = "override"
            };
            in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
            GenerateValidateForMethod(in_dotDef, method);
        }

        public static CSClass CreateDOTClass(Component.CSComponent component, DOTDefinition dotDef)
        {
            var name = CSharpHelper.GenerateDOTClassName(dotDef);

            var result = new CSClass()
            {
                Component = component,
                DocComment = new XmlComment(NameHelper.GetLocalNameUpperCase(dotDef.Names)),
                Name = name,
                InheritsFrom = "DbMappedDOT",
                Partial = true
            };
            component.Classes.Add(result.Name, result);

            // Создание полей класса для разных типов табличных полей:

            foreach (var propDef in dotDef.PropertyDefinitions.Values)
            {
                FieldPropertyHelper.GenerateFieldPropertyAndGetter(propDef, result, out CSClassField field, out CSClassField fieldId, out CSProperty prop, out CSProperty propId, out CSCollectionGetter getter, out CSCollectionCounter counter);
                if (field != null)
                    result.Fields.Add(field.Name, field);
                if (fieldId != null)
                    result.Fields.Add(fieldId.Name, fieldId);
                if (prop != null)
                    result.Properties.Add(prop.Name, prop);
                if (propId != null)
                    result.Properties.Add(propId.Name, propId);
                if (getter != null)
                    result.Methods.Add(getter.Name, getter);
                if (counter != null)
                    result.Methods.Add(counter.Name, counter);
            }

            #region Вставляем методы.
            CreateMethodInitNew(result, dotDef);
            CreateMethodClone(result, dotDef);
            CreateMethodToString(result, dotDef);
            CreateMethodValidate(result, dotDef);
            GenerateResetCachedRefPropertiesMethod(result, dotDef);
            #endregion

            #region Вставляем константы и помощники извлечения ПОД.
            if (dotDef.PredefinedDOs.Count != 0)
            {
                var predefsClass = new CSClassPredefined
                {
                    DocComment = new XmlComment("Быстрое извлечение предопределенных объектов"),
                    Name = "Predefined",
                    ParentClass = result
                };
                result.EmbeddedClassPredefined = predefsClass;

                foreach (var pdo in dotDef.PredefinedDOs)
                {
                    var idConst = new CSClassConstant("Guid", ElementVisibilityClassic.Public, false)
                    {
                        Class = predefsClass,
                        DocComment = new XmlComment("id объекта " + NameHelper.GetLocalNameUpperCase(pdo.Names)),
                        Name = NameHelper.NameToConstantId(pdo.Names),
                        Value = $"new Guid(\"{pdo.Id}\")"
                    };
                    predefsClass.Constants.Add(idConst.Name, idConst);

                    var predefProp = new CSProperty
                    {
                        Class = predefsClass,
                        DocComment = new XmlComment(NameHelper.GetLocalNameUpperCase(pdo.Names)),
                        Name = NameHelper.AddBeginningNIfNeeded(NameHelper.NamesToHungarianName(pdo.Names)),
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

        public DOTPackage(ModelPackage in_parentPackage)
            : base(in_parentPackage, "DOT")
        {
            var model = ParentPackage.ParentPackage.ParentPackage.DomainModel;
            var dbModel = ParentPackage.ParentPackage.ParentPackage.DBbSchemaModel;

            // Для каждого определения ТОД создаем компонент с соответствеющим классом.
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
    };
}
