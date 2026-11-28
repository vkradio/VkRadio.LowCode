using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - table part
/// </summary>
public class PFTTablePart : PFTLink, IPFTTable
{
    public override string StringCode => C_STRING_CODE;

    public PFTTablePart(bool? nullable, bool? unique, XElement xElement)
        : base(nullable, unique, xElement)
    {
    }

    /// <summary>
    /// Relationship
    /// </summary>
    public Relationship.RelationshipTable RelationshipTable { get; set; } = default!;

    public const string C_STRING_CODE = "table";

    public override (EntityDefinition EntityDefinition, bool IsTable) GetEntityDefinition() => (RelationshipTable.PropertyDefinitionInTable.OwnerDefinition, true);
}
