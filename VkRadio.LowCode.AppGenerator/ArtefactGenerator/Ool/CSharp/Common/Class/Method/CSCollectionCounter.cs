using System;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Type;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method
{
    public class CSCollectionCounter : CSMethod
    {
        public static CSCollectionCounter Instantiate(CSClass in_class, PropertyDefinition in_propDef)
        {
            if (!(in_propDef.FunctionalType is PFTBackReferencedTable || in_propDef.FunctionalType is PFTTablePart))
                throw new ArgumentException($"in_propDef.FunctionalType is not table/collection ({in_propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {in_propDef.Id}).");

            var basePropName = NameHelper.NamesToHungarianName(in_propDef.Names, true);
            var dotClassName = TypeHelper.PropertyDefinitionToCSDOTType(in_propDef);

            var counter = new CSCollectionCounter
            {
                _class = in_class,
                _docComment = TypeHelper.GetXmlCommentForTablePropDef(in_propDef),
                _name = "Get" + basePropName + "Count",
                ReturnType = "int",
                AdditionalKeywords = "virtual",
                _hintSingleLineBody = true,
                _visibility = ElementVisibilityClassic.Public
            };
            counter.DocComment.Text += " (подсчет количества объектов)";

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
    };
}
