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
            new CSharpValueType(TypeConstants.Value.C_INT, false),
            "virtual",
            null,
            null,
            false,
            true
        )
    {
        DocComment!.Text += preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? " (подсчет количества объектов)" : " (count an amount of objects)";

        var param = new CSParameter("additionalWhere", new CSharpReferenceType(supportNullableRefTypes, TypeConstants.Reference.C_STRING, true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("@params", new CSharpReferenceType(supportNullableRefTypes, "DbParameter[]", true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("transaction", new CSharpReferenceType(supportNullableRefTypes, "DbTransaction", true), "null");
        Params.Add(param.Name, param);

        var basePropName = NameHelper.NamesToPascalCase(propDef.Names, true);
        var entityClassName = TypeHelper.GetClassNameForEntity(propDef);
        BodyStrings.Add($"StorageRegistry.Instance.{entityClassName}Storage.GetCountForParent({basePropName}Filter, additionalWhere, @params, transaction);");
    }
}
