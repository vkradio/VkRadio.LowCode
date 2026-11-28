// TODO: Need to implement a value of type "tabled reference value" (and a corresponding relationship), that should be similar to a "reference value",
// but not as a single link, but a collection of links, such that a dictionary does not know about who references it (does not have a Foreign Key to the "owner")

using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - reference to an element of dictionary
/// </summary>
public class PFTReferenceValue : PFTDependentLink
{
    private Relationship.RelationshipReference _relationshipReference = default!;

    protected override void SetDefaultOnDeleteAction()
    {
        OnDeleteAction = OnDeleteActionEnum.CannotDelete;
    }

    public override string StringCode => C_STRING_CODE;

    public PFTReferenceValue(bool? nullable, bool? unique, XElement xElement, DomainModel domainModel)
        : base(nullable, unique, xElement, domainModel)
    {
    }

    /// <summary>
    /// Relationship
    /// </summary>
    public Relationship.RelationshipReference RelationshipReference
    {
        get
        {
            return _relationshipReference;
        }
        set
        {
            _relationshipReference = value;

            //_defaultNames.Clear();
            //
            //if (_relationshipReference != null)
            //{
            //    foreach (var name in _relationshipReference.ReferenceDefinition.Names)
            //    {
            //        _defaultNames.Add(name.Key, name.Value);
            //    }
            //}
        }
    }

    public const string C_STRING_CODE = "reference value";

    public override (EntityDefinition EntityDefinition, bool IsTable) GetEntityDefinition() => (RelationshipReference.ReferenceDefinition, false);
}
