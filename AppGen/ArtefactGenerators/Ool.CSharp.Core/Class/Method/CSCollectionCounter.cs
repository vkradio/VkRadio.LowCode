using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;

public class CSCollectionCounter : CSMethod
{
    public static CSCollectionCounter Instantiate(CSClass cSharpClass, PropertyDefinition propDef)
    {
        if (!(propDef.FunctionalType is PFTBackReferencedTable || propDef.FunctionalType is PFTTablePart))
        {
            throw new ArgumentException($"in_propDef.FunctionalType is not table/collection ({propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {propDef.Id}).");
        }

        var basePropName = NameHelper.NamesToPascalCase(propDef.Names, true);
        var dotClassName = TypeHelper.PropertyDefinitionToCSEntityType(propDef);

        var counter = new CSCollectionCounter
        {
            _class = cSharpClass,
            _docComment = TypeHelper.GetXmlCommentForTablePropDef(propDef),
            _name = "Get" + basePropName + "Count",
            ReturnType = "int",
            AdditionalKeywords = "virtual",
            _hintSingleLineBody = true,
            _visibility = ElementVisibilityClassic.Public
        };
        counter.DocComment.Text += " (count an amount of objects)";

        var param = new CSParameter
        {
            Type = "string",
            Name = "additionalWhere",
            Value = "null"
        };
        counter.Params.Add(param.Name, param);
        param = new CSParameter
        {
            Type = "DbParameter[]",
            Name = "@params",
            Value = "null"
        };
        counter.Params.Add(param.Name, param);
        param = new CSParameter
        {
            Type = "DbTransaction",
            Name = "transaction",
            Value = "null"
        };
        counter.Params.Add(param.Name, param);

        counter.BodyStrings.Add($"StorageRegistry.Instance.{dotClassName}Storage.GetCountForParent({basePropName}Filter, additionalWhere, @params, transaction);");

        return counter;
    }
}
