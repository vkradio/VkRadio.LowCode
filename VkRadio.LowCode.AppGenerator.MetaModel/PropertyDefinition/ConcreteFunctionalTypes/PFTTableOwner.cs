namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - link to a table owner
/// </summary>
public class PFTTableOwner : PFTLink, IPFTDependentLink
{
    Relationship.RelationshipTable _relationshipTable;

    public PFTTableOwner()
    {
        _stringCode = C_STRING_CODE;
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

    public OnDeleteActionEnum OnDeleteAction { get; set; }
    public void SetDefaultOnDeleteAction()
    {
        OnDeleteAction = OnDeleteActionEnum.Delete;
    }
}
