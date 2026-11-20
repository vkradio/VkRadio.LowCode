using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;

public class CSCollectionCountMethod : CSMethod
{
    public CSCollectionCountMethod(CSClass cSharpClass, PropertyDefinition propDef, NaturalLanguageEnum preferNaturalLanguageForComments, bool supportNullableRefTypes)
        : base(
            cSharpClass,
            $"Get{NameHelper.NamesToPascalCase(propDef.Names, true)}Count",
            TypeHelper.GetXmlCommentForTablePropDef(propDef, preferNaturalLanguageForComments),
            ElementVisibilityClassic.Public,
            new CSharpValueType(TypeEnum.Int32, false),
            "virtual",
            null,
            null,
            false,
            true
        )
    {
        DocComment!.Text += preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? " (подсчет количества объектов)" : " (count an amount of objects)";

        var param = new CSParameter("additionalWhere", new CSharpReferenceTechnicalType(supportNullableRefTypes, TypeEnum.String, true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("@params", new CSharpReferenceTechnicalType(supportNullableRefTypes, "DbParameter[]", true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("transaction", new CSharpReferenceTechnicalType(supportNullableRefTypes, "DbTransaction", true), "null");
        Params.Add(param.Name, param);

        var basePropName = NameHelper.NamesToPascalCase(propDef.Names, true);
        var entityClassName = propDef.GetEntityClassName();
        BodyStrings.Add($"StorageRegistry.Instance.{entityClassName}Storage.GetCountForParent({basePropName}Filter, additionalWhere, @params, transaction);");
    }
}
