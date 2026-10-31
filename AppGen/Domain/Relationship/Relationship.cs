using Ardalis.GuardClauses;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.Relationship;

/// <summary>
/// Abstract relationship between data object types
/// </summary>
public abstract class Relationship : IUnique
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Id of a relationship</param>
    /// <param name="domainModel"></param>
    protected Relationship(Guid id, DomainModel domainModel)
    {
        Id = id;
        DomainModel = Guard.Against.Null(domainModel, nameof(domainModel));
    }

    /// <summary>
    /// Unique indentifier of a relationship between objects
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Domain Model
    /// </summary>
    public DomainModel DomainModel { get; private set; }

    /// <summary>
    /// Load relationship between data object types from an XML node
    /// </summary>
    /// <param name="domainModel">MetaModel</param>
    /// <param name="containingXel">XML node containing a description of relationship</param>
    /// <returns>Relationship between data object types</returns>
    public static Relationship LoadFromXElement(DomainModel domainModel, XElement containingXel)
    {
        // 1. Load base IUnique properties
        var id = new Guid(containingXel.Element("Id")!.Value);
        var relTypeCode = containingXel.Element("Type")!.Value;

        // 2. Load a description of a concrete relationship type
        Relationship rel = relTypeCode switch
        {
            RelationshipConnector.C_TYPE_CODE => new RelationshipConnector(id, domainModel, containingXel),
            RelationshipReference.C_TYPE_CODE => new RelationshipReference(id, domainModel, containingXel),
            RelationshipTable.C_TYPE_CODE => new RelationshipTable(id, domainModel, containingXel),
            _ => throw new ApplicationException(string.Format("Element Relationship Id {0} has unsupported Type - {1}.", id, relTypeCode ?? "<NULL>")),
        };

        return rel;
    }
}
