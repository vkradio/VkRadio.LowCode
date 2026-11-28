using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional type of a property - collection (table) of objects, implicitly created from links for the corresponding object registry
/// </summary>
public class PFTBackReferencedTable : PFTLink, IPFTTable
{
    public override string StringCode => C_STRING_CODE;

    /// <summary>
    /// Collection constructor
    /// </summary>
    public PFTBackReferencedTable(bool? nullable, bool? unique, XElement xElement)
        : base(nullable, unique, xElement)
    {
    }

    /// <summary>
    /// Reference
    /// </summary>
    public Relationship.RelationshipReference RelationshipReference { get; set; } = default!;
    /*{
        get { return _relationshipReference; }

        set
        {
            _relationshipReference = value;

            // Extract them from a data object type definition, and then setting them as default names for
            // a current property
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
    }*/

    /// <summary>
    /// String code of a functional property type (used in a MetaModel file)
    /// </summary>
    public const string C_STRING_CODE = "back referenced table";

    public override (EntityDefinition EntityDefinition, bool IsTable) GetEntityDefinition() => (RelationshipReference.OwnerPropertyDefinition.OwnerDefinition, true);
}
