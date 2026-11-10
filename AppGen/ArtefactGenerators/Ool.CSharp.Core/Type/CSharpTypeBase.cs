using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public abstract class CSharpTypeBase
{
    private static Dictionary<System.Type, (string CSharpTypeKeyword, bool IsReferenceType)> s_typeMappings;

    protected readonly string _typeName;

    static CSharpTypeBase()
    {
        s_typeMappings = new Dictionary<System.Type, (string CSharpTypeName, bool IsReferenceType)>
        {
            [typeof(PFTBoolean)] = (TypeConstants.Value.C_BOOL, false),
            [typeof(PFTDateTime)] = (TypeConstants.Value.C_DATETIME, false),
            [typeof(PFTDecimal)] = (TypeConstants.Value.C_DECIMAL, false),
            [typeof(PFTInteger)] = (TypeConstants.Value.C_INT, false),
            [typeof(PFTString)] = (TypeConstants.Reference.C_STRING, true),
            [typeof(PFTUniqueCode)] = (TypeConstants.Value.C_GUID, false)
        };
    }

    protected CSharpTypeBase(string typeName) => _typeName = Guard.Against.NullOrEmpty(typeName, nameof(typeName));

    public abstract string GenerateLiteral();

    public override string ToString() => GenerateLiteral();

    public static CSharpTypeBase DeriveFromPropertyFunctionalType(PropertyDefinition propertyDefinition, bool supportNullableRefTypes)
    {
        if (!(propertyDefinition.FunctionalType is PFTLink))
        {
            var type = propertyDefinition.FunctionalType.GetType();

            var (typeName, isReferenceType) = s_typeMappings
                .Where(kvp => kvp.Key.IsAssignableFrom(type))
                .Select(kvp => kvp.Value)
                .FirstOrDefault((string.Empty, false));

            if (typeName == string.Empty)
            {
                throw new GeneratorException($"Unknown value type for property Id {propertyDefinition.Id}: {propertyDefinition.FunctionalType.GetType().Name}.");
            }

            CSharpTypeBase result = isReferenceType
                ? new CSharpReferenceType(supportNullableRefTypes, typeName, propertyDefinition.FunctionalType.Nullable)
                : new CSharpValueType(typeName, propertyDefinition.FunctionalType.Nullable);

            return result;
        }
        else
        {
            EntityDefinition? entityDefinition = null;
            string? otherName = null;

            if (propertyDefinition.FunctionalType is PFTBackReferencedTable pftBackRefTable)
            {
                //var className = GetClassNameForDOT(((PropertyDefinition)pftBackRefTable.RelationshipReference.OwnerPropertyDefinition).OwnerDefinition);
                otherName = "DataTable";
            }
            else if (propertyDefinition.FunctionalType is PFTConnector pftConn)
            {
                var end = pftConn.RelationshipConnector.End1.PropertyDefinition.Id == propertyDefinition.Id
                    ? pftConn.RelationshipConnector.End2
                    : pftConn.RelationshipConnector.End1;

                entityDefinition = end.PropertyDefinition.OwnerDefinition;
            }
            else if (propertyDefinition.FunctionalType is PFTReferenceValue pftRefVal)
            {
                entityDefinition = pftRefVal.RelationshipReference.ReferenceDefinition;
            }
            else if (propertyDefinition.FunctionalType is PFTTableOwner pftTableOwner)
            {
                entityDefinition = pftTableOwner.RelationshipTable.PropertyDefinitionInOwner.OwnerDefinition;
            }
            else if (propertyDefinition.FunctionalType is PFTTablePart pftTable)
            {
                //var className = GetClassNameForDOT(pftTable.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition);
                //result = string.Format("IDictionary<Guid, {0}>", className);
                otherName = "DataTable";
            }
            else
            {
                throw new GeneratorException($"Unknown reference type for property Id {propertyDefinition.Id}: {propertyDefinition.FunctionalType.GetType().Name}.");
            }

            if (entityDefinition is null && otherName is null)
            {
                throw new GeneratorException($"Unknown reference type for property Id {propertyDefinition.Id}: {propertyDefinition.FunctionalType.GetType().Name}.");
            }

            string className;
            bool isNullable;

            if (entityDefinition is not null)
            {
                className = TypeHelper.GetClassNameForEntity(entityDefinition);
                isNullable = propertyDefinition.FunctionalType.Nullable;
            }
            else
            {
                className = otherName!;
                isNullable = false;
            }

            var result = new CSharpReferenceType(supportNullableRefTypes, className, isNullable);

            return result;
        }
    }
}
