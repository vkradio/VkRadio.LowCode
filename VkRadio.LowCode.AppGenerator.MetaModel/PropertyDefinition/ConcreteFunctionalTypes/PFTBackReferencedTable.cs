namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional type of a property - collection (table) of objects, implicitly created from links for the corresponding object registry
/// </summary>
public class PFTBackReferencedTable : PFTLink
{
    Relationship.RelationshipReference _relationshipReference;

    /// <summary>
    /// Collection constructor
    /// </summary>
    public PFTBackReferencedTable()
    {
        _stringCode = C_STRING_CODE;
    }

    /// <summary>
    /// Reference
    /// </summary>
    public Relationship.RelationshipReference RelationshipReference
    {
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
    }

    /// <summary>
    /// String code of a functional property type (used in a MetaModel file)
    /// </summary>
    public const string C_STRING_CODE = "back referenced table";
}
