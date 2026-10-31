using Ardalis.GuardClauses;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.Domain.Relationship;

/// <summary>
/// Relationship of type &quot;object and its related table&quot;
/// </summary>
public class RelationshipTable : Relationship
{
    /// <summary>
    /// Relationship constructor
    /// </summary>
    /// <param name="id">Relationship Id</param>
    /// <param name="domainModel">Domain Model</param>
    public RelationshipTable(Guid id, DomainModel domainModel, XElement containingXel)
        : base(id, domainModel)
    {
        Guard.Against.Null(containingXel, nameof(containingXel));

        var xel = containingXel.Element("SupportsHierarchy");
        SupportsHierarchy = xel is not null
            ? (bool)xel
            : false;

        var ownerPropertyDefinitionId = new Guid(containingXel.Element("PropertyDefinitionIdInOwner")!.Value);
        PropertyDefinitionInOwner = DomainModel.AllPropertyDefinitions[ownerPropertyDefinitionId];

        var ftTablePart = (PFTTablePart)PropertyDefinitionInOwner.FunctionalType;
        ftTablePart.RelationshipTable = this;

        var tablePropertyDefinitionId = new Guid(containingXel.Element("PropertyDefinitionIdInTable")!.Value);
        PropertyDefinitionInTable = DomainModel.AllPropertyDefinitions[tablePropertyDefinitionId];
        PFTTableOwner ftTableOwner = (PFTTableOwner)PropertyDefinitionInTable.FunctionalType;
        ftTableOwner.RelationshipTable = this;
        PropertyDefinitionInTable.FunctionalType.Nullable = SupportsHierarchy;

        // Deferred enrichment of dictionaries of property names. If some property had no name in some language, it is being extracted
        // from a data object type definition to that it points, if exists.
        NameDictionary.EnrichNamesForCollection(PropertyDefinitionInOwner.Names, PropertyDefinitionInTable.OwnerDefinition.Names);
        NameDictionary.EnrichNames(PropertyDefinitionInTable.Names, PropertyDefinitionInOwner.OwnerDefinition.Names);
    }

    /// <summary>
    /// Whether it supports hierarchies. Allows to have empty root owner
    /// </summary>
    public bool SupportsHierarchy { get; private set; } // => _supportsHierarchy;

    /// <summary>
    /// Definition of a property that is an owner of a table part
    /// </summary>
    public PropertyDefinition.PropertyDefinition PropertyDefinitionInOwner { get; private set; } // => _propertyDefinitionInOwner;

    /// <summary>
    /// Definition of a property that is a table part, that points to its owner (Foreign Key in a database)
    /// </summary>
    public PropertyDefinition.PropertyDefinition PropertyDefinitionInTable { get; private set; } // => _propertyDefinitionInTable;

    /// <summary>
    /// Relationship type code
    /// </summary>
    public const string C_TYPE_CODE = "table";
}
