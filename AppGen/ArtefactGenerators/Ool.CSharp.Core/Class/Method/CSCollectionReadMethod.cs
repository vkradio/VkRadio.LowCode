using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;

public class CSCollectionReadMethod : CSMethod
{
    public CSCollectionReadMethod(CSClass cSharpClass, PropertyDefinition propDef, NaturalLanguageEnum preferNaturalLanguageForComments, bool supportNullableRefTypes)
        : base(
            cSharpClass,
            $"Get{NameHelper.NamesToPascalCase(propDef.Names, true)}",
            TypeHelper.GetXmlCommentForTablePropDef(propDef, preferNaturalLanguageForComments),
            ElementVisibilityClassic.Public,
            new CSharpReferenceDomainListType(supportNullableRefTypes, propDef),
            "virtual",
            null,
            null,
            false,
            true
        )
    {
        if (propDef.FunctionalType is not IPFTTable)
        {
            throw new ArgumentException(
                $"{nameof(propDef)}.{nameof(propDef.FunctionalType)} is not a table/collection ({propDef.FunctionalType.GetType().Name} for {nameof(PropertyDefinition)} {nameof(propDef.Id)} {propDef.Id}).",
                nameof(propDef)
            );
        }

        DocComment!.Text += preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? " (чтение коллекции)" : " (read collection)";

        var param = new CSParameter("additionalWhere", new CSharpReferenceTechnicalType(supportNullableRefTypes, TypeEnum.String, true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("@params", new CSharpReferenceTechnicalType(supportNullableRefTypes, "DbParameter[]", true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("orderBy", new CSharpReferenceTechnicalType(supportNullableRefTypes, TypeEnum.String, true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("transaction", new CSharpReferenceTechnicalType(supportNullableRefTypes, "DbTransaction", true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("doNotUseDefaultOrder", new CSharpValueType(TypeEnum.Bool, false), "false");
        Params.Add(param.Name, param);

        param = new CSParameter("selectTop", new CSharpValueType(TypeEnum.Int32, false), "0");
        Params.Add(param.Name, param);

        var basePropName = NameHelper.NamesToPascalCase(propDef.Names, true);
        BodyStrings.Add($"StorageRegistry.Instance.{((CSharpReferenceDomainListType)ReturnType).EntityName}Storage.ReasAsCollectionForParent({basePropName}Filter, additionalWhere, @params, orderBy, transaction, doNotUseDefaultOrder, selectTop);");
    }
}
