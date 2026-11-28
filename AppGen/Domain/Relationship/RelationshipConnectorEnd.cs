using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.Domain.Relationship;

/// <summary>
/// One end of a relationship of type &quot;object-object&quot;
/// </summary>
public class RelationshipConnectorEnd
{
    /// <summary>
    /// Relationship to that it belongs
    /// </summary>
    public RelationshipConnector Connector { get; private set; }

    /// <summary>
    /// An opposing end of relationship
    /// </summary>
    public RelationshipConnectorEnd OtherEnd { get; set; } = default!;

    /// <summary>
    /// An order number of an end (1 or 2)
    /// </summary>
    public byte EndNum { get; private set; }

    /// <summary>
    /// Property definition to that this end belongs
    /// </summary>
    public PropertyDefinition.PropertyDefinition PropertyDefinition { get; private set; }

    public RelationshipConnectorEnd(RelationshipConnector connector, XElement containingXel, byte endNum)
    {
        var pd = connector.DomainModel.AllPropertyDefinitions[new Guid(containingXel.Element("PropertyDefinitionId")!.Value)];

        Connector = connector;
        EndNum = endNum;
        PropertyDefinition = pd;

        var ftConnector = (PFTConnector)pd.FunctionalType;
        ftConnector.RelationshipConnector = connector;
    }
}
