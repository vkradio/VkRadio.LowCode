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
            new CSharpReferenceType(supportNullableRefTypes, $"List<{TypeHelper.GetClassNameForEntity(propDef)}>", false),
            "virtual",
            null,
            null,
            false,
            true
        )
    {
        if (!(propDef.FunctionalType is PFTBackReferencedTable || propDef.FunctionalType is PFTTablePart))
        {
            throw new ArgumentException(
                $"{nameof(propDef)}.{nameof(propDef.FunctionalType)} is not a table/collection ({propDef.FunctionalType.GetType().Name} for {nameof(PropertyDefinition)} {nameof(propDef.Id)} {propDef.Id}).",
                nameof(propDef)
            );
        }

        DocComment!.Text += preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? " (чтение коллекции)" : " (read collection)";

        var param = new CSParameter("additionalWhere", new CSharpReferenceType(supportNullableRefTypes, TypeConstants.Reference.C_STRING, true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("@params", new CSharpReferenceType(supportNullableRefTypes, "DbParameter[]", true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("orderBy", new CSharpReferenceType(supportNullableRefTypes, TypeConstants.Reference.C_STRING, true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("transaction", new CSharpReferenceType(supportNullableRefTypes, "DbTransaction", true), "null");
        Params.Add(param.Name, param);

        param = new CSParameter("doNotUseDefaultOrder", new CSharpValueType(TypeConstants.Value.C_BOOL, false), "false");
        Params.Add(param.Name, param);

        param = new CSParameter("selectTop", new CSharpValueType(TypeConstants.Value.C_INT, false), "0");
        Params.Add(param.Name, param);

        var basePropName = NameHelper.NamesToPascalCase(propDef.Names, true);
        var entityClassName = TypeHelper.GetClassNameForEntity(propDef);
        BodyStrings.Add($"StorageRegistry.Instance.{entityClassName}Storage.ReasAsCollectionForParent({basePropName}Filter, additionalWhere, @params, orderBy, transaction, doNotUseDefaultOrder, selectTop);");
    }
}
