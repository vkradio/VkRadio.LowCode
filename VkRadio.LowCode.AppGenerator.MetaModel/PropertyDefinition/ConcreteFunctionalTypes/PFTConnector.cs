using VkRadio.LowCode.AppGenerator.MetaModel.Relationship;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Property functional type - connection between two single objects
/// </summary>
public class PFTConnector : PFTLink, IPFTDependentLink
{
    Relationship.RelationshipConnector _relationshipConnector;

    /// <summary>
    /// Constructor
    /// </summary>
    public PFTConnector()
    {
        _stringCode = C_STRING_CODE;
    }

    /// <summary>
    /// Connection
    /// </summary>
    public Relationship.RelationshipConnector RelationshipConnector
    {
        get
        {
            return _relationshipConnector;
        }
        set
        {
            _relationshipConnector = value;

            // Setting default names of a property, extracting them from an opposing end of connection (data object type
            // definition of an object on that end of a connection)
            //_defaultNames.Clear();
            //
            //if (_relationshipConnector != null)
            //{
            //    RelationshipConnectorEnd end = _relationshipConnector.End1.PropertyDefinition.Id == _propertyDefinition.Id ?
            //        _relationshipConnector.End2 :
            //        _relationshipConnector.End1;
                
            //    foreach (var name in end.PropertyDefinition.OwnerDefinition.Names)
            //        _defaultNames.Add(name.Key, name.Value);
            //}
        }
    }

    /// <summary>
    /// String code of a functional property type (used in MetaModel files)
    /// </summary>
    public const string C_STRING_CODE = "object connector";

    public OnDeleteActionEnum OnDeleteAction { get; set; }
    public void SetDefaultOnDeleteAction()
    {
        OnDeleteAction = OnDeleteActionEnum.Ingnore;
    }
}
