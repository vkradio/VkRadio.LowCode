using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.Relationship;

/// <summary>
/// Relationship of type &quot;object-object&quot;
/// </summary>
public class RelationshipConnector: Relationship
{
    RelationshipConnectorEnd _end1;
    RelationshipConnectorEnd _end2;

    /// <summary>
    /// Relationship constructor
    /// </summary>
    /// <param name="id">Relationship Id</param>
    /// <param name="metaModel">MetaModel</param>
    public RelationshipConnector(Guid id, MetaModel metaModel)
        : base(id, metaModel)
    {
    }

    /// <summary>
    /// The first end of a relationship
    /// </summary>
    public RelationshipConnectorEnd End1 { get { return _end1; } }
    /// <summary>
    /// The second end of a relationship
    /// </summary>
    public RelationshipConnectorEnd End2 { get { return _end2; } }

    /// <summary>
    /// Get an end that opposes to the given one
    /// </summary>
    /// <param name="in_thisEnd">The connection end for which we search an opposing end</param>
    /// <returns>Opposing end</returns>
    public RelationshipConnectorEnd GetOtherEnd(RelationshipConnectorEnd in_thisEnd)
    {
        return _end1.PropertyDefinition.Id == in_thisEnd.PropertyDefinition.Id
            ? _end2
            : _end1;
    }

    /// <summary>
    /// Relationship type code
    /// </summary>
    public const string C_TYPE_CODE = "connector";

    /// <summary>
    /// Load a concrete properties of a relationship from an XML node
    /// </summary>
    /// <param name="containingXel">XML node</param>
    protected override void LoadFromXElement(XElement containingXel)
    {
        _end1 = RelationshipConnectorEnd.LoadFromXElement(this, containingXel.Element("End1"), 1);
        _end2 = RelationshipConnectorEnd.LoadFromXElement(this, containingXel.Element("End2"), 2);

        _end1.OtherEnd = _end2;
        _end2.OtherEnd = _end1;

        // Deferred mutual enrichment of name dictionaries. If some property had no name in some language,
        // that name is given from a data object type definition for that it points, if it was set for that
        // data object type.
        NameDictionary.EnrichNames(_end1.PropertyDefinition.Names, _end2.PropertyDefinition.OwnerDefinition.Names);
        NameDictionary.EnrichNames(_end2.PropertyDefinition.Names, _end1.PropertyDefinition.OwnerDefinition.Names);
    }
}
