using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Property functional type - connection between two single objects
/// </summary>
public class PFTConnector : PFTDependentLink
{
    private Relationship.RelationshipConnector _relationshipConnector = default!;

    protected override void SetDefaultOnDeleteAction()
    {
        OnDeleteAction = OnDeleteActionEnum.Ingnore;
    }

    public override string StringCode => C_STRING_CODE;

    /// <summary>
    /// Constructor
    /// </summary>
    public PFTConnector(bool? nullable, bool? unique, XElement xElement, DomainModel domainModel)
        : base(nullable, unique, xElement, domainModel)
    {
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

    public override (EntityDefinition EntityDefinition, bool IsTable) GetEntityDefinition()
    {
        var end = RelationshipConnector.End1.PropertyDefinition.Id == PropertyDefinition.Id
            ? RelationshipConnector.End2
            : RelationshipConnector.End1;

        return (end.PropertyDefinition.OwnerDefinition, false);
    }
}
