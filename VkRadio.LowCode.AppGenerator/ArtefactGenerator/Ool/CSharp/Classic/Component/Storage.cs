using System;
using System.Collections.Generic;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Constant;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Model;
using ArtefactGenerationProject.ArtefactGenerator.Sql;
using MetaModel.DOTDefinition;
using MetaModel.PredefinedDO;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component
{
    public class Storage: CSComponentWMainClass
    {
        static List<CSClassConstant> GenerateFieldConstants(TableAndDOTCorrespondenceJson in_tableAndDotCorrespondence, CSClass in_storageClass, List<int> in_decimalConstants)
        {
            List<CSClassConstant> result = new List<CSClassConstant>();

            foreach (ITableFieldJson field in in_tableAndDotCorrespondence.Table.AllFields)
            {
                if (field is PKSingleJson)
                {
                    in_decimalConstants.Add(0);
                    continue;
                }

                ValueFieldJson vf = field as ValueFieldJson;
                ForeignKeyFieldJson fk = field as ForeignKeyFieldJson;

                if (vf == null && fk == null)
                    throw new ApplicationException(string.Format("Table {0} contains ITableField {1} that is not of type ValueField or ForeignKeyField: {2}; PropertyDefinition Id = {3}.", in_tableAndDotCorrespondence.Table.Name, field.Name, field.GetType().Name, in_tableAndDotCorrespondence.PropertyCorrespondences[0].PropertyDefinition.Id));

                CSClassConstant c = new CSClassConstant("string", ElementVisibilityClassic.Public, true)
                {
                    Class = in_storageClass,
                    DocComment = new XmlComment(string.Format("Табличное поле {0} (вариант без кавычек)", NameHelper.GetLocalNameUpperCase(field.DOTPropertyCorrespondence.PropertyDefinition.Names))),
                    Name = string.Format("{0}{1}", NameHelper.NameToConstant(field.DOTPropertyCorrespondence.PropertyDefinition.Names, false), fk != null ? "_ID" : string.Empty),
                    Value = string.Format("\"{0}\"", field.Name)
                };
                in_storageClass.Constants.Add(c.Name, c);
                result.Add(c);

                c = new CSClassConstant("string", ElementVisibilityClassic.Public, true)
                {
                    Class = in_storageClass,
                    DocComment = new XmlComment(string.Format("Табличное поле {0} (вариант с кавычками)", NameHelper.GetLocalNameUpperCase(field.DOTPropertyCorrespondence.PropertyDefinition.Names))),
                    Name = string.Format("{0}{1}_Q", NameHelper.NameToConstant(field.DOTPropertyCorrespondence.PropertyDefinition.Names, false), fk != null ? "_ID" : string.Empty),
                    Value = string.Format("\"\\\"{0}\\\"\"", field.Name)
                };
                in_storageClass.Constants.Add(c.Name, c);
                result.Add(c);

                int decimalPositions = 0;
                if (field.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTMoney)
                {
                    PFTMoney pftMoney = (PFTMoney)field.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType;
                    if (pftMoney.DecimalPositions == 0)
                        throw new ApplicationException(string.Format("Property {0} is PFTMoney but has 0 DecimalPositions.", field.DOTPropertyCorrespondence.PropertyDefinition.Id));
                    decimalPositions = pftMoney.DecimalPositions;
                }
                in_decimalConstants.Add(decimalPositions);
            }

            return result;
        }
        static void GenerateConstructor(List<CSClassConstant> in_fieldConstants, CSClass in_storageClass, string in_dotClassName, string in_storageClassName, DOTDefinition in_dotDef, TableAndDOTCorrespondenceJson in_tableAndDotCorrespondence, List<int> in_decimalConstants)
        {
            CSConstructor ctor = new CSConstructor(in_storageClass)
            {
                Class = in_storageClass,
                DocComment = new XmlComment("Конструктор хранилища"),
                HintSingleLineBody = false,
                Visibility = ElementVisibilityClassic.Public
            };
            in_storageClass.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

            ctor.BodyStrings.Add(string.Format("_tableName = \"{0}\";", in_tableAndDotCorrespondence.Table.Name));
            ctor.BodyStrings.Add(string.Format("_tableNameQ = \"\\\"{0}\\\"\";", in_tableAndDotCorrespondence.Table.Name));

            foreach (int decConstant in in_decimalConstants)
                ctor.BodyStrings.Add(string.Format("_decimalFields.Add({0});", decConstant));

            #region Поля БД.
            ctor.BodyStrings.Add("_fields = new string[]");
            ctor.BodyStrings.Add("{");

            ctor.BodyStrings.Add(string.Format("{0}c_fieldId{1}", CSharpHelper.C_TAB, in_fieldConstants.Count > 0 ? "," : string.Empty));

            for (int i = 0; i < in_fieldConstants.Count; i += 2)
            {
                ctor.BodyStrings.Add(string.Format("{0}{1}{2}",
                    CSharpHelper.C_TAB,
                    in_fieldConstants[i].Name,
                    i != in_fieldConstants.Count - 1 ? "," : string.Empty)
                );
            }

            ctor.BodyStrings.Add("};");
            #endregion

            #region Поля БД на человеческом языке.
            ctor.BodyStrings.Add("_fieldsHuman = new string[]");
            ctor.BodyStrings.Add("{");

            ctor.BodyStrings.Add(string.Format("{0}\"id\"{1}", CSharpHelper.C_TAB, in_fieldConstants.Count > 0 ? "," : string.Empty));

            for (int i = 0; i < in_fieldConstants.Count; i += 2)
            {
                PropertyCorrespondenceJson corr = null;
                for (int j = 0; j < in_tableAndDotCorrespondence.PropertyCorrespondences.Count; j++)
                {
                    string fc = in_fieldConstants[i].Value.Substring(1, in_fieldConstants[i].Value.Length - 2);
                    if (in_tableAndDotCorrespondence.PropertyCorrespondences[j].TableField.Name == fc)
                    {
                        corr = in_tableAndDotCorrespondence.PropertyCorrespondences[j];
                        break;
                    }
                }
                string humanName = corr.PropertyDefinition.Names[MetaModel.Names.HumanLanguageEnum.Ru].Replace("\"", "\\\"");

                ctor.BodyStrings.Add(string.Format("{0}\"{1}\"{2}",
                    CSharpHelper.C_TAB,
                    humanName,
                    (i != in_fieldConstants.Count - 1) ? "," : string.Empty)
                );
            }

            ctor.BodyStrings.Add("};");
            #endregion

            ctor.BodyStrings.Add("InitParams();");

            // Заполнение словаря Id предопределенных объектов.
            if (in_dotDef.PredefinedDOs.Count != 0)
            {
                ctor.BodyStrings.Add(string.Empty);
                foreach (PredefinedDO pdo in in_dotDef.PredefinedDOs)
                    ctor.BodyStrings.Add(string.Format("_predefinedObjects.Add(new Guid(\"{0}\"), null);", pdo.Id));
            }

            #region Формирование упорядочивания списка по умолчанию.
            bool reverseOrder;
            PropertyDefinition orderByProperty = GeneralHelper.GetListSortProperty(in_dotDef, out reverseOrder);
            if (orderByProperty != null)
            {
                string quoteSymbol = in_tableAndDotCorrespondence.DBSchemaMetaModel.SchemaDeploymentScript.QuoteSymbol;
                if (quoteSymbol == "\"")
                    quoteSymbol = "\\" + quoteSymbol;
                ctor.BodyStrings.Add(string.Format("_defaultOrderBy = \"{0}{1}{0}{2}\";", quoteSymbol, NameHelper.NameToUnderscoreSeparatedName(orderByProperty.Names), reverseOrder ? " desc" : string.Empty));
            }
            #endregion
        }
        static void GenerateFillDOFromReader(CSClass in_storageClass, string in_dotClassName, TableAndDOTCorrespondenceJson in_tableAndDotCorrespondence)
        {
            #region Шапка метода.
            CSMethod method = new CSMethod()
            {
                AdditionalKeywords = "override",
                Class = in_storageClass,
                DocComment = new XmlComment("Заполнение свойств ОД из DbDataReader"),
                HintSingleLineBody = false,
                IsStatic = false,
                Name = "FillDOFromReader",
                ReturnType = "void",
                Visibility = ElementVisibilityClassic.Public
            };

            CSParameter param = new CSParameter()
            {
                Name = "in_reader",
                Type = "DbDataReader"
            };
            method.Params.Add(param.Name, param);

            param = new CSParameter()
            {
                Name = "in_o",
                Type = in_dotClassName
            };
            method.Params.Add(param.Name, param);

            in_storageClass.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
            #endregion

            method.BodyStrings.Add("orm.Db.DbProviderFactory factory = orm.Db.DbProviderFactory.Instance;");

            DOTDefinition dotDef = in_tableAndDotCorrespondence.DOTDefinition;
            for (int i = 1; i < in_tableAndDotCorrespondence.Table.AllFields.Count; i++)
            {
                #region Поиск поля таблицы.
                ITableFieldJson field = in_tableAndDotCorrespondence.Table.AllFields[i];
                PropertyCorrespondenceJson corr = null;
                foreach (PropertyCorrespondenceJson c in in_tableAndDotCorrespondence.PropertyCorrespondences)
                {
                    if (c.TableField.Name == field.Name)
                    {
                        corr = c;
                        break;
                    }
                }
                #endregion

                bool isId = false;
                bool isNullable = corr.PropertyDefinition.FunctionalType.Nullable;
                string rightPart;
                if (!(corr.PropertyDefinition.FunctionalType is PFTLink))
                {
                    if (corr.PropertyDefinition.FunctionalType is PFTBoolean)
                        rightPart = $"factory.ReadBool{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                    else if (corr.PropertyDefinition.FunctionalType is PFTDateTime)
                        rightPart = $"factory.ReadDateTime{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                    else if (corr.PropertyDefinition.FunctionalType is PFTDecimal)
                        rightPart = $"factory.ReadDecimal{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                    else if (corr.PropertyDefinition.FunctionalType is PFTInteger)
                        rightPart = $"factory.ReadInt{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                    else if (corr.PropertyDefinition.FunctionalType is PFTString)
                        rightPart = $"factory.ReadStringFromReader(in_reader, {i}, {(isNullable ? "true" : "false")});";
                    else if (corr.PropertyDefinition.FunctionalType is PFTUniqueCode)
                        rightPart = $"factory.ReadGuid{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                    else
                        throw new ApplicationException(string.Format("Unexpected non-reference PropertyFunctionalType {0} in PropertyDefinition Id {1} for constructing FillDOFromReader.", corr.PropertyDefinition.FunctionalType.GetType().Name, corr.PropertyDefinition.Id));
                }
                else
                {
                    if (corr.PropertyDefinition.FunctionalType is PFTBackReferencedTable ||
                        corr.PropertyDefinition.FunctionalType is PFTConnector ||
                        corr.PropertyDefinition.FunctionalType is PFTReferenceValue ||
                        corr.PropertyDefinition.FunctionalType is PFTTableOwner)
                    {
                        isId = true;
                        rightPart = $"factory.ReadGuid{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("Unexpected reference type: {0} in PropertyDefinition Id {1} when constructing values for FillDOFromReader.", corr.PropertyDefinition.FunctionalType.GetType().Name, corr.PropertyDefinition.Id));
                    }
                }

                string propName = NameHelper.NamesToHungarianName(corr.PropertyDefinition.Names);
                if (isId)
                    propName += "Id";

                method.BodyStrings.Add($"in_o.{propName} = {rightPart}");
            }
        }
        static void GenerateFillParameters(CSClass in_storageClass, string in_dotClassName, TableAndDOTCorrespondenceJson in_tableAndDotCorrespondence)
        {
            #region Шапка метода.
            CSMethod method = new CSMethod()
            {
                AdditionalKeywords = "override",
                Class = in_storageClass,
                DocComment = new XmlComment("Заполнение параметров для записи состояния ОД в БД"),
                HintSingleLineBody = false,
                IsStatic = false,
                Name = "FillParameters",
                ReturnType = "void",
                Visibility = ElementVisibilityClassic.Protected
            };

            CSParameter param = new CSParameter()
            {
                Name = "in_params",
                Type = "DbParameterCollection"
            };
            method.Params.Add(param.Name, param);

            param = new CSParameter()
            {
                Name = "in_o",
                Type = in_dotClassName
            };
            method.Params.Add(param.Name, param);

            in_storageClass.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
            #endregion

            method.BodyStrings.Add("orm.Db.DbProviderFactory factory = orm.Db.DbProviderFactory.Instance;");

            DOTDefinition dotDef = in_tableAndDotCorrespondence.DOTDefinition;
            for (int i = 1; i < in_tableAndDotCorrespondence.Table.AllFields.Count; i++)
            {
                #region Поиск поля таблицы.
                ITableFieldJson field = in_tableAndDotCorrespondence.Table.AllFields[i];
                PropertyCorrespondenceJson corr = null;
                foreach (PropertyCorrespondenceJson c in in_tableAndDotCorrespondence.PropertyCorrespondences)
                {
                    if (c.TableField.Name == field.Name)
                    {
                        corr = c;
                        break;
                    }
                }
                #endregion

                bool isNullable, isId;
                string typeName;
                CSharpHelper.GetTypeInfoForFillDOFromReaderRow(corr.PropertyDefinition, out isNullable, out isId, out typeName);

                string propName = NameHelper.NamesToHungarianName(corr.PropertyDefinition.Names);
                if (isId)
                {
                    string propNameWOId = propName;
                    propName += "Id";
                    method.BodyStrings.Add(string.Format("if (!in_o.{0}Id.HasValue && in_o.{0} != null)", propNameWOId));
                    method.BodyStrings.Add(CSharpHelper.C_TAB + string.Format("in_params.Add(factory.CreateParameter(_params[{1}], in_o.{0}.Id.Value, typeof(Guid?), true));", propNameWOId, i));
                    method.BodyStrings.Add("else");
                    method.BodyStrings.Add(CSharpHelper.C_TAB + string.Format("in_params.Add(factory.CreateParameter(_params[{0}], in_o.{1}, typeof({2}), {3}));", i, propName, typeName, isNullable ? "true" : "false"));
                }
                else
                {
                    method.BodyStrings.Add(string.Format("in_params.Add(factory.CreateParameter(_params[{0}], in_o.{1}, typeof({2}), {3}));", i, propName, typeName, isNullable ? "true" : "false"));
                }
            }
        }
        static void GenerateRestoreByNameMethod(CSClass in_storageClass, string in_dotClassName, TableAndDOTCorrespondenceJson in_tableAndDotCorrespondence)
        {
            // 1. Выясняем, есть ли у ТОД поле, соответствующее наименованию.
            PropertyCorrespondenceJson candidate = null;
            foreach (PropertyCorrespondenceJson c in in_tableAndDotCorrespondence.PropertyCorrespondences)
            {
                if (c.PropertyDefinition.FunctionalType is PFTName)
                {
                    if (candidate == null || (!candidate.PropertyDefinition.FunctionalType.Unique && c.PropertyDefinition.FunctionalType.Unique))
                        candidate = c;
                }
            }
            if (candidate == null) // Если поля-наименования нет, то и метод чтения объектов по нему бессмысленнен.
                return;

            // 2. Создаем метод.
            #region Шапка метода.
            string propName = NameHelper.NamesToHungarianName(candidate.PropertyDefinition.Names, false);
            string constName = NameHelper.NameToConstant(candidate.PropertyDefinition.Names, false) + "_Q";
            bool isSingle = candidate.PropertyDefinition.FunctionalType.Unique;
            string nameHuman = candidate.PropertyDefinition.Names[MetaModel.Names.HumanLanguageEnum.Ru].Substring(0, 1).ToUpper();
            if (candidate.PropertyDefinition.Names[MetaModel.Names.HumanLanguageEnum.Ru].Length > 1)
                nameHuman += candidate.PropertyDefinition.Names[MetaModel.Names.HumanLanguageEnum.Ru].Substring(1);
            CSMethod method = new CSMethod()
            {
                Visibility = ElementVisibilityClassic.Public,
                AdditionalKeywords = "virtual",
                ReturnType = isSingle ? in_dotClassName : $"List<{in_dotClassName}>",
                Class = in_storageClass,
                Name = $"ReadBy{NameHelper.NamesToHungarianName(candidate.PropertyDefinition.Names)}",
                DocComment = new XmlComment($"Чтение {(isSingle ? "объекта" : "коллекции объектов")} по значению свойства {nameHuman.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("\'", "&apos;")}"),
                HintSingleLineBody = false,
                IsStatic = false
            };

            CSParameter param = new CSParameter()
            {
                Name = $"in_{propName}",
                Type = "string"
            };
            method.Params.Add(param.Name, param);

            param = new CSParameter()
            {
                Name = $"in_transaction",
                Type = "DbTransaction",
                Value = "null"
            };
            method.Params.Add(param.Name, param);

            in_storageClass.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
            #endregion

            method.BodyStrings.Add($"if (in_{propName} == null)");
            method.BodyStrings.Add($"    throw new ArgumentException(\"{propName}\");");
            method.BodyStrings.Add($"DbParameter[] dbParams = new DbParameter[] {{ orm.Db.DbProviderFactory.Instance.CreateParameter(\"@in_{propName}\", in_{propName}, typeof(string), false) }};");
            method.BodyStrings.Add($"List<{in_dotClassName}> result = ReadAsCollection(");
            method.BodyStrings.Add($"    in_where: {constName} + \" = @in_{propName}\",");
            method.BodyStrings.Add($"    in_params: dbParams,");
            method.BodyStrings.Add($"    in_transaction: in_transaction");
            method.BodyStrings.Add($");");
            method.BodyStrings.Add($"return result{(isSingle ? ".Count != 0 ? result[0] : null" : string.Empty)};");
        }

        public static CSClass CreateStorageClass(CSComponent component, DOTDefinition dotDefinition, DBSchemaMetaModelJson dbModel)
        {
            var dotClassName = CSharpHelper.GenerateDOTClassName(dotDefinition);
            var storageClassName = dotClassName + "Storage";
            var correspondence = (TableAndDOTCorrespondenceJson)dbModel.TableAndSourceCorrespondence[dotDefinition.Id];

            var storageClass = new CSClass
            {
                Component = component,
                DocComment = new XmlComment("Хранилище объектов " + NameHelper.GetLocalNameUpperCase(correspondence.DOTDefinition.Names)),
                Name = storageClassName,
                InheritsFrom = $"DOStorage<{storageClassName}, {dotClassName}>",
                Partial = true
            };
            component.Classes.Add(storageClass.Name, storageClass);

            // Создание констант, соответствующих полям таблицы.
            var decimalConstants = new List<int>();
            var fieldConstants = GenerateFieldConstants(correspondence, storageClass, decimalConstants);

            // Создание конструктора.
            GenerateConstructor(fieldConstants, storageClass, dotClassName, storageClassName, dotDefinition, correspondence, decimalConstants);

            // Метод FillDOFromReader.
            GenerateFillDOFromReader(storageClass, dotClassName, correspondence);

            // Метод FillParameters.
            GenerateFillParameters(storageClass, dotClassName, correspondence);

            // Метод RestoreByName, если применимо.
            GenerateRestoreByNameMethod(storageClass, dotClassName, correspondence);

            return storageClass;
        }

        public Storage(StoragePackage parentPackage, DOTDefinition dotDefinition, string rootNamespace, DBSchemaMetaModelJson dbModel)
        {
            var dotClassName = CSharpHelper.GenerateDOTClassName(dotDefinition);
            var storageClassName = dotClassName + "Storage";

            Package = parentPackage;
            Name = storageClassName + ".cs";
            DOTDefinition = dotDefinition;
            Namespace = $"{rootNamespace}.Model.Storage";

            SystemUsings.Add("System");
            SystemUsings.Add("System.Collections.Generic");
            SystemUsings.Add("System.Data.Common");
            UserUsings.Add("orm.Db");
            //UserUsings.Add("orm.Util");
            UserUsings.Add($"{rootNamespace}.Model.DOT");

            var storageClass = CreateStorageClass(this, dotDefinition, dbModel);
            MainClass = storageClass;
        }
    };
}
