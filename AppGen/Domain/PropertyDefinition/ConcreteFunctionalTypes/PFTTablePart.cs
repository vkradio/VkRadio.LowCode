namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - table part
/// </summary>
public class PFTTablePart : PFTLink, IPFTTable
{
    public PFTTablePart()
    {
        _stringCode = C_STRING_CODE;
    }

    /// <summary>
    /// Relationship
    /// </summary>
    public Relationship.RelationshipTable RelationshipTable { get; set; }

    public const string C_STRING_CODE = "table";

    public override (EntityDefinition EntityDefinition, bool IsTable) GetEntityDefinition() => (RelationshipTable.PropertyDefinitionInTable.OwnerDefinition, true);
}
