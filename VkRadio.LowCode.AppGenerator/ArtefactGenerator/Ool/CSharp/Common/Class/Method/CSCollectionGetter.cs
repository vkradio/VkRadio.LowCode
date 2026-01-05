using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Type;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;

public class CSCollectionGetter : CSMethod
{
    public static CSCollectionGetter Instantiate(CSClass @class, PropertyDefinition propDef)
    {
        if (!(propDef.FunctionalType is PFTBackReferencedTable || propDef.FunctionalType is PFTTablePart))
        {
            throw new ArgumentException($"in_propDef.FunctionalType is not table/collection ({propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {propDef.Id}).");
        }

        var basePropName = NameHelper.NamesToPascalCase(propDef.Names, true);
        var dotClassName = TypeHelper.PropertyDefinitionToCSDOTType(propDef);

        var getter = new CSCollectionGetter
        {
            _class = @class,
            _docComment = TypeHelper.GetXmlCommentForTablePropDef(propDef),
            _name = "Get" + basePropName,
            ReturnType = $"List<{dotClassName}>",
            AdditionalKeywords = "virtual",
            _hintSingleLineBody = true,
            _visibility = ElementVisibilityClassic.Public
        };
        getter.DocComment.Text += " (read collection)";

        var param = new CSParameter
        {
            Type = "string",
            Name = "additionalWhere",
            Value = "null"
        };
        getter.Params.Add(param.Name, param);
        param = new CSParameter
        {
            Type = "DbParameter[]",
            Name = "@params",
            Value = "null"
        };
        getter.Params.Add(param.Name, param);
        param = new CSParameter
        {
            Type = "string",
            Name = "orderBy",
            Value = "null"
        };
        getter.Params.Add(param.Name, param);
        param = new CSParameter
        {
            Type = "DbTransaction",
            Name = "transaction",
            Value = "null"
        };
        getter.Params.Add(param.Name, param);
        param = new CSParameter
        {
            Type = "bool",
            Name = "doNotUseDefaultOrder",
            Value = "false"
        };
        getter.Params.Add(param.Name, param);
        param = new CSParameter
        {
            Type = "int",
            Name = "selectTop",
            Value = "0"
        };
        getter.Params.Add(param.Name, param);

        getter.BodyStrings.Add($"StorageRegistry.Instance.{dotClassName}Storage.ReasAsCollectionForParent({basePropName}Filter, additionalWhere, @params, orderBy, transaction, doNotUseDefaultOrder, selectTop);");
        
        return getter;
    }
}
