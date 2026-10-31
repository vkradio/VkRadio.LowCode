using Ardalis.GuardClauses;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.Relationship;

/// <summary>
/// Relationship of type &quot;object-object&quot;
/// </summary>
public class RelationshipConnector : Relationship
{
    private const string errNoEndDefinedForRelationship = "No <End{0}> defined for relationship {1}";
    /// <summary>
    /// Relationship constructor
    /// </summary>
    /// <param name="id">Relationship Id</param>
    /// <param name="domainModel">Domain Model</param>
    public RelationshipConnector(Guid id, DomainModel domainModel, XElement containingXel)
        : base(id, domainModel)
    {
        Guard.Against.Null(containingXel, nameof(containingXel));

        End1 = RelationshipConnectorEnd.LoadFromXElement(
            this,
            containingXel.Element("End1") ?? throw new ApplicationException(string.Format(errNoEndDefinedForRelationship, "1", id)),
            1
        );
        End2 = RelationshipConnectorEnd.LoadFromXElement(
            this,
            containingXel.Element("End2") ?? throw new ApplicationException(string.Format(errNoEndDefinedForRelationship, "2", id)),
            2
        );

        End1.OtherEnd = End2;
        End2.OtherEnd = End1;

        // Deferred mutual enrichment of name dictionaries. If some property had no name in some language,
        // that name is given from a data object type definition for that it points, if it was set for that
        // data object type.
        NameDictionary.EnrichNames(End1.PropertyDefinition.Names, End2.PropertyDefinition.OwnerDefinition.Names);
        NameDictionary.EnrichNames(End2.PropertyDefinition.Names, End1.PropertyDefinition.OwnerDefinition.Names);
    }

    /// <summary>
    /// The first end of a relationship
    /// </summary>
    public RelationshipConnectorEnd End1 { get; private set; }

    /// <summary>
    /// The second end of a relationship
    /// </summary>
    public RelationshipConnectorEnd End2 { get; private set; }

    /// <summary>
    /// Get an end that opposes to the given one
    /// </summary>
    /// <param name="in_thisEnd">The connection end for which we search an opposing end</param>
    /// <returns>Opposing end</returns>
    public RelationshipConnectorEnd GetOppositeEnd(RelationshipConnectorEnd in_thisEnd)
    {
        return End1.PropertyDefinition.Id == in_thisEnd.PropertyDefinition.Id
            ? End2
            : End1;
    }

    /// <summary>
    /// Relationship type code
    /// </summary>
    public const string C_TYPE_CODE = "connector";
}
