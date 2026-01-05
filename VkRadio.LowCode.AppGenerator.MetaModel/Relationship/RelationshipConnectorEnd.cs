using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.MetaModel.Relationship;

/// <summary>
/// One end of a relationship of type &quot;object-object&quot;
/// </summary>
public class RelationshipConnectorEnd
{
    RelationshipConnector _connector;
    RelationshipConnectorEnd _otherEnd;
    byte _endNum;
    //bool _navigable;
    //bool _nullable;
    PropertyDefinition.PropertyDefinition _propertyDefinition;

    /// <summary>
    /// Relationship to that it belongs
    /// </summary>
    public RelationshipConnector Connector { get { return _connector; } }
    /// <summary>
    /// An opposing end of relationship
    /// </summary>
    public RelationshipConnectorEnd OtherEnd { get { return _otherEnd; } set { _otherEnd = value; } }
    /// <summary>
    /// An order number of an end (1 or 2)
    /// </summary>
    public byte EndNum { get { return _endNum; } }
    /// <summary>
    /// Object to that it points, is visible from the opposite end
    /// </summary>
    //public bool Navigable { get { return _navigable; } }
    /// <summary>
    /// Object to that it points, can be missing
    /// </summary>
    //public bool Nullable { get { return _nullable; } }
    /// <summary>
    /// Property definition to that this end belongs
    /// </summary>
    public PropertyDefinition.PropertyDefinition PropertyDefinition { get { return _propertyDefinition; } }

    /// <summary>
    /// Load a relationship end from an XML node
    /// </summary>
    /// <param name="connector">Relationship</param>
    /// <param name="containingXel">XML node</param>
    /// <param name="endNum">Relationship end number (1 or 2)</param>
    /// <returns></returns>
    public static RelationshipConnectorEnd LoadFromXElement(RelationshipConnector connector, XElement containingXel, byte endNum)
    {
        //var xel = in_xel.Element("Navigable");
        //var navigable = xel != null ? (bool)xel : true;
        //xel = in_xel.Element("Nullable");
        //var nullable = xel != null ? (bool)xel : true;

        var pd = connector.MetaModel.AllPropertyDefinitions[new Guid(containingXel.Element("PropertyDefinitionId")!.Value)];

        var end = new RelationshipConnectorEnd
        {
            _connector = connector,
            _endNum = endNum,
            //_navigable = navigable,
            //_nullable = nullable,
            _propertyDefinition = pd
        };

        var ftConnector = (PFTConnector)pd.FunctionalType;
        ftConnector.RelationshipConnector = connector;
        
        return end;
    }
}
