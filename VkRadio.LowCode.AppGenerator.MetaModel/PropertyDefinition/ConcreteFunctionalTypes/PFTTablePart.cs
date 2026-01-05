namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - table part
/// </summary>
public class PFTTablePart : PFTLink
{
    Relationship.RelationshipTable _relationshipTable;

    public PFTTablePart()
    {
        _stringCode = C_STRING_CODE;
    }

    /// <summary>
    /// Relationship
    /// </summary>
    public Relationship.RelationshipTable RelationshipTable { get { return _relationshipTable; } set { _relationshipTable = value; } }

    public const string C_STRING_CODE = "table";
}
