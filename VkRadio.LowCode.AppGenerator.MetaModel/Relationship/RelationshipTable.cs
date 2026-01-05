using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.MetaModel.Relationship;

/// <summary>
/// Relationship of type &quot;object and its related table&quot;
/// </summary>
public class RelationshipTable: Relationship
{
    bool _supportsHierarchy;
    PropertyDefinition.PropertyDefinition _propertyDefinitionInOwner;
    PropertyDefinition.PropertyDefinition _propertyDefinitionInTable;

    /// <summary>
    /// Relationship constructor
    /// </summary>
    /// <param name="id">Relationship Id</param>
    /// <param name="metaModel">MetaModel</param>
    public RelationshipTable(Guid id, MetaModel metaModel)
        : base(id, metaModel)
    {
    }

    /// <summary>
    /// Whether it supports hierarchies. Allows to have empty root owner
    /// </summary>
    public bool SupportsHierarchy { get { return _supportsHierarchy; } }
    /// <summary>
    /// Definition of a property that is an owner of a table part
    /// </summary>
    public PropertyDefinition.PropertyDefinition PropertyDefinitionInOwner { get { return _propertyDefinitionInOwner; } }
    /// <summary>
    /// Definition of a property that is a table part, that points to its owner (Foreign Key in a database)
    /// </summary>
    public PropertyDefinition.PropertyDefinition PropertyDefinitionInTable { get { return _propertyDefinitionInTable; } }

    /// <summary>
    /// Relationship type code
    /// </summary>
    public const string C_TYPE_CODE = "table";

    /// <summary>
    /// Load of concrete properties of a relationship from an XML node
    /// </summary>
    /// <param name="containingXel">XML node</param>
    protected override void LoadFromXElement(XElement containingXel)
    {
        var xel = containingXel.Element("SupportsHierarchy");
        _supportsHierarchy = xel is not null
            ? (bool)xel
            : false;

        var ownerPropertyDefinitionId = new Guid(containingXel.Element("PropertyDefinitionIdInOwner")!.Value);
        _propertyDefinitionInOwner = _metaModel.AllPropertyDefinitions[ownerPropertyDefinitionId];
        var ftTablePart = (PFTTablePart)_propertyDefinitionInOwner.FunctionalType;
        ftTablePart.RelationshipTable = this;

        var tablePropertyDefinitionId = new Guid(containingXel.Element("PropertyDefinitionIdInTable")!.Value);
        _propertyDefinitionInTable = _metaModel.AllPropertyDefinitions[tablePropertyDefinitionId];
        PFTTableOwner ftTableOwner = (PFTTableOwner)_propertyDefinitionInTable.FunctionalType;
        ftTableOwner.RelationshipTable = this;
        _propertyDefinitionInTable.FunctionalType.Nullable = _supportsHierarchy;

        // Deferred enrichment of dictionaries of property names. If some property had no name in some language, it is being extracted
        // from a data object type definition to that it points, if exists.
        NameDictionary.EnrichNamesForCollection(_propertyDefinitionInOwner.Names, _propertyDefinitionInTable.OwnerDefinition.Names);
        NameDictionary.EnrichNames(_propertyDefinitionInTable.Names, _propertyDefinitionInOwner.OwnerDefinition.Names);
    }
}
