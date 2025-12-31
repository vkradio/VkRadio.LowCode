using System;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Type;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method
{
    public class CSCollectionGetter : CSMethod
    {
        public static CSCollectionGetter Instantiate(CSClass in_class, PropertyDefinition in_propDef)
        {
            if (!(in_propDef.FunctionalType is PFTBackReferencedTable || in_propDef.FunctionalType is PFTTablePart))
                throw new ArgumentException($"in_propDef.FunctionalType is not table/collection ({in_propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {in_propDef.Id}).");

            var basePropName = NameHelper.NamesToHungarianName(in_propDef.Names, true);
            var dotClassName = TypeHelper.PropertyDefinitionToCSDOTType(in_propDef);

            var getter = new CSCollectionGetter
            {
                _class = in_class,
                _docComment = TypeHelper.GetXmlCommentForTablePropDef(in_propDef),
                _name = "Get" + basePropName,
                ReturnType = $"List<{dotClassName}>",
                AdditionalKeywords = "virtual",
                _hintSingleLineBody = true,
                _visibility = ElementVisibilityClassic.Public
            };
            getter.DocComment.Text += " (чтение коллекции)";

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
    };
}
