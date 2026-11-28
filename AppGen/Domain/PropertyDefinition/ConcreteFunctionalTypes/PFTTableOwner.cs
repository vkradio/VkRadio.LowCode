using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - link to a table owner
/// </summary>
public class PFTTableOwner : PFTDependentLink
{
    private Relationship.RelationshipTable _relationshipTable = default!;

    protected override void SetDefaultOnDeleteAction()
    {
        OnDeleteAction = OnDeleteActionEnum.Delete;
    }

    public override string StringCode => C_STRING_CODE;

    public PFTTableOwner(bool? nullable, bool? unique, XElement xElement, DomainModel domainModel)
        : base(nullable, unique, xElement, domainModel)
    {
    }

    /// <summary>
    /// Relationship
    /// </summary>
    public Relationship.RelationshipTable RelationshipTable
    {
        get
        {
            return _relationshipTable;
        }
        set
        {
            _relationshipTable = value;

            //_defaultNames.Clear();
            //
            //if (_relationshipTable != null)
            //{
            //    DOTDefinition.DOTDefinition otherDOT = _relationshipTable.OwnerPropertyDefinition.Id == _propertyDefinition.Id
            //        ? _relationshipTable.TablePropertyDefinition.OwnerDefinition
            //        : _relationshipTable.OwnerPropertyDefinition.OwnerDefinition;
            //
            //    foreach (var name in otherDOT.Names)
            //    {
            //        _defaultNames.Add(name.Key, name.Value);
            //    }
            //}
        }
    }

    public const string C_STRING_CODE = "table owner";

    public override (EntityDefinition EntityDefinition, bool IsTable) GetEntityDefinition() => (RelationshipTable.PropertyDefinitionInOwner.OwnerDefinition, false);

    public void SetSupportshierarchy(bool supportshierarchy) => Nullable = supportshierarchy;
}
