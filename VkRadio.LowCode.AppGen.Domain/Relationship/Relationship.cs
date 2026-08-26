using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.Relationship;

/// <summary>
/// Abstract relationship between data object types
/// </summary>
public abstract class Relationship : IUnique
{
    protected Guid _id;
    protected DomainModel _domainModel;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Id of a relationship</param>
    /// <param name="metaModel"></param>
    protected Relationship(Guid id, DomainModel metaModel)
    {
        _id = id;
        _domainModel = metaModel;
    }

    /// <summary>
    /// Loading of additional concrete relationshiop properties from an XML node
    /// </summary>
    /// <param name="containingXel">XML node containing a description of relationship</param>
    protected abstract void LoadFromXElement(XElement containingXel);

    /// <summary>
    /// Unique indentifier of a relationship between objects
    /// </summary>
    public Guid Id { get => _id; set => _id = value; }

    /// <summary>
    /// MetaModel
    /// </summary>
    public DomainModel DomainModel => _domainModel;

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
            RelationshipConnector.C_TYPE_CODE => new RelationshipConnector(id, domainModel),
            RelationshipReference.C_TYPE_CODE => new RelationshipReference(id, domainModel),
            RelationshipTable.C_TYPE_CODE => new RelationshipTable(id, domainModel),
            _ => throw new ApplicationException(string.Format("Element Relationship Id {0} has unsupported Type - {1}.", id, relTypeCode ?? "<NULL>")),
        };

        rel.LoadFromXElement(containingXel);

        return rel;
    }
}
