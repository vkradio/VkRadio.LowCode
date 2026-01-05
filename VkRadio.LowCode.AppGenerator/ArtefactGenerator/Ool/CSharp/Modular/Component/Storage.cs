using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Constant;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Package.Model;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;
using VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Component;

public class Storage : CSComponentWMainClass
{
    static List<CSClassConstant> GenerateFieldConstants(TableAndDOTCorrespondenceJson tableAndDotCorrespondence, CSClass storageClass, List<int> decimalConstants)
    {
        var result = new List<CSClassConstant>();

        foreach (var field in tableAndDotCorrespondence.Table.AllFields)
        {
            if (field is PKSingleJson)
            {
                decimalConstants.Add(0);
                continue;
            }

            var vf = field as ValueFieldJson;
            var fk = field as ForeignKeyFieldJson;

            if (vf is null && fk is null)
            {
                throw new GeneratorException($"Table {tableAndDotCorrespondence.Table.Name} contains ITableField {field.Name} that is not of type ValueField or ForeignKeyField: {field.GetType().Name}; PropertyDefinition Id = {tableAndDotCorrespondence.PropertyCorrespondences[0].PropertyDefinition.Id}.");
            }

            var c = new CSClassConstant("string", ElementVisibilityClassic.Public, true)
            {
                Class = storageClass,
                DocComment = new XmlComment($"Table field {NameHelper.GetLocalNameUpperCase(field.DOTPropertyCorrespondence.PropertyDefinition.Names)} (variant without quot symbols)"),
                Name = $"{NameHelper.NameToConstant(field.DOTPropertyCorrespondence.PropertyDefinition.Names, false)}{(fk != null ? "_ID" : string.Empty)}",
                Value = $"\"{field.Name}\""
            };
            storageClass.Constants.Add(c.Name, c);
            result.Add(c);

            c = new CSClassConstant("string", ElementVisibilityClassic.Public, true)
            {
                Class = storageClass,
                DocComment = new XmlComment($"Table field {NameHelper.GetLocalNameUpperCase(field.DOTPropertyCorrespondence.PropertyDefinition.Names)} (variant with quot symbols)"),
                Name = $"{NameHelper.NameToConstant(field.DOTPropertyCorrespondence.PropertyDefinition.Names, false)}{(fk != null ? "_ID" : string.Empty)}_Q",
                Value = $"\"\\\"{field.Name}\\\"\""
            };
            storageClass.Constants.Add(c.Name, c);
            result.Add(c);

            var decimalPositions = 0;
            if (field.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTMoney)
            {
                var pftMoney = (PFTMoney)field.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType;
                if (pftMoney.DecimalPositions == 0)
                    throw new GeneratorException($"Property {field.DOTPropertyCorrespondence.PropertyDefinition.Id} is PFTMoney but has 0 DecimalPositions.");
                decimalPositions = pftMoney.DecimalPositions;
            }
            decimalConstants.Add(decimalPositions);
        }

        return result;
    }

    private static void GenerateConstructor(List<CSClassConstant> fieldConstants, CSClass storageClass, string dotClassName, string storageClassName, DOTDefinition dotDef, TableAndDOTCorrespondenceJson tableAndDotCorrespondence, List<int> decimalConstants)
    {
        var ctor = new CSConstructor(storageClass)
        {
            Class = storageClass,
            DocComment = new XmlComment("Storage constructor"),
            HintSingleLineBody = false,
            Visibility = ElementVisibilityClassic.Public
        };
        storageClass.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

        ctor.BodyStrings.Add($"tableName = \"{tableAndDotCorrespondence.Table.Name}\";");
        ctor.BodyStrings.Add($"tableNameQ = \"\\\"{tableAndDotCorrespondence.Table.Name}\\\"\";");

        foreach (var decConstant in decimalConstants)
            ctor.BodyStrings.Add($"decimalFields.Add({decConstant});");

        #region Database fields
        ctor.BodyStrings.Add("fields = new string[]");
        ctor.BodyStrings.Add("{");

        ctor.BodyStrings.Add($"{CSharpHelper.C_TAB}c_fieldId{(fieldConstants.Count > 0 ? "," : string.Empty)}");

        for (var i = 0; i < fieldConstants.Count; i += 2)
        {
            ctor.BodyStrings.Add(
                string.Format(
                    "{0}{1}{2}",
                    CSharpHelper.C_TAB,
                    fieldConstants[i].Name,
                    i != fieldConstants.Count - 1 ? "," : string.Empty
                )
            );
        }

        ctor.BodyStrings.Add("};");
        #endregion

        #region Database fields in a human language
        ctor.BodyStrings.Add("fieldsHuman = new string[]");
        ctor.BodyStrings.Add("{");

        ctor.BodyStrings.Add($"{CSharpHelper.C_TAB}\"id\"{(fieldConstants.Count > 0 ? "," : string.Empty)}");

        for (var i = 0; i < fieldConstants.Count; i += 2)
        {
            PropertyCorrespondenceJson? corr = null;

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

            ctor.BodyStrings.Add(
                string.Format(
                    "{0}\"{1}\"{2}",
                    CSharpHelper.C_TAB,
                    humanName,
                    (i != fieldConstants.Count - 1) ? "," : string.Empty
                )
            );
        }

        ctor.BodyStrings.Add("};");
        #endregion

        ctor.BodyStrings.Add("InitParams();");

        // Field an Id dictionary of predefined objects
        if (dotDef.PredefinedDOs.Count != 0)
        {
            ctor.BodyStrings.Add(string.Empty);

            foreach (var pdo in dotDef.PredefinedDOs)
            {
                ctor.BodyStrings.Add($"predefinedObjects.Add(new Guid(\"{pdo.Id}\"), null);");
            }
        }

        #region Create a default ordering
        var orderByProperty = GeneralHelper.GetListSortProperty(dotDef, out bool reverseOrder);

        if (orderByProperty is not null)
        {
            var quoteSymbol = tableAndDotCorrespondence.DBSchemaMetaModel.SchemaDeploymentScript.QuoteSymbol;

            if (quoteSymbol == "\"")
            {
                quoteSymbol = "\\" + quoteSymbol;
            }

            ctor.BodyStrings.Add($"defaultOrderBy = \"{quoteSymbol}{NameHelper.NameToUnderscoreSeparatedName(orderByProperty.Names)}{quoteSymbol}{(reverseOrder ? " desc" : string.Empty)}\";");
        }
        #endregion
    }

    private static void GenerateFillDOFromReader(CSClass storageClass, string dotClassName, TableAndDOTCorrespondenceJson tableAndDotCorrespondence)
    {
        #region Method header
        var method = new CSMethod
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

        var param = new CSParameter
        {
            Name = "reader",
            Type = "DbDataReader"
        };
        method.Params.Add(param.Name, param);

        param = new CSParameter
        {
            Name = "@object",
            Type = dotClassName
        };
        method.Params.Add(param.Name, param);

        storageClass.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
        #endregion

        method.BodyStrings.Add("var factory = orm.Db.DbProviderFactory.Instance;");

        var dotDef = tableAndDotCorrespondence.DOTDefinition;

        for (var i = 1; i < tableAndDotCorrespondence.Table.AllFields.Count; i++)
        {
            // Search for a table field
            var field = tableAndDotCorrespondence.Table.AllFields[i];
            var corr = tableAndDotCorrespondence.PropertyCorrespondences.SingleOrDefault(c => c.TableField.Name == field.Name);

            var isId = false;
            var isNullable = corr.PropertyDefinition.FunctionalType.Nullable;
            string rightPart;

            if (!(corr.PropertyDefinition.FunctionalType is PFTLink))
            {
                if (corr.PropertyDefinition.FunctionalType is PFTBoolean)
                {
                    rightPart = $"factory.ReadBool{(isNullable ? "Nullable" : string.Empty)}FromReader(reader, {i});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTDateTime)
                {
                    rightPart = $"factory.ReadDateTime{(isNullable ? "Nullable" : string.Empty)}FromReader(reader, {i});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTDecimal)
                {
                    rightPart = $"factory.ReadDecimal{(isNullable ? "Nullable" : string.Empty)}FromReader(reader, {i});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTInteger)
                {
                    rightPart = $"factory.ReadInt{(isNullable ? "Nullable" : string.Empty)}FromReader(reader, {i});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTString)
                {
                    rightPart = $"factory.ReadStringFromReader(reader, {i}, {(isNullable ? "true" : "false")});";
                }
                else if (corr.PropertyDefinition.FunctionalType is PFTUniqueCode)
                {
                    rightPart = $"factory.ReadGuid{(isNullable ? "Nullable" : string.Empty)}FromReader(reader, {i});";
                }
                else
                {
                    throw new GeneratorException($"Unexpected non-reference PropertyFunctionalType {corr.PropertyDefinition.FunctionalType.GetType().Name} in PropertyDefinition Id {corr.PropertyDefinition.Id} for constructing FillDOFromReader.");
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
                    rightPart = $"factory.ReadGuid{(isNullable ? "Nullable" : string.Empty)}FromReader(reader, {i});";
                }
                else
                {
                    throw new GeneratorException($"Unexpected reference type: {corr.PropertyDefinition.FunctionalType.GetType().Name} in PropertyDefinition Id {corr.PropertyDefinition.Id} when constructing values for FillDOFromReader.");
                }
            }

            var propName = NameHelper.NamesToHungarianName(corr.PropertyDefinition.Names);

            if (isId)
            {
                propName += "Id";
            }

            method.BodyStrings.Add($"@object.{propName} = {rightPart}");
        }
    }

    private static void GenerateFillParameters(CSClass storageClass, string dotClassName, TableAndDOTCorrespondenceJson tableAndDotCorrespondence)
    {
        #region Method header
        var method = new CSMethod
        {
            AdditionalKeywords = "override",
            Class = storageClass,
            DocComment = new XmlComment("Fill parameters of data objects to be saved to a database"),
            HintSingleLineBody = false,
            IsStatic = false,
            Name = "FillParameters",
            ReturnType = "void",
            Visibility = ElementVisibilityClassic.Protected
        };

        var param = new CSParameter
        {
            Name = "@params",
            Type = "DbParameterCollection"
        };
        method.Params.Add(param.Name, param);

        param = new CSParameter
        {
            Name = "@object",
            Type = dotClassName
        };
        method.Params.Add(param.Name, param);

        storageClass.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
        #endregion

        method.BodyStrings.Add("var factory = orm.Db.DbProviderFactory.Instance;");

        var dotDef = tableAndDotCorrespondence.DOTDefinition;

        for (var i = 1; i < tableAndDotCorrespondence.Table.AllFields.Count; i++)
        {
            // Search for a database field
            var field = tableAndDotCorrespondence.Table.AllFields[i];
            var corr = tableAndDotCorrespondence.PropertyCorrespondences.SingleOrDefault(c => c.TableField.Name == field.Name);

            CSharpHelper.GetTypeInfoForFillDOFromReaderRow(corr.PropertyDefinition, out bool isNullable, out bool isId, out string typeName);

            var propName = NameHelper.NamesToHungarianName(corr.PropertyDefinition.Names);

            if (isId)
            {
                var propNameWOId = propName;
                propName += "Id";
                method.BodyStrings.Add($"if (!@object.{propNameWOId}Id.HasValue && @object.{propNameWOId} != null)");
                method.BodyStrings.Add($"{CSharpHelper.C_TAB}@params.Add(factory.CreateParameter(this.@params[{i}], @object.{propNameWOId}.Id.Value, typeof(Guid?), true));");
                method.BodyStrings.Add("else");
                method.BodyStrings.Add($"{CSharpHelper.C_TAB}@params.Add(factory.CreateParameter(this.@params[{i}], @object.{propName}, typeof({typeName}), {(isNullable ? "true" : "false")}));");
            }
            else
            {
                method.BodyStrings.Add($"@params.Add(factory.CreateParameter(this.@params[{i}], @object.{propName}, typeof({typeName}), {(isNullable ? "true" : "false")}));");
            }
        }
    }

    private static void GenerateRestoreByNameMethod(CSClass storageClass, string dotClassName, TableAndDOTCorrespondenceJson tableAndDotCorrespondence)
    {
        // 1. Getting know whether a data object type has a name-like field
        PropertyCorrespondenceJson? candidate = null;

        foreach (var c in tableAndDotCorrespondence.PropertyCorrespondences)
        {
            if (c.PropertyDefinition.FunctionalType is PFTName)
            {
                if (candidate == null || (!candidate.PropertyDefinition.FunctionalType.Unique && c.PropertyDefinition.FunctionalType.Unique))
                {
                    candidate = c;
                }
            }
        }

        if (candidate is null) // If name-like field not found, there is no sense to read object by name
        {
            return;
        }

        // 2. Create a method
        #region Method header
        var propName = NameHelper.NamesToHungarianName(candidate.PropertyDefinition.Names, false);
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
            ReturnType = isSingle ? dotClassName : $"List<{dotClassName}>",
            Class = storageClass,
            Name = $"ReadBy{NameHelper.NamesToHungarianName(candidate.PropertyDefinition.Names)}",
            DocComment = new XmlComment($"Read {(isSingle ? "object" : "object collection")} by a value of property {nameHuman.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("\'", "&apos;")}"),
            HintSingleLineBody = false,
            IsStatic = false
        };

        var param = new CSParameter
        {
            Name = propName,
            Type = "string"
        };
        method.Params.Add(param.Name, param);

        param = new CSParameter
        {
            Name = $"transaction",
            Type = "DbTransaction",
            Value = "null"
        };
        method.Params.Add(param.Name, param);

        storageClass.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);
        #endregion

        method.BodyStrings.Add($"if ({propName} == null)");
        method.BodyStrings.Add($"    throw new ArgumentException(\"{propName}\");");
        method.BodyStrings.Add($"var dbParams = new DbParameter[] {{ orm.Db.DbProviderFactory.Instance.CreateParameter(\"@in_{propName}\", {propName}, typeof(string), false) }};");
        method.BodyStrings.Add($"var result = ReadAsCollection(");
        method.BodyStrings.Add($"    where: {constName} + \" = @in_{propName}\",");
        method.BodyStrings.Add($"    @params: dbParams,");
        method.BodyStrings.Add($"    transaction: transaction");
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
            DocComment = new XmlComment("Object storage " + NameHelper.GetLocalNameUpperCase(correspondence.DOTDefinition.Names)),
            Name = storageClassName,
            InheritsFrom = $"DOStorage<{storageClassName}, {dotClassName}>",
            Partial = true
        };
        component.Classes.Add(storageClass.Name, storageClass);

        // Create constants that corresponds to table fields
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
}
