using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Constant;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Model;
using VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component;

public class Storage : CSComponentWMainClass
{
    private static List<CSClassConstant> GenerateFieldConstants(TableAndDOTCorrespondence tableAndDotCorrespondence, CSClass storageClass, List<int> decimalConstants)
    {
        var result = new List<CSClassConstant>();

        foreach (var field in tableAndDotCorrespondence.Table.AllFields)
        {
            if (field is PKSingle)
            {
                decimalConstants.Add(0);
                continue;
            }

            var vf = field as ValueField;
            var fk = field as ForeignKeyField;

            if (vf is null && fk is null)
            {
                throw new ApplicationException(string.Format("Table {0} contains ITableField {1} that is not of type ValueField or ForeignKeyField: {2}; PropertyDefinition Id = {3}.", tableAndDotCorrespondence.Table.Name, field.Name, field.GetType().Name, tableAndDotCorrespondence.PropertyCorrespondences[0].PropertyDefinition.Id));
            }

            var c = new CSClassConstant("string", ElementVisibilityClassic.Public, true)
            {
                Class = storageClass,
                DocComment = new XmlComment(string.Format("Table field {0} (variant without quot chars)", NameHelper.GetLocalNameUpperCase(field.DOTPropertyCorrespondence.PropertyDefinition.Names))),
                Name = string.Format("{0}{1}", NameHelper.NameToConstant(field.DOTPropertyCorrespondence.PropertyDefinition.Names, false), fk != null ? "_ID" : string.Empty),
                Value = string.Format("\"{0}\"", field.Name)
            };
            storageClass.Constants.Add(c.Name, c);
            result.Add(c);

            c = new CSClassConstant("string", ElementVisibilityClassic.Public, true)
            {
                Class = storageClass,
                DocComment = new XmlComment(string.Format("Table field {0} (variant with quot chars)", NameHelper.GetLocalNameUpperCase(field.DOTPropertyCorrespondence.PropertyDefinition.Names))),
                Name = string.Format("{0}{1}_Q", NameHelper.NameToConstant(field.DOTPropertyCorrespondence.PropertyDefinition.Names, false), fk != null ? "_ID" : string.Empty),
                Value = string.Format("\"\\\"{0}\\\"\"", field.Name)
            };
            storageClass.Constants.Add(c.Name, c);
            result.Add(c);

            var decimalPositions = 0;

            if (field.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTMoney)
            {
                var pftMoney = (PFTMoney)field.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType;

                if (pftMoney.DecimalPositions == 0)
                {
                    throw new ApplicationException(string.Format("Property {0} is PFTMoney but has 0 DecimalPositions.", field.DOTPropertyCorrespondence.PropertyDefinition.Id));
                }

                decimalPositions = pftMoney.DecimalPositions;
            }

            decimalConstants.Add(decimalPositions);
        }

        return result;
    }

    private static void GenerateConstructor(List<CSClassConstant> fieldConstants, CSClass storageClass, string dotClassName, string storageClassName, DOTDefinition dotDef, TableAndDOTCorrespondence tableAndDotCorrespondence, List<int> decimalConstants)
    {
        var ctor = new CSConstructor(storageClass)
        {
            Class = storageClass,
            DocComment = new XmlComment("Storage constructor"),
            HintSingleLineBody = false,
            Visibility = ElementVisibilityClassic.Public
        };
        storageClass.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

        ctor.BodyStrings.Add(string.Format("_tableName = \"{0}\";", tableAndDotCorrespondence.Table.Name));
        ctor.BodyStrings.Add(string.Format("_tableNameQ = \"\\\"{0}\\\"\";", tableAndDotCorrespondence.Table.Name));

        foreach (var decConstant in decimalConstants)
        {
            ctor.BodyStrings.Add(string.Format("_decimalFields.Add({0});", decConstant));
        }

        #region Database fields
        ctor.BodyStrings.Add("_fields = new string[]");
        ctor.BodyStrings.Add("{");

        ctor.BodyStrings.Add(string.Format("{0}c_fieldId{1}", CSharpHelper.C_TAB, fieldConstants.Count > 0 ? "," : string.Empty));

        for (var i = 0; i < fieldConstants.Count; i += 2)
        {
            ctor.BodyStrings.Add(string.Format(
                "{0}{1}{2}",
                CSharpHelper.C_TAB,
                fieldConstants[i].Name,
                i != fieldConstants.Count - 1 ? "," : string.Empty
            ));
        }

        ctor.BodyStrings.Add("};");
        #endregion

        #region Database fields in human language
        ctor.BodyStrings.Add("_fieldsHuman = new string[]");
        ctor.BodyStrings.Add("{");

        ctor.BodyStrings.Add(string.Format("{0}\"id\"{1}", CSharpHelper.C_TAB, fieldConstants.Count > 0 ? "," : string.Empty));

        for (var i = 0; i < fieldConstants.Count; i += 2)
        {
            PropertyCorrespondence? corr = null;

            for (var j = 0; j < tableAndDotCorrespondence.PropertyCorrespondences.Count; j++)
            {
                var fc = fieldConstants[i].Value.Substring(1, fieldConstants[i].Value.Length - 2);

                if (tableAndDotCorrespondence.PropertyCorrespondences[j].TableField.Name == fc)
                {
                    corr = tableAndDotCorrespondence.PropertyCorrespondences[j];
                    break;
                }
            }
            
            var humanName = corr.PropertyDefinition.Names[MetaModel.Names.HumanLanguageEnum.Ru].Replace("\"", "\\\"");

            ctor.BodyStrings.Add(string.Format(
                "{0}\"{1}\"{2}",
                CSharpHelper.C_TAB,
                humanName,
                (i != fieldConstants.Count - 1) ? "," : string.Empty
            ));
        }

        ctor.BodyStrings.Add("};");
        #endregion

        ctor.BodyStrings.Add("InitParams();");

        // Fill an Id dictionary of predefined objects
        if (dotDef.PredefinedDOs.Count != 0)
        {
            ctor.BodyStrings.Add(string.Empty);

            foreach (var pdo in dotDef.PredefinedDOs)
            {
                ctor.BodyStrings.Add(string.Format("_predefinedObjects.Add(new Guid(\"{0}\"), null);", pdo.Id));
            }
        }

        #region Creating a default ordering
        bool reverseOrder;
        var orderByProperty = GeneralHelper.GetListSortProperty(dotDef, out reverseOrder);

        if (orderByProperty is not null)
        {
            var quoteSymbol = tableAndDotCorrespondence.DBSchemaMetaModel.SchemaDeploymentScript.QuoteSymbol;

            if (quoteSymbol == "\"")
            {
                quoteSymbol = "\\" + quoteSymbol;
            }

            ctor.BodyStrings.Add(string.Format("_defaultOrderBy = \"{0}{1}{0}{2}\";", quoteSymbol, NameHelper.NameToUnderscoreSeparatedName(orderByProperty.Names), reverseOrder ? " desc" : string.Empty));
        }
        #endregion
    }
    
    private static void GenerateFillDOFromReader(CSClass storageClass, string dotClassName, TableAndDOTCorrespondence tableAndDotCorrespondence)
    {
        #region Method heading
        var method = new CSMethod()
        {
            AdditionalKeywords = "override",
            Class = storageClass,
            DocComment = new XmlComment("Fill data object properties from DbDataReader"),
            HintSingleLineBody = false,
            IsStatic = false,
            Name = "FillDOFromReader",
            ReturnType = "void",
            Visibility = ElementVisibilityClassic.Public
        };

        var param = new CSParameter()
        {
            Name = "in_reader",
            Type = "DbDataReader"
        };
        method.Params.Add(param.Name, param);

        param = new CSParameter()
        {
            Name = "in_o",
            Type = dotClassName
        };
        method.Params.Add(param.Name, param);

        storageClass.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
        #endregion

        method.BodyStrings.Add("orm.Db.DbProviderFactory factory = orm.Db.DbProviderFactory.Instance;");

        var dotDef = tableAndDotCorrespondence.DOTDefinition;

        for (var i = 1; i < tableAndDotCorrespondence.Table.AllFields.Count; i++)
        {
            #region Search for table field
            var field = tableAndDotCorrespondence.Table.AllFields[i];
            PropertyCorrespondence? corr = null;

            foreach (var c in tableAndDotCorrespondence.PropertyCorrespondences)
            {
                if (c.TableField.Name == field.Name)
                {
                    corr = c;
                    break;
                }
            }
            #endregion

            var isId = false;
            var isNullable = corr.PropertyDefinition.FunctionalType.Nullable;
            string rightPart;

            if (!(corr.PropertyDefinition.FunctionalType is PFTLink))
            {
                if (corr.PropertyDefinition.FunctionalType is PFTBoolean)
                {
                    rightPart = $"factory.ReadBool{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTDateTime)
                {
                    rightPart = $"factory.ReadDateTime{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTDecimal)
                {
                    rightPart = $"factory.ReadDecimal{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTInteger)
                {
                    rightPart = $"factory.ReadInt{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTString)
                {
                    rightPart = $"factory.ReadStringFromReader(in_reader, {i}, {(isNullable ? "true" : "false")});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTUniqueCode)
                {
                    rightPart = $"factory.ReadGuid{(isNullable ? "Nullable" : string.Empty)}FromReader(in_reader, {i});";
                }
                else
                {
                    throw new ApplicationException(string.Format("Unexpected non-reference PropertyFunctionalType {0} in PropertyDefinition Id {1} for constructing FillDOFromReader.", corr.PropertyDefinition.FunctionalType.GetType().Name, corr.PropertyDefinition.Id));
                }
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

            var propName = NameHelper.NamesToPascalCase(corr.PropertyDefinition.Names);

            if (isId)
            {
                propName += "Id";
            }

            method.BodyStrings.Add($"in_o.{propName} = {rightPart}");
        }
    }

    private static void GenerateFillParameters(CSClass storageClass, string dotClassName, TableAndDOTCorrespondence tableAndDotCorrespondence)
    {
        #region Method heading
        var method = new CSMethod()
        {
            AdditionalKeywords = "override",
            Class = storageClass,
            DocComment = new XmlComment("Fill parameters for writing the state of a data object to a database"),
            HintSingleLineBody = false,
            IsStatic = false,
            Name = "FillParameters",
            ReturnType = "void",
            Visibility = ElementVisibilityClassic.Protected
        };

        var param = new CSParameter()
        {
            Name = "in_params",
            Type = "DbParameterCollection"
        };
        method.Params.Add(param.Name, param);

        param = new CSParameter()
        {
            Name = "in_o",
            Type = dotClassName
        };
        method.Params.Add(param.Name, param);

        storageClass.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
        #endregion

        method.BodyStrings.Add("orm.Db.DbProviderFactory factory = orm.Db.DbProviderFactory.Instance;");

        var dotDef = tableAndDotCorrespondence.DOTDefinition;

        for (var i = 1; i < tableAndDotCorrespondence.Table.AllFields.Count; i++)
        {
            #region Search for a table field
            var field = tableAndDotCorrespondence.Table.AllFields[i];
            PropertyCorrespondence? corr = null;

            foreach (var c in tableAndDotCorrespondence.PropertyCorrespondences)
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

            var propName = NameHelper.NamesToPascalCase(corr.PropertyDefinition.Names);

            if (isId)
            {
                var propNameWOId = propName;
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

    private static void GenerateRestoreByNameMethod(CSClass in_storageClass, string in_dotClassName, TableAndDOTCorrespondence in_tableAndDotCorrespondence)
    {
        // 1. Detect do the data object type has a field that corresponds to its name
        PropertyCorrespondence? candidate = null;

        foreach (var c in in_tableAndDotCorrespondence.PropertyCorrespondences)
        {
            if (c.PropertyDefinition.FunctionalType is PFTName)
            {
                if (candidate is null || (!candidate.PropertyDefinition.FunctionalType.Unique && c.PropertyDefinition.FunctionalType.Unique))
                {
                    candidate = c;
                }
            }
        }

        if (candidate is null) // If no name-field, then the method for extracting an object by name has no sense
        {
            return;
        }

        // 2. Create a method
        #region Method heading
        var propName = NameHelper.NamesToPascalCase(candidate.PropertyDefinition.Names, false);
        var constName = NameHelper.NameToConstant(candidate.PropertyDefinition.Names, false) + "_Q";
        var isSingle = candidate.PropertyDefinition.FunctionalType.Unique;
        var nameHuman = candidate.PropertyDefinition.Names[MetaModel.Names.HumanLanguageEnum.Ru].Substring(0, 1).ToUpper();

        if (candidate.PropertyDefinition.Names[MetaModel.Names.HumanLanguageEnum.Ru].Length > 1)
        {
            nameHuman += candidate.PropertyDefinition.Names[MetaModel.Names.HumanLanguageEnum.Ru].Substring(1);
        }

        var method = new CSMethod
        {
            Visibility = ElementVisibilityClassic.Public,
            AdditionalKeywords = "virtual",
            ReturnType = isSingle ? in_dotClassName : $"List<{in_dotClassName}>",
            Class = in_storageClass,
            Name = $"ReadBy{NameHelper.NamesToPascalCase(candidate.PropertyDefinition.Names)}",
            DocComment = new XmlComment($"Reading {(isSingle ? "object" : "collection of objects")} by a value of a property {nameHuman.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("\'", "&apos;")}"),
            HintSingleLineBody = false,
            IsStatic = false
        };

        var param = new CSParameter
        {
            Name = $"in_{propName}",
            Type = "string"
        };
        method.Params.Add(param.Name, param);

        param = new CSParameter
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

    public static CSClass CreateStorageClass(CSComponent component, DOTDefinition dotDefinition, DBSchemaMetaModel dbModel)
    {
        var dotClassName = CSharpHelper.GenerateDOTClassName(dotDefinition);
        var storageClassName = dotClassName + "Storage";
        var correspondence = (TableAndDOTCorrespondence)dbModel.TableAndSourceCorrespondence[dotDefinition.Id];

        var storageClass = new CSClass
        {
            Component = component,
            DocComment = new XmlComment("Storage of objects " + NameHelper.GetLocalNameUpperCase(correspondence.DOTDefinition.Names)),
            Name = storageClassName,
            InheritsFrom = $"DOStorage<{storageClassName}, {dotClassName}>",
            Partial = true
        };
        component.Classes.Add(storageClass.Name, storageClass);

        // Create constants corresponding to table fields
        var decimalConstants = new List<int>();
        var fieldConstants = GenerateFieldConstants(correspondence, storageClass, decimalConstants);

        // Create constructor
        GenerateConstructor(fieldConstants, storageClass, dotClassName, storageClassName, dotDefinition, correspondence, decimalConstants);

        // FillDOFromReader method
        GenerateFillDOFromReader(storageClass, dotClassName, correspondence);

        // FillParameters method
        GenerateFillParameters(storageClass, dotClassName, correspondence);

        // RestoreByName method, if applicable
        GenerateRestoreByNameMethod(storageClass, dotClassName, correspondence);

        return storageClass;
    }

    public Storage(StoragePackage parentPackage, DOTDefinition dotDefinition, string rootNamespace, DBSchemaMetaModel dbModel)
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
}
