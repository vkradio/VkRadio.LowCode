using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.Relationship;

/// <summary>
/// Abstract relationship between data object types
/// </summary>
public abstract class Relationship : IUnique
{
    protected Guid _id;
    protected MetaModel _metaModel;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Id of a relationship</param>
    /// <param name="metaModel"></param>
    protected Relationship(Guid id, MetaModel metaModel)
    {
        _id = id;
        _metaModel = metaModel;
    }

    /// <summary>
    /// Loading of additional concrete relationshiop properties from an XML node
    /// </summary>
    /// <param name="containingXel">XML node containing a description of relationship</param>
    protected abstract void LoadFromXElement(XElement containingXel);

    /// <summary>
    /// Unique indentifier of a relationship between objects
    /// </summary>
    public Guid Id { get { return _id; } set { _id = value; } }

    /// <summary>
    /// MetaModel
    /// </summary>
    public MetaModel MetaModel { get { return _metaModel; } }

    /// <summary>
    /// Load relationship between data object types from an XML node
    /// </summary>
    /// <param name="metaModel">MetaModel</param>
    /// <param name="containingXel">XML node containing a description of relationship</param>
    /// <returns>Relationship between data object types</returns>
    public static Relationship LoadFromXElement(MetaModel metaModel, XElement containingXel)
    {
        // 1. Load base IUnique properties
        var id = new Guid(containingXel.Element("Id")!.Value);
        var relTypeCode = containingXel.Element("Type")!.Value;

        // 2. Load a description of a concrete relationship type
        Relationship rel = relTypeCode switch
        {
            RelationshipConnector.C_TYPE_CODE => new RelationshipConnector(id, metaModel),
            RelationshipReference.C_TYPE_CODE => new RelationshipReference(id, metaModel),
            RelationshipTable.C_TYPE_CODE => new RelationshipTable(id, metaModel),
            _ => throw new ApplicationException(string.Format("Element Relationship Id {0} has unsupported Type - {1}.", id, relTypeCode ?? "<NULL>")),
        };

        rel.LoadFromXElement(containingXel);

        return rel;
    }
}
