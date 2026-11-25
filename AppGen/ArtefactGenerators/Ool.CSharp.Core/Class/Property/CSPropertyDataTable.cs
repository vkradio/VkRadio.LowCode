using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;

public class CSPropertyDataTable : CSPropertyPredefined, IPropertyOrField
{
    private readonly IPropertyDefinition _propertyDefinition;

    private CSharpTypeBase? _type;

    public static CSharpReferenceDomainTableType GetPropertyType(IPropertyDefinition propDef) => new CSharpReferenceDomainTableType(false, propDef);

    private static string PropertyName(PropertyDefinition propDef) => $"{NameHelper.NamesToPascalCase(propDef.Names, true)}DataTable";

    private static string NameOfStorageClass(PropertyDefinition propDef)
    {
        EntityDefinition entity;

        if (propDef.FunctionalType is PFTBackReferencedTable refTable)
        {
            entity = refTable.RelationshipReference.OwnerPropertyDefinition.OwnerDefinition;
        }
        else if (propDef.FunctionalType is PFTTablePart tblPart)
        {
            entity = tblPart.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition;
        }
        else
        {
            throw new GeneratorException($"Unsupported {nameof(PropertyFunctionalType)} ({propDef.FunctionalType.GetType().Name}) for collection property Id {propDef.Id}.");
        }

        var storageName = $"{NameHelper.NamesToPascalCase(entity.Names)}Storage";

        return storageName;
    }

    public CSPropertyDataTable(PropertyDefinition propDef, NaturalLanguageEnum preferNaturalLanguageForComments)
        : base(
            PropertyName(propDef),
            $"public virtual {GetPropertyType(propDef)} {PropertyName(propDef)} {{ get => StorageRegistry.Instance.{NameOfStorageClass(propDef)}.ReadAsTable({CSPropertyTableFilter.PropertyName(propDef)}); }}",
            new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names, preferNaturalLanguageForComments) + (preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? " (чтение таблицы)" : " (read table)"))
        )
    {
        _propertyDefinition = GuardRule.Forbid.Null(propDef, nameof(propDef));
    }

    public CSharpTypeBase Type
    {
        get
        {
            _type ??= GetPropertyType(_propertyDefinition);

            return _type;
        }
    }
}
